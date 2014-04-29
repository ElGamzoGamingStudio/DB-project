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

namespace PathToSuccess.DAL
{
    public class SqlRepository : IRepository
    {
        public SqlRepository()
        {
            var conn = new NpgsqlConnection(
                    "Server=127.0.0.1;Port=5432;User Id=postgres;Password=YourPassword;Database=PathToSuccess;");
            conn.Open();
            var cont = new Context(conn);
            var query = cont.Database.SqlQuery<User>("select * from public.users").ToList();
        }
        public int GetNextID()
        {
            return 0;
        }
        public List<Unit> GetImportanceUnits()
        {
            return null;
        }
        public List<Unit> GetUrgencyUnits()
        {
            return null;
        }
        
    }
}
