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
            var withoutTbCopy = new List<Step>(withoutTB);
            foreach (var st in withoutTbCopy)
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
            var withoutTbCopy = new List<Step>(withoutTB);
            foreach (var step in withoutTbCopy)
            {
                var avlIntervals = PathToSuccess.Models.Schedule.GetNotEmptyIntervals(step.TimeRule.ScheduleId);
                bool flag = false;
                int counter = 0;
                DateTime dateCounter = DateTime.Today;
                dateCounter = dateCounter.AddDays(1);
                if (!step.TimeRule.IsPeriodic)
                {
                    while (!flag)
                    {
                        if (dateCounter.DayOfWeek == avlIntervals[counter].dayOfWeek)
                        {
                            var currInterv = Interval.GetIntervalByID(avlIntervals[counter].intervalID);
                            var tbs = TimeBinding.GetTBofDay(dateCounter.Day, dateCounter.Month, dateCounter.Year,Application.CurrentTree);
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
                                    var stTB =  TimeBinding.CreateTimeBinding(0, step.Id, step, timeCounter, timeCounter.Day, timeCounter.Month, timeCounter.Year);
                                    withoutTB.Remove(step);
                                    withTB.Add(step);
                                    flag = true;
                                }
                            }
                            else
                            {
                                if (tbs.Count == 0)
                                {
                                    flag = true;
                                    var newTB = TimeBinding.CreateTimeBinding(0, step.Id, step, currInterv.BeginTime,
                                        dateCounter.Day, dateCounter.Month, dateCounter.Year);
                                    withoutTB.Remove(step);
                                    withTB.Add(step);
                                    continue;
                                }
                                DateTime guf = new DateTime(tbs[0].Year, tbs[0].Month, tbs[0].Day, currInterv.BeginTime.Hour, currInterv.BeginTime.Minute, currInterv.BeginTime.Second);
                                TimeSpan delta = tbs[0].GetNormalTime() - guf;
                                if (delta.Hours > 1)
                                {
                                    flag = true;
                                    var newTB = TimeBinding.CreateTimeBinding(0, step.Id, step, currInterv.BeginTime,
                                        dateCounter.Day, dateCounter.Month, dateCounter.Year);
                                    withoutTB.Remove(step);
                                    withTB.Add(step);
                                    continue;
                                }
                                for (int i = 1; i < tbs.Count - 1 && !flag; i++)
                                {
                                    delta = tbs[i].GetNormalTime() - tbs[i - 1].GetNormalTime();
                                    if (Math.Abs(delta.Hours) > 2)
                                    {
                                        timeCounter = new DateTime(tbs[i].Year, tbs[i].Month, tbs[i].Day, tbs[i].Time.Hour - 1, tbs[i].Time.Minute, 0);
                                        if (timeCounter > currInterv.EndTime)
                                            break;
                                        flag = true;
                                        var newTB = TimeBinding.CreateTimeBinding(0, step.Id, step, timeCounter, timeCounter.Day, timeCounter.Month, timeCounter.Year);
                                        withoutTB.Remove(step);
                                        withTB.Add(step);
                                    }
                                }
                                if (!flag)
                                {
                                    guf = new DateTime(tbs[tbs.Count - 1].Year, tbs[tbs.Count - 1].Month, tbs[tbs.Count - 1].Day, currInterv.EndTime.Hour, currInterv.EndTime.Minute, currInterv.EndTime.Second);
                                    delta = guf - tbs[tbs.Count - 1].GetNormalTime();
                                    if (delta.Hours > 2)
                                    {
                                        flag = true;
                                        timeCounter = new DateTime(tbs[tbs.Count - 1].Year, tbs[tbs.Count - 1].Month, tbs[tbs.Count - 1].Day,
                                            tbs[tbs.Count - 1].Time.Hour - 1, tbs[tbs.Count - 1].Time.Minute, 0);
                                        var newTB = TimeBinding.CreateTimeBinding(0, step.Id, step, timeCounter, timeCounter.Day, timeCounter.Month, timeCounter.Year);
                                        withoutTB.Remove(step);
                                        withTB.Add(step);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (dateCounter.DayOfWeek < avlIntervals[counter].dayOfWeek || dateCounter.DayOfWeek > avlIntervals[avlIntervals.Count-1].dayOfWeek)
                                dateCounter = dateCounter.AddDays(1);
                            else
                            {
                                counter++;
                                counter %= avlIntervals.Count();
                            }
                            if (step.EndDate <= dateCounter)
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
        }

        public static void FillScheduleForPeriodic()
        {
            var periodicSteps = new List<Step>();
            foreach (var st in withoutTB)
                if (st.TimeRule.IsPeriodic)
                    periodicSteps.Add(st);
            foreach (var pstep in periodicSteps)
            {
                var intervals = PathToSuccess.Models.Schedule.GetNotEmptyIntervals(pstep.TimeRule.ScheduleId);
                var intervalDates = new List<DateTime>();
                foreach (var intervin in intervals)
                {
                    var dateCounter = DateTime.Today;
                    dateCounter = dateCounter.AddDays(1);
                    var interv = Interval.GetIntervalByID(intervin.intervalID);
                    while (dateCounter.DayOfWeek != intervin.dayOfWeek)
                        dateCounter = dateCounter.AddDays(1);
                    intervalDates.Add(new DateTime(dateCounter.Year, dateCounter.Month, dateCounter.Day,
                        interv.BeginTime.Hour, interv.BeginTime.Minute, 0));
                }
                bool isFine = true;
                for (int i = 0; i < intervals.Count; i++)
                {
                    var tbs = TimeBinding.GetTBofDay(intervalDates[i].Day, intervalDates[i].Month, intervalDates[i].Year,BL.Application.CurrentTree);
                    if (tbs.Count != 0)
                    {
                        foreach (var tb in tbs)
                        {
                            TimeSpan delta = tb.GetNormalTime() - Interval.GetIntervalByID(intervals[i].intervalID).BeginTime;
                            if (Math.Abs(delta.Hours) < 1)
                                isFine = false;
                        }
                    }
                }
                if (isFine)
                {
                    foreach (var date in intervalDates)
                    {
                        var d = date;
                        while (d < pstep.EndDate)
                        {
                            var tb = TimeBinding.CreateTimeBinding(0, pstep.Id, pstep, d, d.Day, d.Month, d.Year);
                            d = d.AddDays(7);
                        }
                    }
                    withoutTB.Remove(pstep);
                    withTB.Add(pstep);
                }
            }
        }

        public static List<Step> Overdue()
        {
            var overdue = new List<Step>();
            foreach (var step in withTB)
            {
                if (!step.TimeRule.IsPeriodic)
                    if (TimeBinding.GetTBbyStepID(step.Id)[0].Time < DateTime.Now.AddDays(1))
                        overdue.Add(step);
            }
            return overdue;
        }
        public static void Exterminatus()
        {
            foreach (var st in Overdue())
            {
                TimeBinding.DeleteTimeBinding(TimeBinding.GetTBbyStepID(st.Id)[0]);
            }
        }
        public static void CreateSchedule()
        {
            Exterminatus();
            FillScheduleForPeriodic();
            CreateScheduleForNonTB();
        }
 
    }
}
