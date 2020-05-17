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
    public class UtilsQuerys
    {
        private SqlCommand command; 
        public UserAdmin getUserAdmin() 
        {
            StringBuilder sbQuery= new StringBuilder(); 
            sbQuery.Append("SELECT ua.email, u.full_name FROM rol r JOIN rol_type rt on (rt.id=r.rol_type_id) JOIN users u on (u.id=r.user_id) JOIN user_auth ua on (ua.user_id=u.id) WHERE rt.id=3; "); 
            SqlDataReader data = connectionBD(sbQuery.ToString()); 
            UserAdmin admin= new UserAdmin(); 

            while (data.Read())
            {
                admin.email= data["email"].ToString(); 
               admin.fullName=  data["full_name"].ToString();
            }
            data.Close(); 
            return admin; 
        }

          public StringBuilder getTemplateEmail(string event_name, string param= " " ) 
        {
            StringBuilder sbQuery= new StringBuilder(); 
            sbQuery.AppendFormat("SELECT et.template FROM dbo.email_templates et WHERE et.event_name=@event_name;", event_name); 
            SqlDataReader data= connectionBD(sbQuery.ToString()); 
            StringBuilder htmlEmail= new StringBuilder(); 

            while (data.Read())
            {
                if(!param.Equals(" "))
                    htmlEmail.AppendFormat( data["template"].ToString(), param); 
                else 
                   htmlEmail.Append( data["template"].ToString());
            }
            data.Close(); 

            return htmlEmail; 
        }


        private  SqlDataReader connectionBD(string query)        
        {        
              //string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");
             string cnnString = "Server=nursehack.database.windows.net;Database=nursehackdb;Integrated Security=False;User ID=isacalderon;Password=SuperSecret!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"; 
                  
                try
                {
                    SqlConnection connection = new SqlConnection(cnnString); 
                    command = new SqlCommand(query, connection); 
                    connection.Open(); 
                    SqlDataReader data = command.ExecuteReader();
                    return data;     

                }
                catch(Exception ex)
                {
                    throw ex; 
                }        
        }


    }
}