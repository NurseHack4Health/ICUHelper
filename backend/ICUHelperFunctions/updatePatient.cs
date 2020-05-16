using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace ICUHelperFunctions
{
    public static class updatePatient
    {

        //POST With updateType and valueToUpdate
        [FunctionName("updatePatient")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {


            int updateType;
            Patient objPatient = new Patient();


            updateType = Int32.Parse(req.Query["updateType"]);


            switch (updateType)
            {
                case 1://Ventilator, needs to also add or get one ventilator from the  inventory

                    objPatient.isInVent = Int32.Parse(req.Query["valueToUpdate"]);

                    break;

                case 2: //Treatment

                    objPatient.medications = req.Query["valueToUpdate"];

                    break;

                case 3: //Symptoms

                    objPatient.symptoms = req.Query["valueToUpdate"];

                    break;

                case 4: //Status

                    objPatient.condition= req.Query["valueToUpdate"];
                    break;

            }



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";

            if (string.IsNullOrEmpty(req.Query["valueToUpdate"]) || string.IsNullOrEmpty(req.Query["updateType"]))
            {
                responseMessage = "{\"result\":\" parameter missing in your request\"}";
                return new OkObjectResult(responseMessage);

            }
            else
            {


                string writeResult = updatePatientData(objPatient, updateType);

                if (writeResult == "updated patient into DB")
                {
                    responseMessage = writeResult;
                    return new OkObjectResult(responseMessage);
                }
                else
                {
                    responseMessage = writeResult;
                    return new OkObjectResult(responseMessage);
                }

            }

        }



        public static string updatePatientData(Patient objPatient, int updateType)
        {

            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            int result = 0;

            using (SqlConnection connection = new SqlConnection(cnnString))
            {

                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        switch (updateType)
                        {
                            case 1://Ventilator, needs to also add or get one ventilator from the  inventory

                                command.Parameters.AddWithValue("@using_ventilator", objPatient.isInVent);
                                command.Parameters.AddWithValue("@using_ventilator", objPatient.isInVent);
                                command.CommandText = "UPDATE [dbo].[Patient] SET using_ventilator = @using_ventilator Where userId = @userId"; //doesn't use userId

                                break;

                            case 2: //Treatment

                                command.Parameters.AddWithValue("@using_ventilator", objPatient.isInVent);
                                command.Parameters.AddWithValue("@using_ventilator", objPatient.isInVent);
                                command.CommandText = "UPDATE [dbo].[Patient] SET using_ventilator = @using_ventilator Where userId = @userId"; //doesn't use userId

                                break;

                            case 3: //Symptoms

                                break;

                            case 4: //Status

                                break;

                        }
                        


                        

                        connection.Open();
                        result = command.ExecuteNonQuery();

                        if (result > 0)
                        {

                            return "updated patient into DB";
                        }

                        else
                        {

                            return "no patient updated";
                        }


                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Error updating data into Database!");
                        return "error updating DB";
                        //return result;
                    }









                }



            }

        }

    }
}
