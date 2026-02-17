using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
namespace NaatsWebApp.Models
{
    public class DBAccess
    {
        static string constr = "Data Source=DESKTOP-ICNAA62\\SQLEXPRESS;" +
                               "Initial Catalog=NaatDB;" +
                               "Integrated Security=True;" +
                               "TrustServerCertificate=True;";

        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = null;
        SqlDataReader sdr = null;
        public void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
        public void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public void IUD(string query)
        {
            OpenConnection();
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public SqlDataReader GetData(string query)
        {
            OpenConnection();
            cmd = new SqlCommand(query, con);
            sdr = cmd.ExecuteReader();
            return sdr;
        }

    }
}
