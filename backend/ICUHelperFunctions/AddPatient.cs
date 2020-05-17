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
using System.Text;

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
            auxObj.idNumber = Int32.Parse(req.Query["idNumber"]);
            auxObj.idType = Int32.Parse(req.Query["idType"]);
            auxObj.gender = Int32.Parse(req.Query["gender"]);

            //auxObj.fullName = req.Query["fullName"];
            //var sqlFormattedDate = objPatient.dob.Date.ToString("yyyy-MM-dd HH:mm:ss");
            DateTime fecha = Convert.ToDateTime(req.Query["dob"]);
            auxObj.dob =fecha; //fecha.ToString("yyyy-MM-dd HH:mm:ss"); 
                               //DateTime.Parse(req.Query["dob"]).ToString("yyyy-MM-dd HH:mm:ss"); 

            Random random = new Random();
            // Any random integer   
            int num = random.Next();
            auxObj.patientId = num;

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

        }




        public static int WriteToDB(Patient objPatient)
        {

            string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");

            //  Console.WriteLine(cnnString);
            int result = 0;
            using (SqlConnection connection = new SqlConnection("Server=tcp:nursehack.database.windows.net,1433;Initial Catalog=nursehackdb;Persist Security Info=False;User ID=alerico;Password=Albus19878712;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                var sqlFormattedDate = objPatient.dob.Date.ToString("yyyy-MM-dd HH:mm:ss");
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
                        command.Parameters.AddWithValue("@date_of_birth", sqlFormattedDate);
                        command.Parameters.AddWithValue("@identification_number", objPatient.idNumber);
                        command.Parameters.AddWithValue("@identificaton_type", objPatient.idType);
                        command.Parameters.AddWithValue("@history_number",objPatient.patientId);

                        connection.Open();
                        result = command.ExecuteNonQuery();
                        connection.Close();


                    }
                    catch (Exception e)
                    {

                        Console.WriteLine("Error inserting data into Database!");
                        //return result;
                    }
                }

                int idInsert = 0;
                StringBuilder sBquery = new StringBuilder();
                sBquery.Append("DECLARE @return_value int ");
                sBquery.AppendFormat("EXEC	@return_value = [dbo].[new_patient] @history_number={0}, @patientid_out= null ",
                                      objPatient.patientId);
                sBquery.Append("SELECT	'Return Value' = @return_value ");
                 query = sBquery.ToString();
                SqlDataReader dataReader;
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            idInsert = dataReader.GetInt32(0);
                        }


                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }





                return result;
            }

        }
    }



}
