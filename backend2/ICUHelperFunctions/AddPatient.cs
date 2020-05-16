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
using System.Text.Json.Serialization;

namespace ICUHelperFunctions
{
    public static class AddPatient
    {
        [FunctionName("AddPatient")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            Patient auxObj = new Patient();

            log.LogInformation("C# HTTP trigger function processed a request.");
            auxObj.fullName = req.Query["fullName"];
            auxObj.phone = req.Query["phone"];
            auxObj.emergencyContact = req.Query["emergencyContact"];
            auxObj.phoneEmergencyContact = req.Query["phoneEmergencyContact"];
           // auxObj.dob = req.Query["dob"];
            auxObj.idNumber = Int32.Parse( req.Query["idNumber"]);
            auxObj.idType = Int32.Parse(req.Query["idType"]);
            auxObj.gender= Int32.Parse(req.Query["gender"]); 

            //auxObj.fullName = req.Query["fullName"];
             
             DateTime fecha= Convert.ToDateTime(req.Query["dob"]); 
             auxObj.dob = req.Query["dob"]; //fecha.ToString("yyyy-MM-dd HH:mm:ss"); 
            //DateTime.Parse(req.Query["dob"]).ToString("yyyy-MM-dd HH:mm:ss"); 
         
            //auxObj.patientId= Int32.Parse(req.Query["patientId"]);
          //  auxObj.phone = req.Query["phone"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";

            if (string.IsNullOrEmpty(req.Query["fullName"]))
            {
                responseMessage = "{\"result\":\" parameter missing in your request\"}";
                return new OkObjectResult(responseMessage);

            }
            else
            {


                int writeResult = WriteToDB(auxObj);

                if (writeResult >= 0)
                {
                    responseMessage = "{\"result\":\"wrote " + writeResult + " record(s) to db\"}";
                    return new OkObjectResult(responseMessage);
                }
                else
                {
                    responseMessage = "{\"result\":\"error # " + writeResult + " when uploading data\"}";
                    return new OkObjectResult(responseMessage);
                }

                // return new OkObjectResult(responseMessage);
            }
            // string test = req.Query["userId"];
            // string test2= req.Query["name"]; 
            // string test3= req.Query["phone"]; 
            // string test4= req.Query["dob"];  
          //  string test2= req.Query["name"]; 

             
        //     auxObj.patientId= Int32.Parse(req.Query["patientId"]);

        //     string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //     dynamic data = JsonConvert.DeserializeObject(requestBody);
        //     //auxObj.userID = auxObj.userID ?? data?.email;
        //   //  fever = fever ?? data?.fever;
        //    // cough = cough ?? data?.cough;

        //     string responseMessage = "";

        //     if (string.IsNullOrEmpty(req.Query["userId"]))
        //     {
        //         responseMessage = "{\"result\":\"email parameter missing in your request\"}";
        //         return new OkObjectResult(responseMessage);

        //     }
        //     else
        //     {


        //         int writeResult = WriteToDB(auxObj);

        //         if (writeResult >= 0)
        //         {
        //             responseMessage = "{\"result\":\"wrote " + writeResult + " record(s) to db\"}";
        //             return new OkObjectResult(responseMessage);
        //         }
        //         else
        //         {
        //             responseMessage = "{\"result\":\"error # " + writeResult + " when uploading data\"}";
        //             return new OkObjectResult(responseMessage);
        //         }

        //         // return new OkObjectResult(responseMessage);
        //     }
        }




        public static int WriteToDB( Patient objPatient)
        {

            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");

          //  Console.WriteLine(cnnString);
             int result=0;
            using (SqlConnection connection = new SqlConnection("Server=tcp:nursehack.database.windows.net,1433;Initial Catalog=nursehackdb;Persist Security Info=False;User ID=alerico;Password=Albus19878712;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                //String query = "insert into [dbo].[patient] (user_id,condition_id) values (@userId,@conditionId)";
                 String query = "insert into [dbo].[users](full_name, phone,emergency_contact,phone_emergency_contact,gender_id,date_of_birth,identification_number,identificaton_type) values (@full_name, @phone, @emergency_contact, @phone_emergency_contact, @gender_id, @date_of_birth, @identification_number, @identificaton_type); ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {

                        command.Parameters.AddWithValue("@full_name", objPatient.fullName);
                        command.Parameters.AddWithValue("@phone", objPatient.phone);
                        command.Parameters.AddWithValue("@emergency_contact", objPatient.emergencyContact);
                        command.Parameters.AddWithValue("@phone_emergency_contact", objPatient.phoneEmergencyContact);
                        command.Parameters.AddWithValue("@gender_id", objPatient.gender);
                        command.Parameters.AddWithValue("@date_of_birth", objPatient.dob);
                        command.Parameters.AddWithValue("@identification_number", objPatient.idNumber);
                        command.Parameters.AddWithValue("@identificaton_type", objPatient.idType);
                        

                        connection.Open();
                       result = command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Error inserting data into Database!");
                        //return result;
                    }
                    

                       // return result;
                    

                    
                  
                   

                }

                return result;
            }

        }
    }



}

