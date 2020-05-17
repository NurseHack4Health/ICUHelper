using System;
using System.IO;
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
            UtilsQuerys utilsQuerys= new UtilsQuerys(); 
            var patient= utilsQuerys.GetPatient(hitoryNumber);    
             return new ObjectResult(patient); 

           }
           catch(Exception ex)
           {
                return new ExceptionResult (ex, true); 

           }
           
        }




       

    }
        
}

