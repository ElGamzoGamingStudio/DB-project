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
        public Step() { }
        public Step(Step toCopy)
        {
            Order = toCopy.Order;
            ParentTask = toCopy.ParentTask;
            TaskId = toCopy.TaskId;
            Description = toCopy.Description;
            TimeRule = toCopy.TimeRule;
            TimeRuleId = toCopy.TimeRuleId;
            CriteriaId = toCopy.CriteriaId;
            Criteria = toCopy.Criteria;
            Importance = toCopy.Importance;
            ImportanceName = toCopy.ImportanceName;
            Urgency = toCopy.Urgency;
            UrgencyName = toCopy.UrgencyName;
            Id = toCopy.Id;
            EndDate = toCopy.EndDate;
            BeginDate = toCopy.BeginDate;
        }
        /// <summary>
        /// Method to add new step to the database
        /// </summary>
        /// <param name="step">Create using a simple constructor</param>
        public static Step CreateStep(DateTime beginDate, DateTime endDate, string urgencyName, Urgency urgency,
                    string importanceName, Importance importance, int criteriaId, Criteria criteria,
                    int timeRuleId, TimeRule timeRule, string description, Task parentTask, int taskId, int order)
        {
            var set = DAL.SqlRepository.Steps;
            var step = (Step)set.Create(typeof(Step));

            step.BeginDate = beginDate;
            step.EndDate = endDate;
            step.UrgencyName = urgencyName;
            step.Urgency = urgency;
            step.ImportanceName = importanceName;
            step.Importance = importance;
            step.CriteriaId = criteriaId;
            step.Criteria = criteria;
            step.TimeRuleId = timeRuleId;
            step.TimeRule = timeRule;
            step.Description = description;
            step.ParentTask = parentTask;
            step.TaskId = taskId;
            step.Order = order;

            set.Add(step);
            DAL.SqlRepository.Save();
            return step;
        }
        public static void DeleteStep(Step step)
        {
            var set = DAL.SqlRepository.Steps;
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
            if (Criteria.IsCompleted()) return;
            Criteria.Inc();
            DAL.SqlRepository.Save();
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
            return DAL.SqlRepository.Steps
                .Cast<Step>()
                .Where(predicate)
                .ToList();
        }
        public static Step GetFirstUndoneStepByTaskID(int taskID)
        {
            var allSteps = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.TaskId == taskID).ToList();
            allSteps.OrderBy(x => x.Order);
            //foreach (var st in allSteps)
            //{
            //    if (!st.Criteria.IsCompleted())
            //        return st;
            //}
            //return null;
            return allSteps.FirstOrDefault(x => !x.Criteria.IsCompleted());
        }
        public int CompareTo(Step st)
        {
            return Order.CompareTo(st.Order);
        }

        public void UpdateUrgency()
        {
            int maxvalue = Urgency.GetMaxValue();
            double timePassed = (EndDate - DateTime.Now).Ticks / (EndDate - BeginDate).Ticks; // 0..1
            var desiredUrgencyValue = timePassed * maxvalue;
            var urg = DAL.SqlRepository.Urgencies.Cast<Urgency>()
                .OrderBy(x => x.Value)
                .FirstOrDefault(x => x.Value > desiredUrgencyValue); // first urgency with value above desired
            this.Urgency = urg;
            this.UrgencyName = urg.UrgencyName;
        }
    }
}
