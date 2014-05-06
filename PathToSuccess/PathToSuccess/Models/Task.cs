using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Devart.Common;

namespace PathToSuccess.Models
{
    [Table("task", Schema = "public")]
    public class Task //todo
    {
        [Key]
        [Column("id")]
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

        [Required]
        [Column("description")]
        public string Description { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Task Parent { get; set; }



        //methods

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="urgencyName"></param>
        /// <param name="importanceName"></param>
        /// <param name="importance"></param>
        /// <param name="criteriaId"></param>
        /// <param name="criteria"></param>
        /// <param name="description">Actual text ot the task</param>
        /// <param name="parent"></param>
        /// <param name="parentId"></param>
        public Task(DateTime beginDate, DateTime endDate, string urgencyName, string importanceName, 
                    Importance importance, Urgency urgency, int criteriaId, Criteria criteria, string description, Task parent, int parentId)
        {
            BeginDate = beginDate;
            EndDate = endDate;
            UrgencyName = urgencyName;
            ImportanceName = importanceName;
            Importance = importance;
            Urgency = urgency;
            CriteriaId = criteriaId;
            Criteria = criteria;
            Description = description;
            Parent = parent;
            ParentId = parentId;
        }

        public Task() { }

        /// <summary>
        /// Method to add new task to the database
        /// </summary>
        /// <param name="task">Create using a simple constructor</param>
        public static void CreateTask(Task task)
        {
            var set = DAL.SqlRepository.Tasks;

            //if (set.Find(task.Id) != null) return;
            set.Add(task);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static void DeleteTask(Task task)
        {
            var set = DAL.SqlRepository.Tasks;
            var toDelete = set.Find(task.Id);
            if (toDelete == null) return;
            set.Remove(toDelete);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        /// <summary>
        /// Esli potsik vipolnil task
        /// </summary>
        public void Do()
        {
            var set = DAL.SqlRepository.Tasks;
            if (Criteria.IsCompleted()) return;
            Criteria.Inc();
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        /// <summary>
        /// Veri gut methisdsda
        /// </summary>
        /// <returns>double-based rating representing position in the querry</returns>
        public double CalculateScheduleRating()
        {
            return Importance.Value * Math.Exp(Urgency.Value);
        }

        public static List<Task> Select(Func<Task, bool> predicate)
        {
            var set = DAL.SqlRepository.Tasks;
            return set.Cast<Task>().Where(predicate).ToList();
        }

        public List<Task> SelectChildrenTasks()
        {
            //var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();
            //var childrenSet = set.Cast<Task>().Where(x => x.Parent == this);
            return DAL.SqlRepository.Tasks.Cast<Task>().Where(x => x.Parent == this).ToList();
        }
        public List<Task> SelectChildrenTasks(Func<Task, bool> predicate)
        {
            //var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();
            var childrenSet = DAL.SqlRepository.Tasks.Cast<Task>().Where(x => x.Parent == this);
            return childrenSet.Where(predicate).ToList();
        }
        
        public List<Step> SelectChildrenSteps()
        {
            return DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.ParentTask == this).ToList();
        }
        public List<Step> SelectChildrenStep(Func<Step, bool> predicate)
        {
            //var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();
            var childrenSet = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.ParentTask == this);
            return childrenSet.Where(predicate).ToList();
        }

        public bool ChildrenAreSteps()
        {
            var childrenStepSet = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.ParentTask == this);
            //var childrenTaskSet = DAL.SqlRepository.DBContext.GetDbSet<Task>().Cast<Task>().Where(x => x.Parent == this);
            return childrenStepSet.Any();
        }

        public bool HasUncompletedSteps()
        {
            var childrenStepSet = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.ParentTask == this);
            return childrenStepSet.Count(x => !x.Criteria.IsCompleted()) == 0;
        }
        public static List<Task> GetLowestTasks()
        {
            //TODO: should return all tasks with uncompleted steps
            var tasksWithStepsChildren = DAL.SqlRepository.Tasks.Cast<Task>().Where(t => t.ChildrenAreSteps());
            return tasksWithStepsChildren.Where(t => t.HasUncompletedSteps()).ToList();
        }
    }
}
