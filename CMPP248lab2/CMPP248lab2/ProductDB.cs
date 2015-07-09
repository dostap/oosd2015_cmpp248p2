using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMPP248lab2
{
    public static class ProductDB //database class with static methods
    {
        //get product by product code
        public static Product GetProduct(string productCode)
        {
            SqlConnection connection = MMABooksDB.GetConnection(); //connect to the database
            string selectStatement =
                "SELECT ProductCode, Description, UnitPrice, OnHandQuantity " +
                "FROM Products " +
                "WHERE ProductCode = @ProductCode"; // declare string carrying a query
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection); 
            //created new SqlCommand object that will execute the select statement we saved in the string
            selectCommand.Parameters.AddWithValue("@ProductCode", productCode); 
            //add value to the parameters

            try
            {
                connection.Open();
                
                SqlDataReader reader = 
                    selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                //CommandBeahviour - Provides a description of the results of the query and its effect on the database
                //SingleRow means the query expects just one row
                //While the SqlDataReader is being used, the associated SqlConnection is busy serving the SqlDataReader,
                //and no other operations can be performed on the SqlConnection other than closing it.
                //"A data-reader is the hose: it provides one-way/once-only access to data as it flies past you"
                //good for reading data in the most efficient manner possible

                if (reader.Read())
                //The default position of the SqlDataReader is before the first record 
                //Therefore, you must call Read to begin accessing any data
                //Read() method ADVANCES the SqlDataReader to the next record
                //method returns true as long as there is a row to read

                {
                    //now we process the row
                    Product product = new Product(); // create a new instance of Product class
                    product.ProductCode = reader["ProductCode"].ToString();
                    //read column name "ProductCode" and convert it to string, assign it to ProductCode
                    product.Description = reader["Description"].ToString();
                    product.UnitPrice = (decimal)reader["UnitPrice"]; //cast to decimal
                    product.OnHandQuantity = (int)reader["OnHandQuantity"]; //cast to integer
                   
                    return product; //return populated object to the calling emthod
                }
                else // now row = no product
                {
                    return null;
                }
            }
            catch (SqlException ex)//throws exception to the calling method if SqlException occurs
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }//end of GetProduct method


        //method to add a new product to the database
        public static void AddProduct(Product product)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string insertStatement =
                "INSERT INTO Products " +
                "(ProductCode, Description, UnitPrice, OnHandQuantity) " +
                "VALUES(@ProductCode, @Description, @UnitPrice, @OnHandQuantity)"; // SqL INSERT query saved in a string
            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            //create and add values to parameters of the SqlCommand object (insertCommand)
            insertCommand.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            insertCommand.Parameters.AddWithValue("@Description", product.Description);
            insertCommand.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            insertCommand.Parameters.AddWithValue("@OnHandQuantity", product.OnHandQuantity);
            
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery(); //Executes a Transact-SQL statement 
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }//end of add Product method


        //method to modify an existing product in the database
        public static bool ModProduct(Product oldProduct,
            Product newProduct)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string updateStatement =
                "UPDATE Products SET " +
                "ProductCode = @NewProductCode, " +
                "Description = @NewDescription, " +
                "UnitPrice = @NewUnitPrice, " +
                "OnHandQuantity = @NewOnHandQuantity " +
                "WHERE ProductCode = @OldProductCode " +
                "AND Description = @OldDescription " +
                "AND UnitPrice = @OldUnitPrice " +
                "AND OnHandQuantity = @OldOnHandQuantity";
            //new update query
            //where statemenet will ensure that product will only be modified
            //if it has not been changed by another user in the meantine
            SqlCommand updateCommand =
                new SqlCommand(updateStatement, connection);
            updateCommand.Parameters.AddWithValue(
                "@NewProductCode", newProduct.ProductCode);
            updateCommand.Parameters.AddWithValue(
                "@NewDescription", newProduct.Description);
            updateCommand.Parameters.AddWithValue(
                "@NewUnitPrice", newProduct.UnitPrice.ToString());
            updateCommand.Parameters.AddWithValue(
                "@NewOnHandQuantity", newProduct.OnHandQuantity);
            updateCommand.Parameters.AddWithValue(
                "@OldProductCode", oldProduct.ProductCode);
            updateCommand.Parameters.AddWithValue(
                "@OldDescription", oldProduct.Description);
            updateCommand.Parameters.AddWithValue(
                "@OldUnitPrice", oldProduct.UnitPrice.ToString());
            updateCommand.Parameters.AddWithValue(
                "@OldOnHandQuantity", oldProduct.OnHandQuantity);
           try
            {
                connection.Open();
                int count = updateCommand.ExecuteNonQuery();//returns number of rows affected to count
                if (count > 0) //if count is more than zero, means query was successful, return true to calling emthod
                    return true;
                else // if now rows were affected, return false to calling method
                    //this will go back to frmAddModifyProduct and display an error message that
                    //another user has modified row in the meantime
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool DeleteProduct(Product product)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string deleteStatement =
                "DELETE FROM Products " +
                "WHERE ProductCode = @ProductCode " +
                "AND Description = @Description " +
                "AND UnitPrice = @UnitPrice " +
                "AND OnHandQuantity = @OnHandQuantity ";
            SqlCommand deleteCommand =
                new SqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            deleteCommand.Parameters.AddWithValue("@Description", product.Description);
            deleteCommand.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            deleteCommand.Parameters.AddWithValue("@OnHandQuantity", product.OnHandQuantity);
            try
            {
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery(); //works the same way as modifying
                                                            //gives error when there is a concurrency issue
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
