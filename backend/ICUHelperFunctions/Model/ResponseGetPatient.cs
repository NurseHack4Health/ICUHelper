using System;
using System.Collections.Generic;
using System.Text;

namespace ICUHelperFunctions.Model
{
    public class ResponseGetPAtient
    {
        public int code { get; set; }

        public  string message { get; set; }

        public Patient paciente { get; set; }


    }
}
