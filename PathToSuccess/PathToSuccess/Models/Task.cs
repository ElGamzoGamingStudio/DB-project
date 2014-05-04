using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("task", Schema = "public")]
    public class Task //todo
    {
        [Key]
        [Column("id")]
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
        [ForeignKey ("Name")]
        public Urgency Urgency { get; set; }

        [Required]
        [Column("importance_name")]
        public string ImportanceName { get; set; }
        [ForeignKey ("Name")]
        public Importance Importance { get; set; }
    
        [Required]
        [Column("criteria_id")]
        public int CriteriaId { get; set; }
        [ForeignKey ("Id")]
        public Criteria Criteria { get; set; }

        [Required]
        [Column("description")]
        public string Description { get; set; }



        //methods

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate">please USE !!DATETIME.MIN_VALUE!! if is not finished</param>
        /// <param name="urgencyName"></param>
        /// <param name="importanceName"></param>
        /// <param name="importance"></param>
        /// <param name="criteriaId"></param>
        /// <param name="criteria"></param>
        /// <param name="description">Actual text ot the task</param>
        public Task(int id, DateTime beginDate, DateTime endDate, string urgencyName, string importanceName, 
                    Importance importance, int criteriaId, Criteria criteria, string description)
        {
            Id = id;
            BeginDate = beginDate;
            EndDate = endDate;
            UrgencyName = urgencyName;
            ImportanceName = importanceName;
            Importance = importance;
            CriteriaId = criteriaId;
            Criteria = criteria;
            Description = description;
        }

        /// <summary>
        /// Method to add new task to the database
        /// </summary>
        /// <param name="task">Create using a simple constructor</param>
        public static void CreateTask(Task task)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Task>();

            //if (set.Find(task.Id) != null) return;
            set.Add(task);
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
        /// Veri gut methisdsda
        /// </summary>
        /// <returns>integer-based rating representing position in the querry</returns>
        public int CalculateScheduleRating() //todo
        {
            //need importance + urgency to be done
            return int.MinValue;
        }
    }
}
