

using System;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace simulated_device
{
    class SimulatedDevice
    {
        private static DeviceClient s_deviceClient;
        private const int sPO2Threshold = 90;
        private const int respirationRateThreshold = 21;
        private static Random s_randomGenerator = new Random();
    
        // The device connection string to authenticate the device with your IoT hub.
        // Using the Azure CLI:
        // az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id MyDotnetDevice --output table
        private readonly static string s_connectionString = "HostName=alanguloTestIotNH.azure-devices.net;DeviceId=MyDotnetDevice;SharedAccessKey=Kd2n501U0DhxIcmXVSTTR10N5gdF7TzEWxhdzx+esVM=";

        // Async method to send simulated telemetry
        private static async void SendDeviceToCloudMessagesAsync()
        {

            // _temperature = s_randomGenerator.Next(20, 35);
            // _sPO2 = s_randomGenerator.Next(70, 99);
            // _respirationRate = s_randomGenerator.Next(13, 28);



            while (true)
            {
                double currentSPO2 = s_randomGenerator.Next(70, 99);
                double currentRespirationRate = s_randomGenerator.Next(13, 28);
                double currentTemperature = s_randomGenerator.Next(36, 38);

                // Create JSON message
                var telemetryDataPoint = new
                {
                    sPO2 = currentSPO2,
                    temperature = currentTemperature,
                    respirationRate = currentRespirationRate
                };
                var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                // Add a custom application property to the message.
                // An IoT hub can filter on these properties without access to the message body.
                message.Properties.Add("sPO2Alert", (currentSPO2 < sPO2Threshold) ? "true" : "false");
                message.Properties.Add("respiratoryRateAlert", ((currentRespirationRate > respirationRateThreshold) || (currentRespirationRate < 13)) ? "true" : "false");
                message.Properties.Add("temperatureAlert", (currentTemperature > respirationRateThreshold || currentTemperature < respirationRateThreshold) ? "true" : "false");

                // Send the telemetry message
                await s_deviceClient.SendEventAsync(message);
                Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

                await Task.Delay(1000);
            }
        }
        private static void Main(string[] args)
        {
            Console.WriteLine("IoT Hub Quickstarts #1 - Simulated device. Ctrl-C to exit.\n");

            // Connect to the IoT hub using the MQTT protocol
            s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
            SendDeviceToCloudMessagesAsync();
            Console.ReadLine();
        }
    }
}