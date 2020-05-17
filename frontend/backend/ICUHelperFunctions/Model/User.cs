using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ICUHelperFunctions
{
    public class User
    {
        [JsonPropertyName("fullName")]
        public string fullName { get; set; }

        [JsonPropertyName("idNumber")]
        public int idNumber { get; set; }

        [JsonPropertyName("idType")]
        public int idType { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }

        [JsonPropertyName("emergencyContact")]
        public string emergencyContact { get; set; }

        [JsonPropertyName("phoneEmergencyContact")]
        public string phoneEmergencyContact { get; set; }

        [JsonPropertyName("gender")]
        public int gender { get; set; }

        [JsonPropertyName("dob")]
        public DateTime dob { get; set; }

    }
}
