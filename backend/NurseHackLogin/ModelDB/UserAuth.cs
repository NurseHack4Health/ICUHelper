using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class UserAuth
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual Users User { get; set; }
    }
}
