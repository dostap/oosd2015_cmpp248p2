using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CMPP248Part2Lab3
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private static string Title = "Error Occurred";
        private static string path = @"Salespeople.xml";

        public List<Salesperson> salespeople = new List<Salesperson>();
        //create a global list of Salespeople, initialize with null

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                salespeople = SalespersonDB.GetSalesperson();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found: " + path, Title);
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message + " IOException", Title);
            }
            catch (FormatException fex)
            {
                MessageBox.Show(fex.Message + " Format Exception", Title);
            }
            catch (XmlException ex)
            {
                MessageBox.Show(ex.Message, Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Title);
            }
           
           FillSalespersonListBox();
        }

        //method to popualte the listbox with data
        private void FillSalespersonListBox()
        {
            lstSalespeople.Items.Clear();
            foreach (Salesperson s in salespeople)
            {
                lstSalespeople.Items.Add(s.GetDisplayText("\t"));
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //open the frmAdd form instance
            frmAdd newSalespersonForm = new frmAdd();
            //populate the object with data you get from calling the 
            //GetNewSalesperson method of the frmAdd class
           
            Salesperson s1 = newSalespersonForm.GetNewSalesperson();
          
            if (s1 != null) //make sure there is an object to display
            {
                salespeople.Add(s1); //added the new object to the list
                try
                {
                    SalespersonDB.SaveSalespeople(salespeople); //call save method
                }
                    //if saving doesn't work, catch the exception
                catch (FileNotFoundException)
                {
                    MessageBox.Show("File not found: " + path, Title);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message + " IOException", Title);
                }
                catch (FormatException fex)
                {
                    MessageBox.Show(fex.Message + " Format Exception", Title);
                }
                catch (XmlException ex)
                {
                    MessageBox.Show(ex.Message, Title);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Title);
                }
           
                FillSalespersonListBox();//populate listbox with the new list
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            //use listbox to delete 
            //loop through the listbox index
            int i = lstSalespeople.SelectedIndex;
            if(i!=-1) 
                //A value of negative one (-1) is returned if no item is selected
                //so making sure the index is in bounds
            {
                Salesperson salesperson = (Salesperson)salespeople[i];
                string msg = "Are you sure you want to delete " + salesperson.Name + " ?";
                DialogResult button = MessageBox.Show(msg, "Confirm Delete",
					MessageBoxButtons.YesNo);
                if(button ==DialogResult.Yes)
                {
                    salespeople.Remove(salesperson);

                    //try to save, if not, atch the exxception
                    try
                    {
                        SalespersonDB.SaveSalespeople(salespeople);
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("File not found: " + path, Title);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message + " IOException", Title);
                    }
                    catch (FormatException fex)
                    {
                        MessageBox.Show(fex.Message + " Format Exception", Title);
                    }
                    catch (XmlException ex)
                    {
                        MessageBox.Show(ex.Message, Title);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Title);
                    }
                    FillSalespersonListBox();
                }

            }
        }

    }
}
