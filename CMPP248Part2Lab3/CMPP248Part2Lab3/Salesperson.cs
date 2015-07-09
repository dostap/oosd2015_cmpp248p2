using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMPP248Part2Lab3
{
    public class Salesperson
    {
        public Salesperson()
		{
		}

        public Salesperson(string Name, int AgencyID, double Commission, decimal SalesAmount)
        {
            this.Name = Name;
            this.AgencyID = AgencyID;
            this.Commission = Commission;
            this.SalesAmount = SalesAmount;
        }

		public string Name { get; set; }

		public int AgencyID { get; set; }

		public double Commission { get; set; }

        public decimal SalesAmount { get; set; }

		public string GetDisplayText(string sep)
		{
			return Name + sep + AgencyID.ToString() + sep + Commission.ToString("c") + sep + SalesAmount.ToString();
		}
    }
}
