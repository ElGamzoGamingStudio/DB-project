﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    
    [Table("schedule", Schema="public")]
    public class Schedule
    {
        protected bool Equals(Schedule other)
        {
            return Id == other.Id && MondayIntervalId == other.MondayIntervalId &&
                   Equals(MondayInterval, other.MondayInterval) && TuesdayIntervalId == other.TuesdayIntervalId &&
                   Equals(TuesdayInterval, other.TuesdayInterval) && WednesdayIntervalId == other.WednesdayIntervalId &&
                   Equals(WednesdayInterval, other.WednesdayInterval) && ThursdayIntervalId == other.ThursdayIntervalId &&
                   Equals(ThursdayInterval, other.ThursdayInterval) && FridayIntervalId == other.FridayIntervalId &&
                   Equals(FridayInterval, other.FridayInterval) && SaturdayIntervalId == other.SaturdayIntervalId &&
                   Equals(SaturdayInterval, other.SaturdayInterval) && SundayIntervalId == other.SundayIntervalId &&
                   Equals(SundayInterval, other.SundayInterval) && other.Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ MondayIntervalId;
                hashCode = (hashCode*397) ^ (MondayInterval != null ? MondayInterval.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ TuesdayIntervalId;
                hashCode = (hashCode*397) ^ (TuesdayInterval != null ? TuesdayInterval.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ WednesdayIntervalId;
                hashCode = (hashCode*397) ^ (WednesdayInterval != null ? WednesdayInterval.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ ThursdayIntervalId;
                hashCode = (hashCode*397) ^ (ThursdayInterval != null ? ThursdayInterval.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ FridayIntervalId;
                hashCode = (hashCode*397) ^ (FridayInterval != null ? FridayInterval.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ SaturdayIntervalId;
                hashCode = (hashCode*397) ^ (SaturdayInterval != null ? SaturdayInterval.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ SundayIntervalId;
                hashCode = (hashCode*397) ^ (SundayInterval != null ? SundayInterval.GetHashCode() : 0);
                return hashCode;
            }
        }

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
        [Column("name")]
        public string Name { get; set; }

        public Schedule() { }

        public static Schedule CreateSchedule(Interval monday, Interval tuesday, Interval wednesday, Interval thursday, Interval friday, Interval saturday, Interval sunday, string name)
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
            item.Name = name;

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
            var result = new List<IntervalInfo>();
            var sch = (Schedule) DAL.SqlRepository.Schedules.Find(id);
            if (sch.MondayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Monday;
                ii.intervalID = sch.MondayIntervalId;
                result.Add(ii);
            }
            if (sch.TuesdayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Tuesday;
                ii.intervalID = sch.TuesdayIntervalId;
                result.Add(ii);
            }
            if (sch.WednesdayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Wednesday;
                ii.intervalID = sch.WednesdayIntervalId;
                result.Add(ii);
            }
            if (sch.ThursdayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Thursday;
                ii.intervalID = sch.ThursdayIntervalId;
                result.Add(ii);
            }
            if (sch.FridayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Friday;
                ii.intervalID = sch.FridayIntervalId;
                result.Add(ii);
            }
            if (sch.SaturdayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Saturday;
                ii.intervalID = sch.SaturdayIntervalId;
                result.Add(ii);
            }
            if (sch.SundayInterval.Id != -1)
            {
                var ii = new IntervalInfo();
                ii.dayOfWeek = DayOfWeek.Sunday;
                ii.intervalID = sch.SundayIntervalId;
                result.Add(ii);
            }
            return result;
        }
        public bool IsPios()
        {
        //    if (MondayInterval.Id != -1)
        //        if (MondayInterval.EndTime == Interval.PIOS)
        //            return true;
        //    if (TuesdayInterval.Id != -1)
        //        if (TuesdayInterval.EndTime == Interval.PIOS)
        //            return true;
        //    if (WednesdayInterval.Id != -1)
        //        if (WednesdayInterval.EndTime == Interval.PIOS)
        //            return true;
        //    if (ThursdayInterval.Id != -1)
        //        if (ThursdayInterval.EndTime == Interval.PIOS)
        //            return true;
        //    if (FridayInterval.Id != -1)
        //        if (FridayInterval.EndTime == Interval.PIOS)
        //            return true;
        //    if (SaturdayInterval.Id != -1)
        //        if (SaturdayInterval.EndTime == Interval.PIOS)
        //            return true;
        //    if (SundayInterval.Id != -1)
        //        if (SundayInterval.EndTime == Interval.PIOS)
        //            return true;
            return false;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Schedule) obj);
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
