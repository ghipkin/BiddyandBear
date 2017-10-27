using System.Configuration;
using System.Data.SqlClient;

namespace BB.DataLayer
{
    public class Connection
    {
        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["BBcn"].ConnectionString);
        }
    }
}