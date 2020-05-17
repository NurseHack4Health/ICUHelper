using System;
using System.IO;
using System.Text; 
using System.Web.Http; 
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
           try
           {

            int hitoryNumber= Int32.Parse(req.Query["historyNumber"]);   
            // UtilsQuerys utilsQuerys= new UtilsQuerys(); 
            var patient= GetPatient(hitoryNumber);    
             return new ObjectResult(patient); 

           }
           catch(Exception ex)
           {
                return new ExceptionResult (ex, true); 

           }
           
        }

          public static DatePatients GetPatient(int history) 
        {
            DatePatients patients= new DatePatients(); 

            StringBuilder sbQuery= new StringBuilder(); 
            sbQuery.Append("SELECT p.history_number, s.full_name, g.value as patient_gender, c.value as patient_condition, p.using_ventilator"); 
            sbQuery.Append(" FROM dbo.patient p JOIN dbo.users s on (s.id=p.user_id) JOIN dbo.conditions c on (c.id=p.condition_id) JOIN dbo.gender g on (s.gender_id=g.id)"); 
            sbQuery.AppendFormat(" WHERE p.history_number = {0}", history); 

           
            string cnnString= "Server=tcp:nursehack.database.windows.net,1433;Initial Catalog=nursehackdb;Persist Security Info=False;User ID=alerico;Password=Albus19878712;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; 
               
                try
                {
                    SqlConnection connection = new SqlConnection(cnnString); 
                    SqlCommand command = new SqlCommand(sbQuery.ToString(), connection); 
                    connection.Open(); 
                    SqlDataReader data = command.ExecuteReader();
                    while (data.Read())
                    {
                        patients.history_number=Convert.ToInt32( data["history_number"]); 
                        patients.full_name= data["full_name"].ToString(); 
                        patients.patient_condition= data["patient_condition"].ToString(); 
                        patients.patient_gender= data["patient_gender"].ToString(); 
                        patients.using_ventilator=data["using_ventilator"].ToString(); 
                    }

                    data.Close();   

                }
                catch(Exception ex)
                {
                    throw ex; 
                }                  

         

            return patients; 
        }

    }
        
}

