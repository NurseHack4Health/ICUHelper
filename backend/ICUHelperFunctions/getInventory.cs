using System;
using System.IO;
using System.Text; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using System.Web.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ICUHelperFunctions.Model;
using System.Data.SqlClient;
using SendGrid; 

namespace ICUHelperFunctions
{
    public static class getInventory
    {
        [FunctionName("getInventory")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function Get inventory");
            ResponseAvailableInventory response= new ResponseAvailableInventory(); 

            try
            {
               response.inventory = await ReadInventoryDB();   

                if( response.inventory.Count >0 )
                {
                    response.code= 200; 
                    response.message= "success";
                    
                log.LogInformation("C# HTTP trigger function Get inventory");
                    //string responseMessage = JsonConvert.SerializeObject(avaibles); 
                    return new OkObjectResult(response);
                }
                else 
                {
                    response.code= 101; 
                    response.message= "No found dates on DB";
                    return new ObjectResult(response); 
                }
            }
            catch(Exception ex)
            {
                return new ExceptionResult (ex, true); 
            }

         
        }

        public static async Task<List<InventoryAvaible>> ReadInventoryDB()        
        {
            List<InventoryAvaible> avaibles= new List<InventoryAvaible>(); 
           
            //string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");
             string cnnString = "Server=nursehack.database.windows.net;Database=nursehackdb;Integrated Security=False;User ID=isacalderon;Password=SuperSecret!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"; 
             StringBuilder sBquery = new StringBuilder();
             sBquery.Append("WITH  supplies_inventory as ( SELECT s.id, s.sku, s.name, s.inventory as deposit  FROM dbo.supplies s), ");
             sBquery.Append("supplies_in_use as ( SELECT ps.supplies_id, COUNT (id) as in_use   FROM dbo.patient_supplies ps  GROUP BY ps.supplies_id) "); 
             sBquery.Append("SELECT si.sku,si.name,  SUM(si.deposit - suin.in_use) as supplies_avalible FROM supplies_inventory si   JOIN supplies_in_use suin on (suin.supplies_id=si.id) GROUP BY si.sku,si.name; "); 

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
                                    InventoryAvaible item= new InventoryAvaible(); 
                                    item.name= dataReader["name"].ToString(); 
                                    item.sku= dataReader["sku"].ToString(); 
                                    item.supplies_avaible= dataReader.GetInt32(2); 

                                    avaibles.Add(item); 
                              }
                

                        }
                        catch(Exception ex)
                        {
                            avaibles= null; 
                            throw ex; 
                        }
                }
            }



             return avaibles; 
        }

        public static async Task AlarmLowSupplies()        
        {            
                
            // string sendgridApi = Environment.GetEnvironmentVariable("SENDGRID_API_KEY.");
            // var client = new SendGridClient(sendgridApi);
            // var from = new EmailAddress("alerico90@gmail.com", "Example User");
            // var subject = "Sending with SendGrid is Fun";
            // var to = new EmailAddress("alerico90@gmail.com", "Example User");
            // var plainTextContent = "and easy to do any"; 
            
            
        }
    }
}
