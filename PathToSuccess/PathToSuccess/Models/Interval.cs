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

        public static Interval CreateInterval(DateTime begin, DateTime end)
        {
            var set = DAL.SqlRepository.Intervals;
            var interval = new Interval();
            interval.BeginTime = begin;
            interval.EndTime = end;
            set.Add(interval);
            DAL.SqlRepository.Save();
            return interval;
        }

        public static void DeleteInterval(Interval interval)
        {
            var set = DAL.SqlRepository.Intervals;
            var i = set.Find(interval.Id);
            if (i != null)
            {
                set.Remove(i);
                DAL.SqlRepository.Save();
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
            if (DAL.SqlRepository.Intervals.Find(-1) == null)
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

        public override string ToString()
        {
            return "From = " + BeginTime.ToString() + " To" + EndTime.ToString();
        }
    }
}
