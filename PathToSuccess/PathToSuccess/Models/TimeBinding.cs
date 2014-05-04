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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Column("step_id")]
        public int StepId { get; set; }

        [Column("time")]
        public DateTime Time { get; set; }

        [Column("day")]
        public int Day { get; set; }

        [Column("month")]
        public int Month { get; set; }

        [Column("year")]
        public int Year { get; set; }

        public TimeBinding(int id,int stepId,DateTime time, int day, int month, int year)
        {
            Id = id;
            StepId = stepId;
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
    }
}
