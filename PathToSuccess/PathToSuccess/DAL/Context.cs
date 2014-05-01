using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data.OleDb;

namespace PathToSuccess.DAL
{
    public class Context : DbContext
    {
        public Context(System.Data.Common.DbConnection connection)
            :base(connection,true)
        {
            
        }

        
    }
}
