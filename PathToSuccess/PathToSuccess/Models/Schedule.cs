using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("schedule", Schema="public")]
    public class Schedule
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }

        [Column("monday")]
        public int MondayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval MondayInterval { get; set; }

        [Column("tuesday")]
        public int TuesdayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval TuesdayInterval { get; set; }

        [Column("wednesday")]
        public int WednesdayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval WednesdayInterval { get; set; }

        [Column("thirsday")]
        public int ThirsdayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval ThirsdayInterval { get; set; }

        [Column("friday")]
        public int FridayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval FridayInterval { get; set; }

        [Column("saturday")]
        public int SaturdayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval SaturdayInterval { get; set; }

        [Column("sunday")]
        public int SundayIntervalId { get; set; }
        [ForeignKey("Id")]
        public Interval SundayInterval { get; set; }


        public Schedule() { }

        public Schedule(Interval monday, Interval tuesday, Interval wednesday, Interval thirsday, Interval friday, Interval saturday, Interval sunday)
        {
            MondayInterval = monday;
            TuesdayInterval = tuesday;
            WednesdayInterval = wednesday;
            ThirsdayInterval = thirsday;
            FridayInterval = friday;
            SaturdayInterval = saturday;
            SundayInterval = sunday;

            MondayIntervalId    = monday == null    ? 0 : monday.Id;
            TuesdayIntervalId   = tuesday == null   ? 0 : tuesday.Id;
            WednesdayIntervalId = wednesday == null ? 0 : wednesday.Id;
            ThirsdayIntervalId  = thirsday == null  ? 0 : thirsday.Id;
            FridayIntervalId    = friday == null    ? 0 : friday.Id;
            SaturdayIntervalId  = saturday == null  ? 0 : saturday.Id;
            SundayIntervalId    = sunday == null    ? 0 : sunday.Id;
        }
    }
}
