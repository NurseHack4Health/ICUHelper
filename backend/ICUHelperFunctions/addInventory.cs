using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ICUHelperFunctions.Model;
using System.Data.SqlClient;

namespace ICUHelperFunctions
{
    public static class addInventory
    {
        [FunctionName("addInventory")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            InventoryObject auxObj = new InventoryObject();


            log.LogInformation("C# HTTP trigger function processed a request.");

            auxObj.sku = req.Query["sku"];
            auxObj.addedNumber = Int32.Parse( req.Query["quantity"]);
           



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";

            if (string.IsNullOrEmpty(req.Query["sku"]))
            {
                responseMessage = "{\"result\":\" parameter missing in your request\"}";
                return new OkObjectResult(responseMessage);

            }
            else
            {


                string writeResult = WriteToDB(auxObj);

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




        public static string WriteToDB(InventoryObject objInventory)
        {
            int auxItemQuantity;
            InventoryObject queryObj = new InventoryObject();
            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            int result = 0;
            using (SqlConnection connection = new SqlConnection("Server=tcp:nursehack.database.windows.net,1433;Initial Catalog=nursehackdb;Persist Security Info=False;User ID=alerico;Password=Albus19878712;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                String query1 = "SELECT * FROM [dbo].[supplies] WHERE sku = '53808-0627' ";

                String query2 = " ";




                auxItemQuantity = queryObj.inventoryNumber + objInventory.addedNumber;

                using (SqlCommand command = new SqlCommand(query1, connection))
                {
                    try
                    {
                     

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