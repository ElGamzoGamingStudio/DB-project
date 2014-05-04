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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Required]
        [Column("is_periodic")]
        public bool IsPeriodic { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }
        [ForeignKey("Name")]
        public Schedule Schedule { get; set; }

        public TimeRule(int id, bool isPeriodic, int scheduleId, Schedule schedule)
        {
            Id = id;
            IsPeriodic = isPeriodic;
            ScheduleId = scheduleId;
            Schedule = schedule;
        }
        public static void CreateTimeRule(TimeRule timeRule)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeRule>();

            set.Add(timeRule);
            DAL.SqlRepository.DBContext.SaveChanges();
        }
        public static void DeleteTimeRule(TimeRule timeRule)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<TimeRule>();
            var tr = set.Find(timeRule.Id);
            if (tr != null)
            {
                set.Remove(timeRule);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }
    }
}
