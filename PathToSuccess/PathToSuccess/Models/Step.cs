using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Devart.Common;
using LinqToDB.Common;

namespace PathToSuccess.Models
{
    [Table("step", Schema = "public")]
    public class Step
    {
        [Key]
        [Column ("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [Column("begin_date")]
        public DateTime BeginDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("urgency_name")]
        public string UrgencyName { get; set; }
        [ForeignKey("UrgencyName")]
        public Urgency Urgency { get; set; }

        [Required]
        [Column("importance_name")]
        public string ImportanceName { get; set; }
        [ForeignKey("ImportanceName")]
        public Importance Importance { get; set; }

        [Required]
        [Column("criteria_id")]
        public int CriteriaId { get; set; }
        [ForeignKey("CriteriaId")]
        public Criteria Criteria { get; set; }

        [Column("timerule_id")]
        public int TimeRuleId { get; set; }
        [ForeignKey("TimeRuleId")]
        public TimeRule TimeRule { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Column("task_id")]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task ParentTask { get; set; }

        [Column("order")]
        public int Order { get; set; }

        
        //methods

        /// <summary>
        /// 
        /// </summary>
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
        /// <param name="parentTask"></param>
        /// <param name="taskId"></param>
        public Step(DateTime beginDate, DateTime endDate, string urgencyName, Urgency urgency,
                    string importanceName, Importance importance, int criteriaId, Criteria criteria, 
                    int timeRuleId, TimeRule timeRule, string description, Task parentTask, int taskId, int order)
        {
            BeginDate = beginDate;
            EndDate = endDate;
            UrgencyName = urgencyName;
            Urgency = urgency;
            ImportanceName = importanceName;
            Importance = importance;
            CriteriaId = criteriaId;
            Criteria = criteria;
            TimeRuleId = timeRuleId;
            TimeRule = timeRule;
            Description = description;
            ParentTask = parentTask;
            TaskId = taskId;
            Order = order;
        }

        public Step() { }

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
        public static void DeleteStep(Step step)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Step>();
            var toDelete = set.Find(step.Id);
            if (toDelete == null) return;
            set.Remove(toDelete);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        /// <summary>
        /// Esli potsik vipolnil step
        /// </summary>
        public void Do()
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Step>();
            if (Criteria.IsCompleted()) return;
            Criteria.Inc();
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        /// <summary>
        /// Such pro wow
        /// </summary>
        /// <returns>double-based rating representing position in the querry</returns>
        public double CalculateScheduleRating()
        {
            return Importance.Value * Math.Exp(Urgency.Value);
        }

        public static List<Step> Select(Func<Step, bool> predicate)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Step>();
            return set.Cast<Step>().Where(predicate).ToList();
        }
        public Step GetFirstUndoneStepByTaskID(int taskID)
        {
            //TODO: Should return the first ndone step of this task
            var steps = ParentTask.SelectChildrenStep(st => !(st.Criteria.IsCompleted()));
            return steps.IsNullOrEmpty() ? null : steps[0];
            return null;
        }
        
    }
}
