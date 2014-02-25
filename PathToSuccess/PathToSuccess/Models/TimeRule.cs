using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public abstract class TimeRule//правило, по которому задача или шаг вносится в дерево (одноразовое, ежедневное, еженедельное и т.д.)
    {
        public TimeRule()
        {
            //...//
        }

        /// <summary>
        /// Generates tasks using some hidden rule descripted by the class name of a subclass.
        /// </summary>
        /// <param name="TimePeriod">Hours</param>
        /// <returns></returns>
        public virtual List<Task> GenerateTasks(int TimePeriod)
        {
            //to override;
            return null;
        }
    }

    //example:
    public static class EverydayTimeRule : TimeRule
    {
        public override List<Task> GenerateTasks(int TimePeriod)
        {
            int count = TimePeriod / 24;
            var list = new List<Task>();

            for (int i = 0; i < count; i++)
            {
                //create tasks
            }

            return list;
        }
    }

    public class WeeklyTimeRule : TimeRule
    {
        public List<Day> AcceptableDays;

        public WeeklyTimeRule(params Day[] days)
        {
            AcceptableDays = days.ToList<Day>();
        }

        public override List<Task> GenerateTasks(int TimePeriod)
        {
            //...//
        }
    }

    timerules

        id
        idtask
        rulename
        params

    days:{Monday,Tuesday};
}
