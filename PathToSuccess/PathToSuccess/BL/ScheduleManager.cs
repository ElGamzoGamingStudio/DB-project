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
        {}

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
                while (!flag)
                {
                    if (dateCounter.DayOfWeek == avlIntervals[counter].dayOfWeek)
                    {
                        //check if there is avaliable time
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
