using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Sql;

namespace PathToSuccess.Models
{
    [Table("interval", Schema="public")]
    public class Interval
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
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
            return EndTime == DateTime.MinValue ? new TimeSpan(0) : EndTime - BeginTime;
        }

        public void Postpone(TimeSpan time)
        {
            if (EndTime != null)
                EndTime += time;
        }

        public static void Seed()
        {
            if (DAL.SqlRepository.DBContext.GetDbSet<Interval>().Find(-1) == null)
            {
                DAL.SqlRepository.DBContext.Database.ExecuteSqlCommand(
                    "insert into public.interval values (-1, @date, @date);", new Npgsql.NpgsqlParameter("date", DateTime.Now)
                    );
            }
        }
        public static Interval GetIntervalByID(int id)
        {
            return (Interval) DAL.SqlRepository.Intervals.Find(id);
        }
    }
}
