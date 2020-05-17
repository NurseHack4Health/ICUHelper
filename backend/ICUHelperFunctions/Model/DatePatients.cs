using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace ICUHelperFunctions
{
    public class DatePatients
    {   

        public int history_number { get; set; }

        public  string full_name { get; set; }

        public string patient_gender { get; set; }

        public string patient_condition { get; set; }

        public string using_ventilator { get; set; }


    }
}
