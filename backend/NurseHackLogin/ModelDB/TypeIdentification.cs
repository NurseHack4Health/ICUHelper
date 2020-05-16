using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class TypeIdentification
    {
        public TypeIdentification()
        {
            IdentificationNumber = new HashSet<IdentificationNumber>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public virtual ICollection<IdentificationNumber> IdentificationNumber { get; set; }
    }
}
