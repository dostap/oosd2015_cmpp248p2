using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cmpp248lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.productsBindingSource.EndEdit(); //accept edit by user
                this.tableAdapterManager.UpdateAll(this.northwndDataSet); //update data set
            }

            catch (NoNullAllowedException exNull)
            {
                MessageBox.Show(exNull.Message, "No nulls allowed!");
                this.productsBindingSource.CancelEdit(); // stop and cancel edit/update
            }

            catch (ArgumentException exarg) // check exceptions in arguments (e.g. max length)
            //exception thrown when argument is outside the range of acceptable values
            {
                MessageBox.Show(exarg.Message, "Argument Exception");
                this.productsBindingSource.CancelEdit(); // stop and cancel edit/update
            }

            catch (DBConcurrencyException)
            //thrown thrown by the DataAdapter during an insert, update, or delete operation if the number of rows affected equals zero
            //caused by a concurrency violation
            {
                MessageBox.Show("A concurrency exception happened. " +
                    "Some rows were not updated", "Concurrency Exception");
                this.productsTableAdapter.Fill(this.northwndDataSet.Products); // reload the products data
            }

            catch (DataException exdata) // thrown when errors are generated using ADO.NET components
            {
                MessageBox.Show("ADO.NET error #: "
                    + exdata.Message, exdata.GetType().ToString());
                this.productsBindingSource.CancelEdit(); // stop and cancel edit/update
            }

            catch (SqlException exsql)
            //exception created whenever the .NET Framework Data Provider for SQL Server encounters an error generated from the server
            //here, we are calling it exsql to access sqlexception class properties and methods
            //reports provider-specific errors
            {
                MessageBox.Show("Database error #: " + exsql.Number +
                    ": " + exsql.Message, exsql.GetType().ToString());
            }

            catch (Exception ex)
            //exceptions generic exceptions from the common language runtime
            {
                MessageBox.Show("An error occurred: " + ex.Message);

            }
                 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //check for exceptions encountered when loading the form 
            try
            {
                // this loads the Categories table
                this.categoriesTableAdapter.Fill(this.northwndDataSet.Categories);
                // this loads the Suppliers table
                this.suppliersTableAdapter.Fill(this.northwndDataSet.Suppliers);
                // this loads the Order_Details table
                this.order_DetailsTableAdapter.Fill(this.northwndDataSet.Order_Details);
                // this loads the Products table
                this.productsTableAdapter.Fill(this.northwndDataSet.Products);
            }
            catch (SqlException exsql)
            //exception created whenever the .NET Framework Data Provider for SQL Server encounters an error generated from the server
            //here, we are calling it exsql to access sqlexception class properties and methods
            //reports provider-specific errors
            {
                MessageBox.Show("Database error #" + exsql.Number + ": " + exsql.Message,
                                exsql.GetType().ToString());
            }
            catch (Exception ex)
            //exceptions generic exceptions from the common language runtime
            {
                MessageBox.Show("An error occurred: '" + ex.ToString());
            }

        }
    }
}
