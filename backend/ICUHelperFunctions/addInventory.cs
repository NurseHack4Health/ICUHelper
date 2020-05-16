using System;
using System.IO;
using System.Threading.Tasks;
using System.Text; 
using System.Web.Http; 
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ICUHelperFunctions.Model;


namespace ICUHelperFunctions
{
    public static class addInventory
    {
        [FunctionName("addInventory")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {         
             log.LogInformation("C# HTTP trigger function insert inventory");
            ResponseAddInventory response= new ResponseAddInventory(); 

            try
            {
                InventoryObject auxObj = new InventoryObject();
                auxObj.sku = req.Query["sku"];
                auxObj.name = req.Query["name"];
                auxObj.description = req.Query["description"];
                auxObj.inventoryNumber = Int32.Parse(req.Query["quantity"]); 

                 response.id_supplier = WriteToDB(auxObj);     

                if( response.id_supplier != 0 )
                {
                    response.code= 200; 
                    response.message= "success";
                    
                    log.LogInformation("Insert succes");
                    //string responseMessage = JsonConvert.SerializeObject(avaibles); 
                    return new OkObjectResult(response);
                }
                else 
                {
                    response.code= 101; 
                    response.message= "Problem with the insert";
                     log.LogInformation("Problem with the insert");
                    return new ObjectResult(response); 
                }
            }
            catch(Exception ex)
            {
                log.LogInformation("Error" + ex.Message);
                return new ExceptionResult (ex, true); 
            }


            
        }


        public static int WriteToDB(InventoryObject objInventory)
        {
             string cnnString = "Server=nursehack.database.windows.net;Database=nursehackdb;Integrated Security=False;User ID=isacalderon;Password=SuperSecret!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"; 
             int idInsert= 0; 
             StringBuilder sBquery = new StringBuilder();
             sBquery.Append("DECLARE @return_value int ");
             sBquery.AppendFormat("EXEC @return_value = dbo.validate_supplies_insert  @sku_in= {0}, @name_in= {1}, @description_in= {2}, @inventory_in= {3},  @id_out= null ", 
                                   objInventory.sku, objInventory.name, objInventory.description, objInventory.inventoryNumber );
             sBquery.Append("SELECT	'Return Value' = @return_value ");
           
            string query= sBquery.ToString(); 
            SqlDataReader dataReader; 
            using (SqlConnection connection = new SqlConnection(cnnString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                        try
                        {
                            connection.Open(); 
                             dataReader = command.ExecuteReader();

                              while (dataReader.Read())
                              {                                    
                                    idInsert= dataReader.GetInt32(0); 
                              }
                

                        }
                        catch(Exception ex)
                        {                           
                            throw; 
                        }
                }
            }

            return idInsert; 

        }
    }



}