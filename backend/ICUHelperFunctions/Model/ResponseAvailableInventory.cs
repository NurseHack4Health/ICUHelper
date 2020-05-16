using System;
using System.Collections.Generic;
using System.Text;

namespace ICUHelperFunctions.Model
{
    public class ResponseAvailableInventory
    {
        public int code { get; set; }

        public  string message { get; set; }

        public List<InventoryAvaible> inventory { get; set; }


    }
}
