using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.DAL;
using Npgsql;

namespace PathToSuccess.BL
{
    public static class RawSqlPusher
    {
        private static Context ctx;
        private static string connectionString;

        public static object PushQuery(string queryText)
        {
            //ctx.Database.SqlQuery()
            return null;
        }

        public static void Initialize()
        {
            connectionString = "Server=127.0.0.1;Port=3306;User Id=select_only;Database=PathToSuccess;Password=ratdog";
            var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            ctx = new Context(conn);
        }
    }
}
