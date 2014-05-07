using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    
    [Table("schedule", Schema="public")]
    public class Schedule
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("monday")]
        public int MondayIntervalId { get; set; } 
        [ForeignKey("MondayIntervalId")]
        public Interval MondayInterval { get; set; }

        [Column("tuesday")]
        public int TuesdayIntervalId { get; set; }
        [ForeignKey("TuesdayIntervalId")]
        public Interval TuesdayInterval { get; set; }

        [Column("wednesday")]
        public int WednesdayIntervalId { get; set; }
        [ForeignKey("WednesdayIntervalId")]
        public Interval WednesdayInterval { get; set; }

        [Column("thursday")]
        public int ThursdayIntervalId { get; set; }
        [ForeignKey("ThursdayIntervalId")]
        public Interval ThursdayInterval { get; set; }

        [Column("friday")]
        public int FridayIntervalId { get; set; }
        [ForeignKey("FridayIntervalId")]
        public Interval FridayInterval { get; set; }

        [Column("saturday")]
        public int SaturdayIntervalId { get; set; }
        [ForeignKey("SaturdayIntervalId")]
        public Interval SaturdayInterval { get; set; }

        [Column("sunday")]
        public int SundayIntervalId { get; set; }
        [ForeignKey("SundayIntervalId")]
        public Interval SundayInterval { get; set; }


        public Schedule() { }

        public static Schedule CreateSchedule(Interval monday, Interval tuesday, Interval wednesday, Interval thursday, Interval friday, Interval saturday, Interval sunday)
        {
            var set = DAL.SqlRepository.Schedules;
            var item = (Schedule)set.Create(typeof(Schedule));

            item.MondayInterval = monday;
            item.TuesdayInterval = tuesday;
            item.WednesdayInterval = wednesday;
            item.ThursdayInterval = thursday;
            item.FridayInterval = friday;
            item.SaturdayInterval = saturday;
            item.SundayInterval = sunday;

            item.MondayIntervalId = monday == null ? -1 : monday.Id;
            item.TuesdayIntervalId = tuesday == null ? -1 : tuesday.Id;
            item.WednesdayIntervalId = wednesday == null ? -1 : wednesday.Id;
            item.ThursdayIntervalId = thursday == null ? -1 : thursday.Id;
            item.FridayIntervalId = friday == null ? -1 : friday.Id;
            item.SaturdayIntervalId = saturday == null ? -1 : saturday.Id;
            item.SundayIntervalId = sunday == null ? -1 : sunday.Id;

            DAL.SqlRepository.Schedules.Add(item);
            DAL.SqlRepository.Save();

            return item;
        }

        public static void DeleteSchedule(Schedule schedule)
        {
            var item = DAL.SqlRepository.Schedules.Find(schedule.Id);
            if (item != null)
            {
                DAL.SqlRepository.Schedules.Remove(item);
                DAL.SqlRepository.Save();
            }
        }
        public static List<IntervalInfo> GetNotEmptyIntervals(int id)
        {
            //TODO: should return not null intervals with dayOfWeek accepted by DateTime.DayOfWeek by schedule id
            return null;
        }

        //public void LoadIntervals()
        //{
        //    MondayInterval = (Interval)DAL.SqlRepository.Intervals.Find(MondayIntervalId);
        //    TuesdayInterval = (Interval)DAL.SqlRepository.Intervals.Find(TuesdayIntervalId);
        //    WednesdayInterval = (Interval)DAL.SqlRepository.Intervals.Find(WednesdayIntervalId);
        //    ThursdayInterval = (Interval)DAL.SqlRepository.Intervals.Find(ThursdayIntervalId);
        //    FridayInterval = (Interval)DAL.SqlRepository.Intervals.Find(FridayIntervalId);
        //    SaturdayInterval = (Interval)DAL.SqlRepository.Intervals.Find(SaturdayIntervalId);
        //    SundayInterval = (Interval)DAL.SqlRepository.Intervals.Find(SundayIntervalId);
        //}
    }

    public struct IntervalInfo
    {
        public DayOfWeek dayOfWeek;
        public int intervalID;
    }
}
