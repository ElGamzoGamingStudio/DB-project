using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    [Table("timerule", Schema = "public")]
    public class TimeRule
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("is_periodic")]
        public bool IsPeriodic { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }

        [Column("is_user_approved")]
        public bool IsUserApproved { get; set; }

        public TimeRule()
        { }

        public TimeRule(TimeRule toCopy)
        {
            Schedule = toCopy.Schedule;
            IsUserApproved = toCopy.IsUserApproved;
            IsPeriodic = toCopy.IsPeriodic;
            ScheduleId = toCopy.ScheduleId;
            Id = toCopy.Id;
        }
        
        public static TimeRule CreateTimeRule(bool isPeriodic, int scheduleId, Schedule schedule)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeRule>();
            var tr = new TimeRule();

            tr.IsPeriodic = isPeriodic;
            tr.ScheduleId = scheduleId;
            tr.Schedule = schedule;
            tr.IsUserApproved = schedule.IsPios();
            set.Add(tr);
            DAL.SqlRepository.Save();
            return tr;
        }
        public static void DeleteTimeRule(TimeRule timeRule)
        {
            var set = DAL.SqlRepository.TimeRules;
            var tr = set.Find(timeRule.Id);
            if (tr != null)
            {
                set.Remove(timeRule);
                DAL.SqlRepository.Save();
            }
        }
        public static List<TimeRule> GetAll()
        {
            return DAL.SqlRepository.TimeRules
                .Cast<TimeRule>()
                .ToList();
        }
        public static List<TimeRule> GetPeriodic()
        {
            return DAL.SqlRepository.TimeRules
                .Cast<TimeRule>()
                .Where(x => x.IsPeriodic == true)
                .ToList();
        }
        public static TimeRule GetByID(int id)
        {
            return (TimeRule) DAL.SqlRepository.TimeRules.Find(id);
        }
        public static List<TimeRule> GetApproved()
        {
            return DAL.SqlRepository.TimeRules
                .Cast<TimeRule>()
                .Where(x => x.IsUserApproved == true)
                .ToList();
        }
    }
}
