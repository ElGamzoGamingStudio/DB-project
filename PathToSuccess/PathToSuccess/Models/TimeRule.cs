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
        /// <param name="timePeriod">Depends on particular rule. May be days, weeks, month etc.</param>
        /// <param name="taskToExtend"></param>
        /// <returns></returns>
        public virtual List<Task> GenerateTasks(int timePeriod, Task taskToExtend)
        {
            //to override;
            return null;
        }
    }

    //example:
    public class EverydayTimeRule : TimeRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timePeriod">Days</param>
        /// <param name="taskToExtend"></param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int timePeriod, Task taskToExtend)
        {
            var result = new List<Task>();
            var today = DateTime.Now;

            for (int i = 0; i < timePeriod; i++)
            {
                var day = today.AddDays(i);
                //create tasks
            }

            return result;
        }
    }

    public class DaysOfWeekTimeRule : TimeRule
    {
        public List<DayOfWeek> AcceptableDays;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timePeriod">Days</param>
        /// <param name="taskToExtend"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public List<Task> GenerateTasks(int timePeriod, Task taskToExtend, params DayOfWeek[] days)
        {
            var result = new List<Task>();
            var today = DateTime.Now;
            for (int i = 0; i < timePeriod; i++)
            {
                if (AcceptableDays.Contains(
                    today.AddDays(i)
                    .DayOfWeek)){}
                    //... create task ...//
            }
            return result;
        }
    }

    public class WeeklyTimeRule : TimeRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timePeriod">Weeks</param>
        /// <param name="taskToExtend"></param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int timePeriod, Task taskToExtend)
        {
            var result = new List<Task>();
            var today = DateTime.Now;
            for (int i = 0; i < timePeriod; i++)
            {
                var day = today.AddDays(i * 7);
                //... create task ...//
            }
            return result;
        }
    }

    public  class MonthlyTimeRule : TimeRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timePeriod">Months</param>
        /// <param name="taskToExtend"></param>
        /// <returns></returns>
        public override List<Task> GenerateTasks(int timePeriod, Task taskToExtend)
        {
            var today = DateTime.Now.Date;
            var result = new List<Task>();
            for (int i = 0; i < timePeriod; i++)
            {
                var nextOne = today.AddMonths(i);
                if (nextOne.Day == today.Day)
                {
                    
                }// ... create task ... //
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
