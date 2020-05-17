using System;
using System.Text; 
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace ICUHelperFunctions
{
    public static class ventInventoryVerify
    {
        [FunctionName("ventInventoryVerify")]
        public static async Task RunAsync([TimerTrigger("0 0 */6 * * *")]TimerInfo myTimer, ILogger log)
       
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            UtilsQuerys uq= new UtilsQuerys(); 
            int vent=  uq.getVentilators(); 
            //Missing get ventilators here 
            if(vent <= 5 )
            {
                 UserAdmin user= uq.getUserAdmin();
   
                var adminEmail= user.email;
                var adminName = user.fullName;         

                string sendgridApi = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                string fromEmail = Environment.GetEnvironmentVariable("FROMEMAIL");
                string fromUser = Environment.GetEnvironmentVariable("FROMUSER");
                  
                var client = new SendGridClient(sendgridApi);
                var from = new EmailAddress(fromEmail, fromUser);
                var subject = "Ventilators are running out!";
                var to = new EmailAddress(adminEmail, adminName);
                StringBuilder strHtml= uq.getTemplateEmail("low_ventilators"); 
                //var plainTextContent = "";
                var htmlContent = strHtml.ToString(); //MISSING TEMPLATE
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
                var response = await client.SendEmailAsync(msg);

            }
            else
            {
                log.LogInformation($"there are enough ventilator");
            }
        }
    }
}
