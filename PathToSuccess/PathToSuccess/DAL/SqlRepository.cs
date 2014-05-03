using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

using Npgsql;
using NpgsqlTypes;

using System.Data.Entity;

namespace PathToSuccess.DAL
{
    public static class SqlRepository
    {
        public static Context DBContext { get; private set; }

        public static void Initialize()
        {
            var conn = new NpgsqlConnection(
                    "Server=127.0.0.1;Port=5432;User Id=postgres;Password=1;Database=PathToSuccess;"); //5432
            conn.Open();

            DBContext = new Context(conn);
        }
    }
}
