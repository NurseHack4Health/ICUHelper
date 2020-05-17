using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using ICUHelperFunctions.Model;

namespace ICUHelperFunctions
{
    public static class updatePatient
    {

        //POST With updateType and valueToUpdate
        [FunctionName("updatePatient")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            int updateType;
            int newvalue; 
            int idPatient; 
             ResponseupdatePatient response= new ResponseupdatePatient(); 

            try{
            
                updateType = Int32.Parse(req.Query["updateType"]);      
                newvalue= Int32.Parse(req.Query["newvalue"]);   
                idPatient= Int32.Parse(req.Query["idPatient"]);   
                UtilsQuerys utilsQuerys= new UtilsQuerys(); 
                if(utilsQuerys.updatePatient(updateType, idPatient, newvalue))
                {
                    response.code= 200; 
                    response.message= "success"; 
                      return new OkObjectResult(response);


                }
                else
                {
                    response.code= 109; 
                    response.message= "error update"; 
                      return new OkObjectResult(response);

                }
            }
            catch(Exception ex)
            {
                 return new ExceptionResult (ex, true); 
            }

          

        }



       

    }
}
