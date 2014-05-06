using Npgsql;
using System.Xml.Linq;

namespace PathToSuccess.DAL
{
    public static class SqlRepository
    {
        public static Context DBContext { get; private set; }

        public static void Initialize()
        {
            var doc = XDocument.Load("DbConnectionInfo.xml");
            string connectionString = "Server=" + doc.Root.Attribute("ip").Value + ";Port=" + doc.Root.Attribute("port").Value +
                                      ";User Id=" + doc.Root.Attribute("id").Value + ";Password=" + doc.Root.Attribute("pass").Value +
                                      ";Database=" + doc.Root.Attribute("dbname").Value + ";";
            var conn = new NpgsqlConnection(connectionString); //5432
            conn.Open();

            DBContext = new Context(conn);

            //BL.DbTest.Seed();
        }
    }
}