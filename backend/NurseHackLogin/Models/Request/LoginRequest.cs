using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NurseHackLogin.Models.Request
{
    public class LoginRequest
    {
        [JsonProperty("user", Required = Required.Always)]
        public string User { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }

        public DateTime PasswordExpiration { get; set; }

    }
}
