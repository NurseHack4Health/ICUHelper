using System;
using System.Collections.Generic;
using System.Text;

namespace ICUHelperFunctions
{
    public class HealthCareWorker
    {
        public int userId { get; set; }

        public int specialtyId { get; set; }

        public string licenseNumber { get; set; }

        public User objUser { get; set; }
    }
}
