using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace CMPP248Part2Lab3
{
    class SalespersonDB
    {
        private static string path = @"Salespeople.xml";  // this uses the project file

        //a method that will retrieve Salesperson data and populate the lsitbox

        public static List<Salesperson> GetSalesperson()
        {
            //declare a new list to hold the data
            List<Salesperson> salespeople = new List<Salesperson>();

            //use XmlReader - lets you run through the XML content one element at a time
            //create a new XmlReaderSettings object
            XmlReaderSettings settings = new XmlReaderSettings();
            
            XmlReader xmlRead = null;


            try
            {
                //check if file exists or not
                //create it if it doesnt exist
                XmlDocument xDoc = new XmlDocument();
                if (!File.Exists("Salespeople.xml"))
                    //if no file yet exists than return an empty salespeople list
                 {   MessageBox.Show("New file for Salespeople: \""+ path + "\", will be created!");
                    return salespeople;
                }

                //create a new XmlReader object
                //input our path and settings as arguments
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
                xmlRead = XmlReader.Create(path, settings);

                //read past everything to the Salesperson node
                if (xmlRead.ReadToDescendant("Salesperson"))
                {
                    do
                    {
                        //create a new instance of a salesperson
                        Salesperson salesperson = new Salesperson();

                        //get the salesperson attribute "Name" and assign it to the salesperson.Name property
                        salesperson.Name = xmlRead["Name"];

                        //ReadStartElement - Checks that the current node is an element and advances the reader to the next node.
                        xmlRead.ReadStartElement("Salesperson");

                        //read the element values an assign them to other salesperson properties
                        salesperson.AgencyID = xmlRead.ReadElementContentAsInt();
                        salesperson.Commission = xmlRead.ReadElementContentAsDouble();
                        salesperson.SalesAmount = xmlRead.ReadElementContentAsDecimal();

                        //add the resulting salesperson object to the list of salespeople
                        salespeople.Add(salesperson);
                    }
                    while (xmlRead.ReadToNextSibling("Salesperson"));
                    //keeps looping as long as the enxt element is a Salesperson one
                }

                // close the XmlReader object
                // xmlRead.Close();
            }

            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (FormatException fex)
            {
                throw fex;
            }
            catch (XmlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlRead != null) //prevents NullPointerException
                {
                    xmlRead.Close();
                }
            }

            //return the resulting list of salespeople
            return salespeople;

        }// end of GetSalesperson() method

        public static void SaveSalespeople(List<Salesperson> salespeople)
        {
            //use XmlWriter 
            //create a new XmlReaderSettings object
            XmlWriterSettings settings = new XmlWriterSettings();
            
            //since we are creating an xml doc, we need a tab = 4spaces for indentation
            XmlWriter xmlWrite = null; //declare new instance of XmlWriter

            try
            {
                //create the XmlWriter object
                settings.Indent = true;
                settings.IndentChars = ("    ");
                xmlWrite = XmlWriter.Create(path, settings);

                //write the XML declaration
                xmlWrite.WriteStartDocument();
                //write the specified tag
                xmlWrite.WriteStartElement("Salespeople");

                //loop through th elsit of all salespeople
                foreach (Salesperson salesperson in salespeople)
                {
                    xmlWrite.WriteStartElement("Salesperson");
                    //add attribute to element
                    xmlWrite.WriteAttributeString("Name", salesperson.Name);
                    //add child elements
                    xmlWrite.WriteElementString("AgencyID", Convert.ToString(salesperson.AgencyID));
                    xmlWrite.WriteElementString("Commission", Convert.ToString(salesperson.Commission));
                    xmlWrite.WriteElementString("SalesAmount", Convert.ToString(salesperson.SalesAmount));
                    //write out the closing tag to the element
                    xmlWrite.WriteEndElement();
                }
                //write the tag to close root element <Salesperson>
                xmlWrite.WriteEndElement();

            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            catch (FormatException fex)
            {
                throw fex;
            }
            catch (XmlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlWrite != null) //prevents NullPointerException
                {
                    xmlWrite.Close();
                }
            }

        }//end of class SalespersonDB 

    }
}
