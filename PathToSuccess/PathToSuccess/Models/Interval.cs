using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Sql;
using System.Collections.Generic;
using System.Linq;
using PathToSuccess.DAL;

namespace PathToSuccess.Models
{
    [Table("interval", Schema="public")]
    public class Interval
    {
        protected bool Equals(Interval other)
        {
            return Id == other.Id && BeginTime.Equals(other.BeginTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ BeginTime.GetHashCode();
                hashCode = (hashCode*397) ^ EndTime.GetHashCode();
                return hashCode;
            }
        }

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

        public static readonly DateTime PIOS = new DateTime(1000, 8, 6, 4, 2, 0);

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
        public static void RemoveTrash()
        {
            var allInts = DAL.SqlRepository.Intervals.Cast<Interval>().ToList();
            var allSch = DAL.SqlRepository.Schedules.Cast<Schedule>().ToList();
            foreach (var sch in allSch)
            {
                foreach (var intervinf in PathToSuccess.Models.Schedule.GetNotEmptyIntervals(sch.Id))
                {
                    if (allInts.Contains(Interval.GetIntervalByID(intervinf.intervalID)))
                        allInts.Remove(Interval.GetIntervalByID(intervinf.intervalID));
                }
            }
            for (int i = allInts.Count-1; i >= 0; i--)
            {
                var interv = allInts[i];
                SqlRepository.Intervals.Remove(interv);
            }
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Interval) obj);
        }
    }
}
