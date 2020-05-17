using System;
using System.IO;
using System.Text; 
using System.Collections.Generic;
using System.Threading.Tasks;  
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Web.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ICUHelperFunctions.Model;
using System.Data.SqlClient;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ICUHelperFunctions
{
    public static class inventoryReport
    {
        [FunctionName("inventoryReport")]
        public static void Run([TimerTrigger("0 0 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            List<InventoryAvaible> lowInventor=  ReadLowInventoryDB(); 

            if(lowInventor.Count > 0)
            {
                // send Email 
                string json_lowInventor= JsonConvert.SerializeObject(lowInventor); 

                UtilsQuerys uq= new UtilsQuerys(); 
                UserAdmin user= uq.getUserAdmin();
   
                 string sendgridApi = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                string fromEmail = Environment.GetEnvironmentVariable("FROMEMAIL");
                string fromUser = Environment.GetEnvironmentVariable("FROMUSER");

                var adminEmail= user.email;
                var adminName = user.fullName;

                var client = new SendGridClient(sendgridApi);
                var from = new EmailAddress(fromEmail, fromUser);
                var subject = "Alarm we need more supplies";
                var to = new EmailAddress(adminEmail, adminName);
                //var plainTextContent = json_lowInventor;
                StringBuilder strHtml= uq.getTemplateEmail("low_supplies", json_lowInventor); 
                string htmlContent= strHtml.ToString(); 
                var msg = MailHelper.CreateSingleEmail(from, to, subject, " ", htmlContent);
                Task<Response> response =  client.SendEmailAsync(msg);
                response.Wait(); 
                var rep= response.Result; 


            }
            else 
            {
                 log.LogInformation($"We have enough supplies!! ");

            }
        }

        public static bool SendEmail(string lowInventor )
        {

                return true; 
        }

        public static List<InventoryAvaible> ReadLowInventoryDB()        
        {
            List<InventoryAvaible> lowAlarm= new List<InventoryAvaible>(); 
           
            //string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");
             string cnnString = "Server=nursehack.database.windows.net;Database=nursehackdb;Integrated Security=False;User ID=isacalderon;Password=SuperSecret!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"; 
             StringBuilder sBquery = new StringBuilder();
             sBquery.Append("WITH  supplies_inventory as ( SELECT s.id, s.sku, s.name, s.inventory as deposit  FROM dbo.supplies s), ");
             sBquery.Append("supplies_in_use as ( SELECT ps.supplies_id, COUNT (id) as in_use   FROM dbo.patient_supplies ps  GROUP BY ps.supplies_id) "); 
             sBquery.Append("SELECT si.sku,si.name,  SUM(si.deposit - suin.in_use) as supplies_avalible FROM supplies_inventory si   JOIN supplies_in_use suin on (suin.supplies_id=si.id) GROUP BY si.sku,si.name "); 
             sBquery.Append("HAVING SUM(si.deposit - suin.in_use) < 10"); 

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

                                    lowAlarm.Add(item); 
                              }
                        }
                        catch(Exception ex)
                        {
                            lowAlarm= null; 
                            throw ex; 
                        }
                }
            }

             return lowAlarm; 
        }

    }
}
