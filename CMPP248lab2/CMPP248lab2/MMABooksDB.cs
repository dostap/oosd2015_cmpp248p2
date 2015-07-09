using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMPP248lab2
{
    class MMABooksDB
    {
        //set up connection to the database
        public static SqlConnection GetConnection()
        {
        string connectionString=
        "Data Source=(LocalDB)\\v11.0;AttachDbFilename=|DataDirectory|\\MMABooks.mdf;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        return connection;
        }
    }
}


