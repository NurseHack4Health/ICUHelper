using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
