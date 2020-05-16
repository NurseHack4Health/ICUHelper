using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class RolType
    {
        public RolType()
        {
            Rol = new HashSet<Rol>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<Rol> Rol { get; set; }
    }
}
