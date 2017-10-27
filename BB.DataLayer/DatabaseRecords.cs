using System;
using System.Data.SqlClient;
using System.Collections.Generic;


namespace BB.DataLayer
{
    public abstract class DatabaseRecords
    {
        public abstract void LoadRecords(Dictionary<String, object> WhereParams);
        public abstract void SaveRecords(SqlConnection cn = null);
        protected SqlConnection GetSQLConnection()
        {
            var conn = new Connection();

            SqlConnection cn = conn.GetConnection();
            cn.Open();

            return cn;
        }
    }
}
