using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ICUHelperFunctions
{
    public class UserAdmin
    {
        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("fullName")]
        public string fullName { get; set; }
    }
}
