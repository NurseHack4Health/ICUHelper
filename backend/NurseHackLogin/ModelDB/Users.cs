using System;
using System.Collections.Generic;

namespace NurseHackLogin.ModelDB
{
    public partial class Users
    {
        public Users()
        {
            Rol = new HashSet<Rol>();
            UserAuth = new HashSet<UserAuth>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public int IdentificationNumberId { get; set; }
        public string Phone { get; set; }
        public string EmergencyContact { get; set; }
        public string PhoneEmergencyContact { get; set; }
        public int GenderId { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual IdentificationNumber IdentificationNumber { get; set; }
        public virtual ICollection<Rol> Rol { get; set; }
        public virtual ICollection<UserAuth> UserAuth { get; set; }
    }
}
