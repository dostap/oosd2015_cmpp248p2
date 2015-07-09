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
    public partial class frmAddModifyProduct : Form
    {
        public frmAddModifyProduct()
        {
            InitializeComponent();
        }
        //declare variables
        public bool addProduct;
        public Product product;

        private void frmAddModifyProduct_Load(object sender, EventArgs e)
        {
            //this.LoadStateComboBox();
            if (addProduct) //if vaue is set to true - we are adding
            {
                this.Text = "Add Product";
            }
            else //if not adding, we must be modifying. limited options here.
            {
                this.Text = "Modify Product Information";
                this.DisplayProduct(); //populate form fields with product information
            }
        }


        private void DisplayProduct()
        {
            txtProductCode.Text = product.ProductCode;
            txtDesc.Text = product.Description;
            txtPrice.Text = product.UnitPrice.ToString("F2");
            txtQuantity.Text = product.OnHandQuantity.ToString();
            

        }

        private void PutProductData(Product product)//basically the opposite of DiplayProduct
            //this grabs data from the textboxes and assigns it to variables
        {
            product.ProductCode = txtProductCode.Text;
            product.Description = txtDesc.Text;
            product.UnitPrice = Convert.ToDecimal(txtPrice.Text);
            product.OnHandQuantity = Convert.ToInt32(txtQuantity.Text);
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (IsDataPresent())
            {
                if (addProduct) // check Boolean flag for adding a product
                {
                    product = new Product(); //new instance of class Product
                    this.PutProductData(product); //call PutProductData method to populate product with user's inputs
                    try
                    {
                        ProductDB.AddProduct(product); //returns a product object populated by data from the database
                        MessageBox.Show("Product '" + product.Description + "' added!");//send user a message that the product has been added
                        //for peace of mind....
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
                else
                {
                    Product newProduct = new Product(); //if we are modifying
                    //create new instance of Product
                    newProduct.ProductCode = product.ProductCode; //let newProduct get the properties of the product we are modiyfing
                    this.PutProductData(newProduct); //display properties of the product user is modifying
                    try
                    {
                        if (!ProductDB.ModProduct(product, newProduct))
                        {
                            MessageBox.Show("Another user has updated or " +
                                "deleted that product.", "Database Error");
                            this.DialogResult = DialogResult.Retry;
                        }
                        else
                        {
                            product = newProduct;
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsDataPresent()
        {
            return
                Validator.IsPresent(txtProductCode) &&
                Validator.IsPresent(txtDesc) &&
                Validator.IsPresent(txtPrice) &&
                Validator.IsDecimal(txtPrice) &&
                Validator.IsInt32(txtQuantity) &&
                Validator.IsPresent(txtQuantity);
        }



    }
}
