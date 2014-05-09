using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    public static class DbTest
    {
        //private static User[] users;
        //private static Urgency[] urgencies;
        //private static Importance[] importancies;
        //private static Interval[] intervals;
        //private static Criteria[] criterias;
        //private static Models.Schedule[] schedules;
        //private static TimeRule[] timeRules;
        //private static TimeBinding[] timeBindings;
        //private static Models.Task[] tasks;
        //private static Step[] steps;
        //private static Tree[] trees;

        //PLEASE DO NOT CALL THIS METHOD IT IS TOTALLY BAD.
        public static void Seed()
        {

            var user = User.CreateUser("xxxrassiyavperedxxx", "A.Pistoletov", new DateTime(1985, 3, 22), "secret_pass", DateTime.Now);
            //var user = (User)DAL.SqlRepository.Users.Find("xxxrassiyavperedxxx");
            var tree = Tree.CreateTree(user, user.Login, "new era of trees", "must work please");
           // var tree = Tree.FindTreesForUser(user).First();

            var importancies = Importance.GetViableImportanceLevels();
            var urgencies = Urgency.GetViableUrgencyLevels();
            var criterias = new Criteria[] {
                Criteria.CreateCriteria(0, 10, "wtuk"),
                Criteria.CreateCriteria(0, 1, "raz"),
                Criteria.CreateCriteria(0, 666, "fragov v cs")
            };
            //var criterias = DAL.SqlRepository.Criterias.Cast<Criteria>().ToList();

            var tasks = new Task[4];

            tasks[0] =  Task.CreateTask(DateTime.Now, DateTime.Now.AddDays(5), urgencies[0].UrgencyName, importancies[0].ImportanceName, importancies[0], urgencies[0], criterias[0].Id, criterias[0], "task 1", tree.MainTask, tree.MainTask.Id);
            tasks[1] = Task.CreateTask(DateTime.Now, DateTime.Now.AddDays(5), urgencies[1].UrgencyName, importancies[2].ImportanceName, importancies[2], urgencies[1], criterias[1].Id, criterias[1], "task 2", tree.MainTask, tree.MainTask.Id);
            tasks[2] = Task.CreateTask(DateTime.Now, DateTime.Now.AddDays(5), urgencies[2].UrgencyName, importancies[1].ImportanceName, importancies[1], urgencies[2], criterias[2].Id, criterias[2], "task 3", tasks[0], tasks[0].Id);
            tasks[3] = Task.CreateTask(DateTime.Now, DateTime.Now.AddDays(5), urgencies[0].UrgencyName, importancies[0].ImportanceName, importancies[0], urgencies[0], criterias[0].Id, criterias[0], "task 4", tasks[1], tasks[1].Id);

            var intervals = new Interval[] {
                Interval.CreateInterval(DateTime.Now, DateTime.Now.AddHours(3)),
                Interval.CreateInterval(DateTime.Now, DateTime.Now.AddHours(5)),
                Interval.CreateInterval(DateTime.Now, DateTime.Now.AddHours(7)),
                Interval.CreateInterval(DateTime.Now, DateTime.Now.AddHours(13)),
                Interval.CreateInterval(DateTime.Now, DateTime.Now.AddHours(22)),
                Interval.CreateInterval(DateTime.Now, DateTime.Now.AddHours(50))
            };

            var schedules = new Models.Schedule[] {
                Models.Schedule.CreateSchedule(intervals[0],intervals[1],intervals[2],intervals[3],intervals[4],intervals[5],intervals[0]),
                Models.Schedule.CreateSchedule(intervals[1],intervals[2],intervals[3],intervals[4],intervals[5],intervals[0],intervals[1]),
                Models.Schedule.CreateSchedule(intervals[2],intervals[3],intervals[4],intervals[5],intervals[0],intervals[1],intervals[0])
            };

            var timeRules = new TimeRule[] {
                TimeRule.CreateTimeRule(false, schedules[0].Id, schedules[0]),
                TimeRule.CreateTimeRule(true, schedules[1].Id, schedules[1]),
                TimeRule.CreateTimeRule(false, schedules[2].Id, schedules[2]),
                TimeRule.CreateTimeRule(true, schedules[0].Id, schedules[0]),
            };

            var steps = new Step[] {
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[0].UrgencyName, urgencies[0], importancies[0].ImportanceName, importancies[0], criterias[2].Id, criterias[2], timeRules[1].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 0),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[1].UrgencyName, urgencies[1], importancies[1].ImportanceName, importancies[1], criterias[1].Id, criterias[1], timeRules[2].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 1),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[2].UrgencyName, urgencies[2], importancies[2].ImportanceName, importancies[2], criterias[0].Id, criterias[0], timeRules[3].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 2),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[1].UrgencyName, urgencies[1], importancies[0].ImportanceName, importancies[0], criterias[1].Id, criterias[1], timeRules[0].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 3),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[2].UrgencyName, urgencies[2], importancies[2].ImportanceName, importancies[2], criterias[2].Id, criterias[2], timeRules[1].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 4),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[1].UrgencyName, urgencies[1], importancies[1].ImportanceName, importancies[1], criterias[0].Id, criterias[0], timeRules[2].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 5),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[2].UrgencyName, urgencies[2], importancies[0].ImportanceName, importancies[0], criterias[0].Id, criterias[0], timeRules[3].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 6),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[0].UrgencyName, urgencies[0], importancies[1].ImportanceName, importancies[1], criterias[0].Id, criterias[0], timeRules[0].Id, timeRules[0], "step 1", tasks[2], tasks[2].Id, 7),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[1].UrgencyName, urgencies[1], importancies[2].ImportanceName, importancies[2], criterias[2].Id, criterias[2], timeRules[1].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 0),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[0].UrgencyName, urgencies[0], importancies[0].ImportanceName, importancies[0], criterias[0].Id, criterias[0], timeRules[2].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 1),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[2].UrgencyName, urgencies[2], importancies[2].ImportanceName, importancies[2], criterias[1].Id, criterias[1], timeRules[3].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 2),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[0].UrgencyName, urgencies[0], importancies[1].ImportanceName, importancies[1], criterias[0].Id, criterias[0], timeRules[0].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 3),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[2].UrgencyName, urgencies[2], importancies[0].ImportanceName, importancies[0], criterias[0].Id, criterias[0], timeRules[1].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 4),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[1].UrgencyName, urgencies[1], importancies[1].ImportanceName, importancies[1], criterias[2].Id, criterias[2], timeRules[2].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 5),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[0].UrgencyName, urgencies[0], importancies[2].ImportanceName, importancies[2], criterias[0].Id, criterias[0], timeRules[3].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 6),
                Step.CreateStep(DateTime.Now, DateTime.Now.AddDays(30), urgencies[1].UrgencyName, urgencies[1], importancies[0].ImportanceName, importancies[0], criterias[0].Id, criterias[0], timeRules[0].Id, timeRules[0], "step 1", tasks[3], tasks[3].Id, 7)
            };

            
        }

        public static void Try()
        {
            var collection = DAL.SqlRepository.TimeBindings.Cast<TimeBinding>().ToList();
            foreach (var tb in collection)
                DAL.SqlRepository.TimeBindings.Remove(tb);
            DAL.SqlRepository.Save();

            ScheduleManager.Initialize();

            ScheduleManager.FillScheduleForPeriodic();

            ScheduleManager.CreateScheduleForNonTB();
        }
    }
}
