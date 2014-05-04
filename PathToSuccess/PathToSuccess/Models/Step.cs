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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }
        
        [Required]
        [Column("begin_date")]
        public DateTime BeginDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("urgency_name")]
        public string UrgencyName { get; set; }
        [ForeignKey("Name")]
        public Urgency Urgency { get; set; }

        [Required]
        [Column("importance_name")]
        public string ImportanceName { get; set; }
        [ForeignKey("Name")]
        public Importance Importance { get; set; }

        [Required]
        [Column("criteria_id")]
        public int CriteriaId { get; set; }
        [ForeignKey("Id")]
        public Criteria Criteria { get; set; }

        [Column("timerule_id")]
        public int TimeRuleId { get; set; }
        [ForeignKey("Id")]
        public TimeRule TimeRule { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        
        //methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate">please USE !!DATETIME.MIN_VALUE!! if is not finished</param>
        /// <param name="urgencyName"></param>
        /// <param name="importanceName"></param>
        /// <param name="importance"></param>
        /// <param name="criteriaId"></param>
        /// <param name="criteria"></param>
        /// <param name="timeRuleId"></param>
        /// <param name="timeRule"></param>
        /// <param name="description">actual text of the step</param>
        public Step(int id, DateTime beginDate, DateTime endDate, string urgencyName,
                    string importanceName, Importance importance, int criteriaId, Criteria criteria, 
                    int timeRuleId, TimeRule timeRule, string description)
        {
            Id = id;
            BeginDate = beginDate;
            EndDate = endDate;
            UrgencyName = urgencyName;
            ImportanceName = importanceName;
            Importance = importance;
            CriteriaId = criteriaId;
            Criteria = criteria;
            TimeRuleId = timeRuleId;
            TimeRule = timeRule;
            Description = description;
        }

        /// <summary>
        /// Method to add new step to the database
        /// </summary>
        /// <param name="step">Create using a simple constructor</param>
        public static void CreateStep(Step step)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Step>();

            //if (set.Find(step.Id) != null) return;
            set.Add(step);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        /// <summary>
        /// Writes DateTime.Now to EndDate parameter
        /// </summary>
        public void EndTask()
        {
            if(!DateTime.MinValue.Equals(EndDate)) return; //is finished or broken
            EndDate = DateTime.Now;
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        /// <summary>
        /// Such pro wow
        /// </summary>
        /// <returns>integer-based rating representing position in the querry</returns>
        public int CalculateScheduleRating() //todo
        {
            //need importance + urgency to be done
            return int.MinValue;
        }

    }
}
