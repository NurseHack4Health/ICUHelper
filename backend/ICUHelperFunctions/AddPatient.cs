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
using System.Text.Json.Serialization;

namespace ICUHelperFunctions


{
    public static class AddPatient
    {


        [FunctionName("AddPatient")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Patient auxObj = new Patient();
           

            log.LogInformation("C# HTTP trigger function processed a request.");

            auxObj.fullName = req.Query["fullName"];
            auxObj.phone = req.Query["phone"];
            auxObj.emergencyContact = req.Query["emergencyContact"];
            auxObj.phoneEmergencyContact = req.Query["phoneEmergencyContact"];
            auxObj.idNumber = Int32.Parse(req.Query["idNumber"]);
            auxObj.idType = Int32.Parse(req.Query["idType"]);
            auxObj.gender = Int32.Parse(req.Query["genderId"]);

            DateTime fecha = Convert.ToDateTime(req.Query["dob"]);


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";

            if (string.IsNullOrEmpty(req.Query["fullName"]))
            {
                responseMessage = "{\"result\":\" parameter missing in your request\"}";
                return new OkObjectResult(responseMessage);

            }
            else
            {


                string writeResult = WriteToDB(auxObj,fecha);

                if (writeResult == "inserted patient into DB")
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




        public static string  WriteToDB(Patient objPatient, DateTime fecha)
        {

            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            
            int result = 0;
            using (SqlConnection connection = new SqlConnection("")
            {
                String query = "insert into [dbo].[users](full_name, phone,emergency_contact,phone_emergency_contact,gender_id,date_of_birth,identification_number,identificaton_type)values(@full_name, @phone, @emergency_contact, @phone_emergency_contact, @gender_id, @date_of_birth, @identification_number, @identificaton_type); ";
                var sqlFormattedDate = objPatient.dob.Date.ToString("yyyy-MM-dd HH:mm:ss");
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        
                        command.Parameters.AddWithValue("@full_name", objPatient.fullName);
                        command.Parameters.AddWithValue("@phone", objPatient.phone);
                        command.Parameters.AddWithValue("@emergency_contact", objPatient.emergencyContact);
                        command.Parameters.AddWithValue("@phone_emergency_contact", objPatient.phoneEmergencyContact);
                        command.Parameters.AddWithValue("@gender_id", objPatient.gender);
                        command.Parameters.AddWithValue("@date_of_birth", fecha);
                        command.Parameters.AddWithValue("@identification_number", objPatient.idNumber);
                        command.Parameters.AddWithValue("@identificaton_type", objPatient.idType);


                        connection.Open();
                        result = command.ExecuteNonQuery();

                    if (result > 0)
                    {

                        return "inserted patient into DB";
                    }

                    else {

                        return "no patient added";
                    }


                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Error inserting data into Database!");
                        return "error inserting into DB";
                        //return result;
                    }


                






                }

              

            }

        }
    }



}