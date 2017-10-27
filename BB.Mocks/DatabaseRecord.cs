using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BB.Mocks
{
    public abstract class DatabaseRecord
    {
        public bool Saved { get; set; }

        protected String InsertSQL { get; set; }

        protected SqlConnection GetSQLConnection()
        {
            var conn = new Connection();
            
            SqlConnection cn  = conn.GetConnection();
            cn.Open();

            return cn;
        }

        public abstract string GetInsertSQL();

        public abstract void Save(SqlConnection cn = null);

        public abstract void Load(Dictionary<string, object> parms);
    }
}
