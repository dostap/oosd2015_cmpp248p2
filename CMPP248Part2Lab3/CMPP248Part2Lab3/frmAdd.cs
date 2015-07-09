using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMPP248Part2Lab3
{
    public partial class frmAdd : Form
    {
        public frmAdd()
        {
            InitializeComponent();
        }

        private Salesperson salesperson = null;

        //return salesperson created
        public Salesperson GetNewSalesperson()
        {
            this.ShowDialog();
            return salesperson;
        }

        private bool IsValidData()
        {
            return Validator.IsPresent(txtName) &&

                   Validator.IsInt32(txtAgencyID) &&
                   Validator.IsValidID(txtAgencyID) &&

                   Validator.IsDouble(txtCommission) &&
                   

                   Validator.IsDecimal(txtSalesAmount) &&
                   Validator.IsPos(txtSalesAmount) &&
                   Validator.IsPos(txtCommission) ;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtCommission.Text = "";
            txtAgencyID.Text = "";
            txtSalesAmount.Text = "";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsValidData()) //validate all data before saving
            {
                salesperson = new Salesperson(txtName.Text,
                    Convert.ToInt32(txtAgencyID.Text), 
                    Convert.ToDouble(txtCommission.Text),
                    Convert.ToDecimal(txtSalesAmount.Text));
                this.Close();
            }
        }


        
    }
}
