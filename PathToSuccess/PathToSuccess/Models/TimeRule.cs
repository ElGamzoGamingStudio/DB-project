using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public abstract class TimeRule//правило, по которому задача или шаг вносится в дерево (одноразовое, ежедневное, еженедельное и т.д.)
    {
        public readonly string ID;
        public TimeRule()
        {
            ID = Guid.NewGuid().ToString();
            //...//
        }

        /// <summary>
        /// Generates tasks using some hidden rule descripted by the class name of a subclass.
        /// </summary>
        /// <param name="TimePeriod">Depends on particular rule. May be days, weeks, month etc.</param>
        /// <returns></returns>
        public virtual List<Task> GenerateTasks(int TimePeriod, Task TaskToExtend)
        {
            //to override;
            return null;
        }
    }

    //example:
    public static class EverydayTimeRule : TimeRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimePeriod">Days</param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int TimePeriod, Task TaskToExtend)
        {
            var result = new List<Task>();
            var today = DateTime.Now;

            for (int i = 0; i < TimePeriod; i++)
            {
                var day = today.AddDays(i);
                //create tasks
            }

            return result;
        }
    }

    public static class DaysOfWeekTimeRule : TimeRule
    {
        public List<DayOfWeek> AcceptableDays;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimePeriod">Days</param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int TimePeriod, Task TaskToExtend, params DayOfWeek[] days)
        {
            var result = new List<Task>();
            var today = DateTime.Now;
            for (int i = 0; i < TimePeriod; i++)
            {
                if (AcceptableDays.Contains(
                    today.AddDays(i)
                    .DayOfWeek))
                    //... create task ...//
            }
            return result;
        }
    }

    public static class WeeklyTimeRule : TimeRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimePeriod">Weeks</param>
        /// <param name="TaskToExtend"></param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int TimePeriod, Task TaskToExtend)
        {
            var result = new List<Task>();
            var today = DateTime.Now;
            for (int i = 0; i < TimePeriod; i++)
            {
                var day = today.AddDays(i * 7);
                //... create task ...//
            }
            return result;
        }
    }

    public static class MonthlyTimeRule : TimeRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimePeriod">Months</param>
        /// <param name="TaskToExtend"></param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int TimePeriod, Task TaskToExtend)
        {
            var today = DateTime.Now.Date;
            var result = new List<Task>();
            for (int i = 0; i < TimePeriod; i++)
            {
                var nextOne = today.AddMonths(i);
                if (nextOne.Day == today.Day) // ... create task ... //
            }
            return result;
        }
    }


    /*
    timerules

        id
        idtask
        rulename
        params

    days:{Monday,Tuesday};
     */
}
