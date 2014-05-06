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
        public TimeBinding(int id,int stepId,Step step,DateTime time, int day, int month, int year)
        {
            Id = id;
            StepId = stepId;
            Step = step;
            Time = time;
            Day = day;
            Month = month;
            Year = year;
        }
        public static void CreateTimeBinding(TimeBinding timeBinding)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();

            set.Add(timeBinding);
            DAL.SqlRepository.DBContext.SaveChanges();
        }
        public static void DeleteTimeBinding(TimeBinding timeBinding)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            var tb = set.Find(timeBinding.Id);
            if (tb != null)
            {
                set.Remove(timeBinding);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }
        public static List<TimeBinding> Select(Func<TimeBinding, bool> predicate)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            return set.Cast<TimeBinding>().Where(predicate).ToList();
        }
        public static List<TimeBinding> GetAll()
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            return set.Cast<TimeBinding>().ToList();
        }
        public static List<TimeBinding> GetTBofDay(int day, int month, int year)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            return set.Cast<TimeBinding>().Where(x => x.Year == year && x.Month == month && x.Day == day).ToList();
        }
        public static List<TimeBinding> GetTBbyStepID(int stepid)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeBinding>();
            return set.Cast<TimeBinding>().Where(x => x.StepId == stepid).ToList();           
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
    }
}
