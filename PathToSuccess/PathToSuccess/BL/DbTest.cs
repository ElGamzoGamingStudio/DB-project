using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    public static class DbTest
    {
        private static User[] users;
        private static Urgency[] urgencies;
        private static Importance[] importancies;
        private static Interval[] intervals;
        private static Criteria[] criterias;
        private static Models.Schedule[] schedules;
        private static TimeRule[] timeRules;
        private static TimeBinding[] timeBindingds;
        private static Models.Task[] tasks;
        private static Step[] steps;
        private static Tree[] trees;

        public static void Seed()
        {
            addUsers();
            addImportances();
            addUrgencies();

            //add all the stuff above

            var users = DAL.SqlRepository.DBContext.GetDbSet<User>().Cast<User>().ToList<User>();
            
            var ugencies = DAL.SqlRepository.DBContext.GetDbSet<Urgency>().Cast<Urgency>().ToList<Urgency>();
            
            var importancies = DAL.SqlRepository.DBContext.GetDbSet<Importance>().Cast<Importance>().ToList<Importance>();
            
            //var intervals = DAL.SqlRepository.DBContext.GetDbSet<Interval>().Cast<Interval>().ToList<Interval>();
            
            //var criterias = DAL.SqlRepository.DBContext.GetDbSet<Criteria>().Cast<Criteria>().ToList<Criteria>();
            
            //var schedules = DAL.SqlRepository.DBContext.GetDbSet<Models.Schedule>().Cast<Models.Schedule>().ToList<Models.Schedule>();
            
            //var timerules = DAL.SqlRepository.DBContext.GetDbSet<TimeRule>().Cast<TimeRule>().ToList<TimeRule>();
            
            //var timebindings = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>().Cast<TimeBinding>().ToList<TimeBinding>();
            
            //var tasks = DAL.SqlRepository.DBContext.GetDbSet<Models.Task>().Cast<Models.Task>().ToList<Models.Task>();
            
            //var steps = DAL.SqlRepository.DBContext.GetDbSet<Step>().Cast<Step>().ToList<Step>();
            
            //var trees = DAL.SqlRepository.DBContext.GetDbSet<Tree>().Cast<Tree>().ToList<Tree>();
        }

        private static void addUsers()
        {
            users = new User[] {
                new User("qwerty", "vasya", DateTime.Now, "1234", DateTime.Now),
                new User("asdf", "petya", DateTime.Now, "qwer", DateTime.Now),
                new User("testlogin", "kolya", DateTime.Now, "xxxdemonxxx", DateTime.Now)
            };
            var usersSet = DAL.SqlRepository.DBContext.GetDbSet<User>();
            foreach (User user in users)
            {
                usersSet.Add(user);
            }
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        private static void addUrgencies()
        {
            urgencies = new Urgency[] {
                new Urgency("low", 0),
                new Urgency("medium", 5),
                new Urgency("high", 10)
            };
            var set = DAL.SqlRepository.DBContext.GetDbSet<Urgency>();
            foreach(Urgency u in urgencies)
            {
                set.Add(u);
            }
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        private static void addImportances()
        {
            importancies = new Importance[] {
                new Importance("not important", 0),
                new Importance("maybe important", 10),
                new Importance("very important", 100)
            };
            var set = DAL.SqlRepository.DBContext.GetDbSet<Importance>();
            foreach(Importance i in importancies)
            {
                set.Add(i);
            }
            DAL.SqlRepository.DBContext.SaveChanges();
        }


    }
}
