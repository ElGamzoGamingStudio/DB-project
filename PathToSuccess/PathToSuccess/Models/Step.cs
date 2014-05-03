using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("step", Schema = "public")]
    public class Step //todo
    {
        [Key]
        [Column ("id")] 
        public int Id { get; set; }
        
        [Required]
        [Column("begin_date")]
        public DateTime BeginDate { get; set; }

        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("urgency_name")]
        public string UrgencyName { get; set; }

        [Required]
        [Column("importance_name")]
        public string ImportanceName { get; set; }

        [Required]
        [Column("criteria_id")]
        public int CriteriaId { get; set; }

        [Required]
        [Column("timerule_id")]
        public int TimeruleId { get; set; }


    }
}
