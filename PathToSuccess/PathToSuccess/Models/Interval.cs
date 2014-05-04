using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("interval", Schema="public")]
    public class Interval
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Column("begin_time")]
        public DateTime BeginTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        public Interval() { }

        public Interval(DateTime beginTime, DateTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public static void CreateInterval(Interval interval)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Interval>();
            set.Add(interval);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static void DeleteInterval(Interval interval)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Interval>();
            var i = set.Find(interval.Id);
            if (i != null)
            {
                set.Remove(i);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }

        public TimeSpan GetIntervalLength()
        {
            return EndTime - BeginTime;
        }

        public void Postpone(TimeSpan time)
        {
            EndTime += time;
        }
    }
}
