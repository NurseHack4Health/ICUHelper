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

        public bool updatePatient(int upadatetype, int id, int newvalue)
        {
            DatePatients patients= new DatePatients(); 
            StringBuilder sbQuery= new StringBuilder(); 

            switch (upadatetype)
            {
                case 1://Ventilator, needs to also add or get one ventilator from the  inventory
                     sbQuery.AppendFormat("UPDATE dbo.Patient SET using_ventilator = {0} Where id = {1};", newvalue, id); 
                    break;

                case 2: //Treatment
                     sbQuery.AppendFormat("UPDATE dbo.Patient SET condition_id={0} Where id = {1};", newvalue, id); 
                    break;

                case 3: //Symptoms
                    sbQuery.AppendFormat("UPDATE dbo.Patient SET history_number={0} Where id = {1};", newvalue, id); 
                    break;

            } 

            SqlDataReader data = connectionBD(sbQuery.ToString());  
            data.Close(); 

            return true; 

        }

         public DatePatients GetPatient(int history) 
        {
            DatePatients patients= new DatePatients(); 
            StringBuilder sbQuery= new StringBuilder(); 
            sbQuery.Append("SELECT p.history_number, s.full_name, g.value as patient_gender, c.value as patient_condition, p.using_ventilator"); 
            sbQuery.Append(" FROM dbo.patient p JOIN dbo.users s on (s.id=p.user_id) JOIN dbo.conditions c on (c.id=p.condition_id) JOIN dbo.gender g on (s.gender_id=g.id)"); 
            sbQuery.AppendFormat(" WHERE p.history_number = {0}", history); 

            SqlDataReader data = connectionBD(sbQuery.ToString());            

            while (data.Read())
            {
               patients.history_number=Convert.ToInt32( data["history_number"]); 
               patients.full_name= data["full_name"].ToString(); 
               patients.patient_condition= data["patient_condition"].ToString(); 
               patients.patient_gender= data["patient_gender"].ToString(); 
               patients.using_ventilator=data["using_ventilator"].ToString(); 
            }

            data.Close(); 

            return patients; 
        }
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
            sbQuery.AppendFormat("SELECT et.template FROM dbo.email_templates et WHERE et.event_name='{0}';", event_name); 
            SqlDataReader data= connectionBD(sbQuery.ToString()); 
            StringBuilder htmlEmail= new StringBuilder(); 
            try{

            while (data.Read())
            {
                if(!param.Equals(" "))
                    htmlEmail.Append( data.GetValue(0).ToString()) ; 
                else 
                   htmlEmail.Append( data["template"].ToString());
            }
            data.Close(); 
            
            } catch(Exception ex)
            {


            }
            return htmlEmail; 
        }

          public int getVentilators() 
        {
            StringBuilder sbQuery= new StringBuilder(); 
            sbQuery.Append("WITH  ventilator_inventory  as ( SELECT s.inventory as avalible, 1 as org  FROM dbo.supplies s  WHERE name like ('%Ventilator%')), "); 
            sbQuery.Append(" ventilator_in_use  as ( SELECT COUNT (id) as in_use, 1 as org  from dbo.patient p  WHERE p.using_ventilator=1 ) "); 
            sbQuery.Append(" SELECT avalible-in_use FROM ventilator_inventory vi JOIN ventilator_in_use vu on (vi.org=vu.org)"); 
            SqlDataReader data = connectionBD(sbQuery.ToString()); 
            int totalVentiladores= 0; 

            while (data.Read())
            {
                totalVentiladores= data.GetInt32(0);  
            }
            data.Close(); 
            return totalVentiladores; 
        }


        private  SqlDataReader connectionBD(string query)        
        {        
            //    string cnnString = Environment.GetEnvironmentVariable("DB_CONNECTION");
            string cnnString= "Server=nursehack.database.windows.net;Database=nursehackdb;Integrated Security=False;User ID=isacalderon;Password=SuperSecret!;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"; 
               
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