using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020324.DataLayers.SQLServer
{
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";

        public _BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection OpenConection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
