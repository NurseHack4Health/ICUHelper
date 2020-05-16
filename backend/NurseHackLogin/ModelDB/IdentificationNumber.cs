using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class IdentificationNumber
    {
        public IdentificationNumber()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public int? TypeIdentificationId { get; set; }
        public long? Value { get; set; }

        public virtual TypeIdentification TypeIdentification { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
