using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    class ScheduleManager//Создает расписание и преобразует.
    {
        public static List<Step> withoutTB = new List<Step>();
        public static List<Step> withTB = new List<Step>();
        public ScheduleManager()
        { }

        public static void Initialize()
        {
            List<PathToSuccess.Models.Task> tsk = PathToSuccess.Models.Task.GetLowestTasks();
            foreach (var ts in tsk)
            {
                Step s = Step.GetFirstUndoneStepByTaskID(ts.Id);
                if (s != null)
                    withoutTB.Add(s);
            }
            if (withoutTB.Count == 0)
                return;
            foreach (var st in withoutTB)
            {
                List<TimeBinding> tb = TimeBinding.GetTBbyStepID(st.Id);
                if (tb.Count != 0)
                {
                    withTB.Add(st);
                    withoutTB.Remove(st);
                }
            }
        }
        public static void CreateScheduleForNonTB()
        {
            foreach (var step in withoutTB)
            {
                var avlIntervals = PathToSuccess.Models.Schedule.GetNotEmptyIntervals();
                bool flag = false;
                int counter = 0;
                DateTime dateCounter = DateTime.Today;
                dateCounter.AddDays(1);
                if (!step.TimeRule.IsPeriodic)
                {
                    while (!flag)
                    {
                        if (dateCounter.DayOfWeek == avlIntervals[counter].dayOfWeek)
                        {
                            var currInterv = Interval.GetIntervalByID(avlIntervals[counter].intervalID);
                            var tbs = TimeBinding.GetTBofDay(dateCounter.Day, dateCounter.Month, dateCounter.Year);
                            tbs.Sort();
                            DateTime timeCounter = new DateTime(dateCounter.Year, dateCounter.Month, dateCounter.Day,
                                currInterv.BeginTime.Hour, currInterv.BeginTime.Minute, currInterv.BeginTime.Second);
                            if (step.TimeRule.IsUserApproved)
                            {
                                bool isFine = true;
                                foreach (var tb in tbs)
                                {
                                    TimeSpan ts = tb.GetNormalTime() - timeCounter;
                                    if (Math.Abs(ts.Hours) < 1)
                                        isFine = false;
                                }
                                if (isFine)
                                {
                                    var stTB = new TimeBinding(1225236, step.Id, step, timeCounter, timeCounter.Day, timeCounter.Month, timeCounter.Year);
                                    TimeBinding.CreateTimeBinding(stTB);
                                }
                            }


                        }
                        else
                        {
                            if (dateCounter.DayOfWeek < avlIntervals[counter].dayOfWeek)
                                dateCounter.AddDays(1);
                            else
                            {
                                counter++;
                                counter %= avlIntervals.Count();
                            }
                        }
                    }
                }


                while (!flag)
                {
                    if (dateCounter.DayOfWeek == avlIntervals[counter].dayOfWeek)
                    {
                    }
                    else
                    {
                        if (dateCounter.DayOfWeek < avlIntervals[counter].dayOfWeek)
                            dateCounter.AddDays(1);
                        else
                        {
                            counter++;
                            counter %= avlIntervals.Count();
                        }
                    }
                }
            }
        }
    }
}
