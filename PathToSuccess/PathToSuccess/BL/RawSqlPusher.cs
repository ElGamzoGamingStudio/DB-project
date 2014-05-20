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
        public static Context Ctx;
        public static NpgsqlConnection Conn;
        public static string PushQuery(string queryText)
        {
            var sb = new StringBuilder();

            var command = new NpgsqlCommand(queryText, Conn);

            try
            {
                NpgsqlDataReader dr = command.ExecuteReader();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    sb.AppendFormat("{0} \t\t", dr.GetName(i));
                }
                sb.Append("\n\n");
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        sb.AppendFormat("{0} \t\t  ", dr[i]);
                    }
                    sb.Append("\n");
                }
            }
            catch
            {
                return "error";
            }
            return sb.ToString();
        }

        public static void Initialize()
        {
            var doc = XDocument.Load("RawSqlConnectionInfo.xml");
            string connectionString = "Server=" + doc.Root.Attribute("ip").Value + ";Port=" + doc.Root.Attribute("port").Value +
                                      ";User Id=" + doc.Root.Attribute("id").Value + ";Password=" + doc.Root.Attribute("pass").Value +
                                      ";Database=" + doc.Root.Attribute("dbname").Value + ";";

            Conn = new NpgsqlConnection(connectionString);

            Conn.Open();
            Ctx = new PathToSuccess.DAL.Context(Conn);
        }
    }
}
