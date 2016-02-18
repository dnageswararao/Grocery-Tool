using System.Configuration;
using System.Data.SqlClient;

namespace DataLayer
{
    public class DbUtility
    {
        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["GroceriesContext"].ConnectionString
            };
            return conn;
        }
    }
}
