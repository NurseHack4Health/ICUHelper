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
using System.Net.Http;
using System.Net;
using System.Text;

namespace ICUHelperFunctions
{
    public static class getTokenPowerBi
    {
        [FunctionName("getTokenPowerBi")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = "dfsdf";
            string token="";
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

           string response= getPowerBiToken(token);

            string responseMessage = string.IsNullOrEmpty(token)
                ? response
                : token;

            var jsonToReturn = JsonConvert.SerializeObject(responseMessage);
            //var ontent = new StringContent(jsonToReturn, Encoding.UTF8, "application/json");

          //  return new OkObjectResult(new StringContent(jsonToReturn, Encoding.UTF8, "application/json"));


            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
            };
        }




        public static string getPowerBiToken(string token)
        {

            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            int result = 0;
            using (SqlConnection connection = new SqlConnection("Server=tcp:nursehack.database.windows.net,1433;Initial Catalog=nursehackdb;Persist Security Info=False;User ID=alerico;Password=Albus19878712;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                String query = "SELECT* FROM[dbo].[powerbi_token]";




                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {


                        //var aux;
                        connection.Open();
                        result = command.ExecuteNonQuery();

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                  token = reader[0].ToString();
                                var whatEver = reader[1];
                                // get the rest of the columns you need the same way
                            }

                           // token = aux;
                        }




                        if (result < 0)
                        {

                            return token;
                        }

                        else
                        {

                            return "couldn't retrieve token";
                        }


                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Error inserting data into Database!");
                        return "error getting token";
                        //return result;
                    }









                }



            }

        }



    }









}
