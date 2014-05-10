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

        public Task() { }
        public Task(Task toCopy)
        {
            Id = toCopy.Id;
            Description = toCopy.Description;
            ParentId = toCopy.ParentId;
            Parent = new Task(toCopy.Parent);
            Criteria = new Criteria(toCopy.Criteria);
            CriteriaId = toCopy.CriteriaId;
            Urgency = toCopy.Urgency;
            UrgencyName = toCopy.UrgencyName;
            Importance = toCopy.Importance;
            ImportanceName = toCopy.ImportanceName;
            EndDate = toCopy.EndDate;
            BeginDate = toCopy.BeginDate;
        }
        /// <summary>
        /// Method to add new task to the database
        /// </summary>
        /// <param name="task">Create using a simple constructor</param>
        public static Task CreateTask(DateTime beginDate, DateTime endDate, string urgencyName, string importanceName,
                    Importance importance, Urgency urgency, int criteriaId, Criteria criteria, string description, Task parent, int parentId)
        {
            var set = DAL.SqlRepository.Tasks;
            var task = (Task)set.Create(typeof(Task));

            task.BeginDate = beginDate;
            task.EndDate = endDate;
            task.Urgency = urgency;
            task.UrgencyName = urgencyName;
            task.Importance = importance;
            task.ImportanceName = importanceName;
            task.Criteria = criteria;
            task.CriteriaId = criteriaId;
            task.Description = description;
            task.Parent = parent;
            task.ParentId = parentId;

            DAL.SqlRepository.Tasks.Add(task);
            DAL.SqlRepository.Save();

            return task;
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
            var set = DAL.SqlRepository.Tasks.Cast<Models.Task>().ToList();
            return set.Where(predicate).ToList();
        }

        public List<Task> SelectChildrenTasks()
        {
            //var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();
            //var childrenSet = set.Cast<Task>().Where(x => x.Parent == this);
            return DAL.SqlRepository.Tasks.Cast<Task>().ToList().Where(x => x.ParentId == this.Id).ToList();
        }
        public List<Task> SelectChildrenTasks(Func<Task, bool> predicate)
        {
            //var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();
            var childrenSet = DAL.SqlRepository.Tasks.Cast<Task>().Where(x => x.ParentId == this.Id);
            return childrenSet.Where(predicate).ToList();
        }
        
        public List<Step> SelectChildrenSteps()
        {
            return DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.ParentTask.Id == this.Id).ToList();
        }
        public List<Step> SelectChildrenStep(Func<Step, bool> predicate)
        {
            //var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();
            var childrenSet = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.ParentTask.Id == this.Id);
            return childrenSet.Where(predicate).ToList();
        }

        public bool ChildrenAreSteps()
        {
            var childrenStepSet = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.TaskId == this.Id);
            //var childrenTaskSet = DAL.SqlRepository.DBContext.GetDbSet<Task>().Cast<Task>().Where(x => x.Parent == this);
            return childrenStepSet.Any();
        }

        public bool HasUncompletedSteps()
        {
            var childrenStepSet = DAL.SqlRepository.Steps.Cast<Step>().Where(x => x.TaskId == this.Id).ToList();
            foreach (var step in childrenStepSet)
            {
                if (!step.Criteria.IsCompleted())
                    return true;
            }
            return false;
        }
        public static List<Task> GetLowestTasks()
        {
            //TODO: should return all tasks with uncompleted steps
            var tasksWithStepsChildren = DAL.SqlRepository.Tasks.Cast<Task>().ToList().Where(t => t.ChildrenAreSteps()).ToList();
            return tasksWithStepsChildren.Where(t => t.HasUncompletedSteps()).ToList();
        }

        public static Task GetOldestParent(Task child)
        {
            return child.ParentId == -1 ? child : GetOldestParent(child.Parent);
        }

        public bool HasUncomplitedTasks()
        {
            var list = DAL.SqlRepository.Tasks.Cast<Task>().Where(t => t.ParentId == Id).ToList();
            bool res=HasUncompletedSteps();
            if (res) return true;
            foreach (var task1 in list)
            {
                res = task1.HasUncomplitedTasks();
                if (res) return true;
            }
            return false;
        }
        public static void CascadeRemoving(Task targetTask)
        {
            
            var children = targetTask.SelectChildrenTasks();
            var steps = targetTask.SelectChildrenSteps();
            foreach (var step in steps)
            {
                DAL.SqlRepository.Steps.Remove(step);
            }
            foreach (var child in children)
            {
                CascadeRemoving(child);
            }
            DAL.SqlRepository.Tasks.Remove(targetTask);
        }

        public static List<Task> SelectAllTreeTask(int taskId)
        {
            var q = DAL.SqlRepository.Tasks.Cast<Task>();
            var p = new List<Task>();
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var task in q)
                // ReSharper restore LoopCanBeConvertedToQuery
            {
                if (GetOldestParent(task).Id == taskId)
                    p.Add(task);
            }
            return p;
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
