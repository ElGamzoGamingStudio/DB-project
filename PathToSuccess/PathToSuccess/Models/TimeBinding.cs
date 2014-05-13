using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    [Table("time_binding", Schema = "public")]
    public class TimeBinding
    {
        protected bool Equals(TimeBinding other)
        {
            return Id == other.Id && StepId == other.StepId && Equals(Step, other.Step) && Time.Equals(other.Time) &&
                   Day == other.Day && Month == other.Month && Year == other.Year;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ StepId;
                hashCode = (hashCode*397) ^ (Step != null ? Step.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Time.GetHashCode();
                hashCode = (hashCode*397) ^ Day;
                hashCode = (hashCode*397) ^ Month;
                hashCode = (hashCode*397) ^ Year;
                return hashCode;
            }
        }

        [Key]
        [Column("tb_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("step_id")]
        public int StepId { get; set; }
        [ForeignKey("StepId")]
        public Step Step { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("day")]
        public int Day { get; set; }

        [Column("month")]
        public int Month { get; set; }

        [Column("year")]
        public int Year { get; set; }

        public TimeBinding()
        {}
        
        public static TimeBinding CreateTimeBinding(int id, int stepId, Step step, DateTime time, int day, int month, int year)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            var tb = new TimeBinding();

            tb.Id = id;
            tb.StepId = stepId;
            tb.Step = step;
            tb.Time = time;
            tb.Day = day;
            tb.Month = month;
            tb.Year = year;

            set.Add(tb);
            DAL.SqlRepository.Save();
            return tb;
        }
        public static void DeleteTimeBinding(TimeBinding timeBinding)
        {
            var set = DAL.SqlRepository.TimeBindings;
            var tb = set.Find(timeBinding.Id);
            if (tb != null)
            {
                set.Remove(timeBinding);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }
        public static List<TimeBinding> Select(Func<TimeBinding, bool> predicate)
        {
            return DAL.SqlRepository.TimeBindings
                .Cast<TimeBinding>()
                .Where(predicate)
                .ToList();
        }
        public static List<TimeBinding> GetAll()
        {
            return DAL.SqlRepository.TimeBindings
                .Cast<TimeBinding>()
                .ToList();
        }
        public static List<TimeBinding> GetTBofDay(int day, int month, int year,Tree tree)
        {
            var tasks = Task.SelectAllTreeTask(tree.MainTaskId).Where(x => x.ChildrenAreSteps());
            var steps = new List<Step>();
            foreach (var tsk in tasks)
            {
                steps.Add(Step.GetFirstUndoneStepByTaskID(tsk.Id));
            }
            var tbs = new List<TimeBinding>();
            foreach (var st in steps)
            {
                tbs.AddRange(TimeBinding.GetTBbyStepID(st.Id));
            }
            return tbs
                .Where(x => x.Year == year && x.Month == month && x.Day == day).ToList();
        }
        public static List<TimeBinding> GetTBbyStepID(int stepid)
        {
            return DAL.SqlRepository.TimeBindings
                .Cast<TimeBinding>()
                .Where(x => x.StepId == stepid)
                .ToList();           
        }
        public bool TimeCheck()
        {
            DateTime d = new DateTime(Year, Month, Day, Time.Hour, Time.Minute, Time.Second);
            return DateTime.Compare(DateTime.Now, d) > 0;
        }
        public int CompareTo(TimeBinding toCompare)
        {
            DateTime first = new DateTime(Year,Month,Day,Time.Hour,Time.Minute,0);
            DateTime second = new DateTime(toCompare.Year, toCompare.Month, toCompare.Day, toCompare.Time.Hour, toCompare.Time.Minute, 0);
            return first.CompareTo(second);
        }
        public DateTime GetNormalTime()
        {
            return new DateTime(Year,Month,Day,Time.Hour,Time.Minute,Time.Second);
        }

        public static List<TimeBinding> GetAllSortedByTime()
        {
            return DAL.SqlRepository.TimeBindings
                .Cast<TimeBinding>()
                .ToList()
                .OrderBy(x => x.GetNormalTime())
                .ToList();
               
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeBinding) obj);
        }
    }
}
