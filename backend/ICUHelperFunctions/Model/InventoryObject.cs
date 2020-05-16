using System;
using System.Collections.Generic;
using System.Text;

namespace ICUHelperFunctions.Model
{
    public class InventoryObject
    {
        public string sku { get; set; }

        public  string name { get; set; }

        public string description { get; set; }

        public int inventoryNumber { get; set; }

        public int isdepleted { get; set; }

        public int addedNumber { get; set; }


    }
}
