using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.DAL;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using Npgsql;
using System.Xml.Linq;


namespace PathToSuccess.BL
{
    public static class RawSqlPusher
    {

        public static object PushQuery(string queryText)
        {
            
            return null;
        }

        public static void Initialize()
        {
            //var doc = XDocument.Load("RawSqlConnectionInfo.xml");
            //string connectionString = "Server=" + doc.Root.Attribute("ip").Value + ";Port=" + doc.Root.Attribute("port").Value +
            //                          ";User Id=" + doc.Root.Attribute("id").Value + ";Password=" + doc.Root.Attribute("pass").Value +
            //                          ";Database=" + doc.Root.Attribute("dbname").Value + ";";

            //var conn = new NpgsqlConnection(connectionString);
    
            //conn.Open();
            
            //var conn = new NpgsqlConnection(connectionString);
            //conn.Open();
            //ctx = new Context(conn);
        }
    }
}
