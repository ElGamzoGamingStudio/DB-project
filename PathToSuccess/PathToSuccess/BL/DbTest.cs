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
        private static TimeBinding[] timeBindings;
        private static Models.Task[] tasks;
        private static Step[] steps;
        private static Tree[] trees;

        public static void Seed()
        {
            //addUsers();
            //addImportances();
            //addUrgencies();
            //addIntervals();
            //addCriterias();
            

            //add all the stuff above

            users = DAL.SqlRepository.DBContext.GetDbSet<User>().Cast<User>().ToList<User>().ToArray();
            
            urgencies = DAL.SqlRepository.DBContext.GetDbSet<Urgency>().Cast<Urgency>().ToList<Urgency>().ToArray();
            

            importancies = DAL.SqlRepository.DBContext.GetDbSet<Importance>().Cast<Importance>().ToList<Importance>().ToArray();

            //Interval.Seed();

            intervals = DAL.SqlRepository.DBContext.GetDbSet<Interval>().Cast<Interval>().ToList<Interval>().ToArray();

            importancies = DAL.SqlRepository.DBContext.GetDbSet<Importance>().Cast<Importance>().ToList<Importance>().ToArray();
            
            intervals = DAL.SqlRepository.DBContext.GetDbSet<Interval>().Cast<Interval>().ToList<Interval>().ToArray();


            criterias = DAL.SqlRepository.DBContext.GetDbSet<Criteria>().Cast<Criteria>().ToList<Criteria>().ToArray();


            //addTreeAndTask();

            trees = DAL.SqlRepository.DBContext.GetDbSet<Tree>().Cast<Tree>().ToList<Tree>().ToArray();

            tasks = DAL.SqlRepository.DBContext.GetDbSet<Models.Task>().Cast<Models.Task>().ToList<Models.Task>().ToArray();

            //addSchedule();

            schedules = DAL.SqlRepository.DBContext.GetDbSet<Models.Schedule>().Cast<Models.Schedule>().ToList<Models.Schedule>().ToArray();

            addTimeRules();

            timeRules = DAL.SqlRepository.DBContext.GetDbSet<TimeRule>().Cast<TimeRule>().ToList<TimeRule>().ToArray();

            //addSteps();

            steps = DAL.SqlRepository.DBContext.GetDbSet<Step>().Cast<Step>().ToList<Step>().ToArray();

            //addTimebindings();

            timeBindings = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>().Cast<TimeBinding>().ToList<TimeBinding>().ToArray();
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

        private static void addIntervals()
        {
            intervals = new Interval[] {
                new Interval(DateTime.Now, DateTime.Now),
                new Interval(DateTime.Now, DateTime.Now),
                new Interval(DateTime.Now, DateTime.MinValue)
            };
            var set = DAL.SqlRepository.DBContext.GetDbSet<Interval>();
            foreach (Interval i in intervals)
            {
                set.Add(i);
            }
            DAL.SqlRepository.DBContext.SaveChanges();
           
        }

        private static void addCriterias()
        {
            criterias = new Criteria[] {
                new Criteria(0, 1, "raz"),
                new Criteria(5, 10, "povtoreniy"),
                new Criteria(10, 10, "wtuk")
            };
            var set = DAL.SqlRepository.DBContext.GetDbSet<Criteria>();
            foreach (Criteria c in criterias)
            {
                set.Add(c);
            }
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        private static void addTreeAndTask()
        {
            trees = new Tree[] {
                new Tree(users[0],users[0].Login, null,0, "testTree", "descriptionepta")
            };
            tasks = new Models.Task[] {
                new Models.Task(DateTime.Now, DateTime.Now, urgencies[0].UrgencyName, importancies[0].ImportanceName, importancies[0], urgencies[0], criterias[0].Id, criterias[0], "desc", null, -1)
            };
            trees[0].MainTask = tasks[0];
            trees[0].MainTaskId = tasks[0].Id;

            var treeset = DAL.SqlRepository.DBContext.GetDbSet<Tree>();
            var taskset = DAL.SqlRepository.DBContext.GetDbSet<Models.Task>();

            treeset.Add(trees[0]);
            taskset.Add(tasks[0]);

            DAL.SqlRepository.DBContext.SaveChanges();

        }

        private static void addSchedule()
        {
            schedules = new Models.Schedule[] {
                new Models.Schedule(intervals[0], intervals[0], intervals[1], intervals[2], null, null, null)
            };

            var set = DAL.SqlRepository.DBContext.GetDbSet<Models.Schedule>();

            set.Add(schedules[0]);
            
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        private static void addTimeRules()
        {
            timeRules = new TimeRule[] {
                new TimeRule(0, true, schedules[0].Id, schedules[0]),
                new TimeRule(0, false, schedules[0].Id, schedules[0])
            };

            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeRule>();

            set.Add(timeRules[0]);
            set.Add(timeRules[1]);

            DAL.SqlRepository.DBContext.SaveChanges();

        }

        private static void addSteps()
        {
            steps = new Step[] {
                new Step(DateTime.Now, DateTime.Now, urgencies[0].UrgencyName, urgencies[0], importancies[0].ImportanceName, importancies[0], criterias[0].Id, criterias[0], timeRules[0].Id, timeRules[0], "first step",
                    trees[0].MainTask, trees[0].MainTask.Id, 0)
            };
            var set = DAL.SqlRepository.DBContext.GetDbSet<Step>();
            set.Add(steps[0]);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        private static void addTimebindings()
        {
            timeBindings = new TimeBinding[] {
                new TimeBinding(0, steps[0].Id, steps[0], DateTime.Now, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year)
            };

            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            set.Add(timeBindings[0]);
            DAL.SqlRepository.DBContext.SaveChanges();
        }
    }
}
