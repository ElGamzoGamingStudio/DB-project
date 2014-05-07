using Npgsql;
using System.Xml.Linq;
using System.Data.Entity;
using PathToSuccess.Models;

namespace PathToSuccess.DAL
{
    public static class SqlRepository
    {
        public static Context DBContext { get; private set; }

        public static DbSet Users { get; private set; }
        public static DbSet Urgencies { get; private set; }
        public static DbSet Importancies { get; private set; }
        public static DbSet Intervals { get; private set; }
        public static DbSet Criterias { get; private set; }
        public static DbSet Trees { get; private set; }
        public static DbSet Tasks { get; private set; }
        public static DbSet Schedules { get; private set; }
        public static DbSet TimeRules { get; private set; }
        public static DbSet Steps { get; private set; }
        public static DbSet TimeBindings { get; private set; }

        public static void Save()
        {
            DBContext.SaveChanges();
        }
        

        public static void Initialize()
        {
            if (System.Windows.SystemParameters.IsSlowMachine) throw new System.Exception("VISOSI UEBOK COMP NORM KUPI KRIM NAW");
                    
            var doc = XDocument.Load("DbConnectionInfo.xml");
            string connectionString = "Server=" + doc.Root.Attribute("ip").Value + ";Port=" + doc.Root.Attribute("port").Value +
                                      ";User Id=" + doc.Root.Attribute("id").Value + ";Password=" + doc.Root.Attribute("pass").Value +
                                      ";Database=" + doc.Root.Attribute("dbname").Value + ";";
            var conn = new NpgsqlConnection(connectionString); //5432
            conn.Open();

            DBContext = new Context(conn);

            Users = DBContext.GetDbSet<User>();
            Urgencies = DBContext.GetDbSet<Urgency>();
            Importancies = DBContext.GetDbSet<Importance>();
            Criterias = DBContext.GetDbSet<Criteria>();
            Trees = DBContext.GetDbSet<Tree>();
            Schedules = DBContext.GetDbSet<Models.Schedule>();
            TimeRules = DBContext.GetDbSet<TimeRule>();
            Steps = DBContext.GetDbSet<Step>();
            Tasks = DBContext.GetDbSet<Task>();
            TimeBindings = DBContext.GetDbSet<TimeBinding>();
            Intervals = DBContext.GetDbSet<Interval>();

            //BL.Application.CurrentTree = (Tree)Trees.Find(1);
            //BL.DbTest.Seed();
        }
    }
}