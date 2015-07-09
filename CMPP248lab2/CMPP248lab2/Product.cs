using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMPP248lab2
{
    public class Product
    {
        //default constructor
        public Product() { }
        //declare publc properties (auto-implement)
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int OnHandQuantity { get; set; }
    }
}

