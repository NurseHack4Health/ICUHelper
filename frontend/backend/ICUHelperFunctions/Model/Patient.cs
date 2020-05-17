using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace ICUHelperFunctions
{
    public class Patient
    {
   
       // public User ObjUser { get; set; }

        public int patientId { get; set; }

        public  DateTime dateIngress { get; set; }

        public int conditionId { get; set; }

        public DateTime releaseDate { get; set; }

        public int userID { get; set; }

        public int isInVent { get; set; }

        public List<string> symptoms { get; set; }

        public List<string> medications { get; set; }

        public string condition { get; set; }

        public int possibleVentilator { get; set; }

        public string fullName { get; set; }

        public int idNumber { get; set; }

        public int idType { get; set; }

        public string phone { get; set; }

        public string emergencyContact { get; set; }

        public string phoneEmergencyContact { get; set; }

        public int gender { get; set; }

        public DateTime dob { get; set; }


    }
}
