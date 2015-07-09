using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMPP248lab2
{
    public partial class frmProductMaintenance : Form
    {
        public frmProductMaintenance()
        {
            InitializeComponent();
        }

        private Product product; //create a Product object only accessible by this form

        //for Get Product button
        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtProductCode))
            {
                string productCode = txtProductCode.Text;
                this.GetProduct(productCode);
                if (product == null)
                {
                    MessageBox.Show("No product found with this code. " +
                         "Please try again.", "Product Not Found");
                    this.ClearControls(); //reset form fields to empty 
                }
                else
                {
                    this.DisplayProduct();//populate fields with data
                    btnModify.Enabled = true;
                    btnClear.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
         }

        //GetProduct method - accepts product code
        private void GetProduct(string productCode)
        {
            try
            {
                product = ProductDB.GetProduct(productCode); //returns a product object populated by data from the database
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString());
            }
        }

        private void ClearControls()//resets the form
        {
            txtProductCode.Text = "";
            txtDesc.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            btnModify.Enabled = false;
            btnClear.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void DisplayProduct()
        {
            txtProductCode.Text  = product.ProductCode;
            txtDesc.Text = product.Description;
            txtPrice.Text = product.UnitPrice.ToString("c");//display in textbox as currency
            txtQuantity.Text = product.OnHandQuantity.ToString();
            btnModify.Enabled = true;
            btnClear.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddModifyProduct addProductForm = new frmAddModifyProduct();  //instantiate new Add form
            addProductForm.addProduct = true; //set bool to true that we are adding a product
            //this is used to differentiate adding and modifying (form heading)

            DialogResult result = addProductForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                product = addProductForm.product; //grab the object from the Add/Modify Product form
               // txtProductCode.Text = product.ProductCode.ToString();
                this.DisplayProduct(); // populate textboxes with data from the newly created product
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            frmAddModifyProduct modProductForm = new frmAddModifyProduct();  //instantiate new Add form
            modProductForm.addProduct = false; //set bool to false that we are NOT adding a product, but modifying
            //this is used to differentiate adding and modifying (form heading)

            modProductForm.product = product; // populates modify form with the product info
            DialogResult result = modProductForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                product = modProductForm.product;
                this.DisplayProduct();
            }
            else if (result == DialogResult.Retry)
            {
                this.GetProduct(product.ProductCode);
                if (product != null)
                    this.DisplayProduct();
                else
                    this.ClearControls();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Delete " + product.ProductCode + "?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    if (!ProductDB.DeleteProduct(product))
                    {
                        MessageBox.Show("Another user has updated or deleted " +
                            "that product.", "Database Error");
                        this.GetProduct(product.ProductCode);
                        if (product != null)
                            this.DisplayProduct();
                        else
                            this.ClearControls();
                    }
                    else
                        this.ClearControls();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) //clears form for all inputs/textboxes
        {
            this.ClearControls();
        }

        private void btnExit_Click(object sender, EventArgs e) //closes the form 
        {
            this.Close();
        } 

    }
}
