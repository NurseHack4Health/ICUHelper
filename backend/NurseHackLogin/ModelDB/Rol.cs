using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class Rol
    {
        public int RolTypeId { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }

        public virtual RolType RolType { get; set; }
        public virtual Users User { get; set; }
    }
}
