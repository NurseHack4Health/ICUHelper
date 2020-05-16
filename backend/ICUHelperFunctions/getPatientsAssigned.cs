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
    public static class getPatientsAssigned
    {
        [FunctionName("getPatientsAssigned")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            HealthCareWorker auxObj = new HealthCareWorker();



            auxObj.userId = Int32.Parse(req.Query["idNumber"]);



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";

            if (string.IsNullOrEmpty(req.Query["idNumber"]))
            {
                responseMessage = "{\"result\":\" parameter missing in your request\"}";
                return new OkObjectResult(responseMessage);

            }
            else
            {


                string writeResult = searchDB(auxObj);

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




        public static string searchDB(HealthCareWorker objPatient)
        {

            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            int result = 0;
            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                String query = "get the patient list given doc ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {

                        command.Parameters.AddWithValue("@user_id", objPatient.userId);



                        connection.Open();
                        result = command.ExecuteNonQuery();

                        if (result > 0)
                        {

                            return "inserted patient into DB";
                        }

                        else
                        {

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

