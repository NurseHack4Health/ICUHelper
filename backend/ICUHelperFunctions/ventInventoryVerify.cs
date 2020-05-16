using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace ICUHelperFunctions
{
    public static class ventInventoryVerify
    {
        [FunctionName("ventInventoryVerify")]
        public static async Task RunAsync([TimerTrigger("0 0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var vent=false;
            //Missing get ventilators here 


            ///Get the administrator email
            var adminEmail="";
            var adminName = "";

            if (vent==true) {

                string sendgridApi = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                string fromEmail = Environment.GetEnvironmentVariable("FROMEMAIL");
                string fromUser = Environment.GetEnvironmentVariable("FROMUSER");

                var client = new SendGridClient(sendgridApi);
                var from = new EmailAddress(fromEmail, fromUser);
                var subject = "Ventilators are running out!";
                var to = new EmailAddress(adminEmail, adminName);
                var plainTextContent = "";
                var htmlContent = ""; //MISSING TEMPLATE
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }


            //else return ""

           



        }
    }
}
