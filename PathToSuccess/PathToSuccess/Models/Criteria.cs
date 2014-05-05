using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("criteria", Schema="public")]
    public class Criteria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("current_value")]
        public int CurrentValue { get; set; }

        [Required]
        [Column("target_value")]
        public int TargetValue { get; set; }

        [Column("unit")]
        public string Unit { get; set; }

        public Criteria() { }

        public Criteria(int currentValue, int targetValue, string unit)
        {
            CurrentValue = currentValue;
            TargetValue = targetValue;
            Unit = unit;
        }

        public static void CreateCriteria(Criteria criteria)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Criteria>();
            set.Add(criteria);
            DAL.SqlRepository.DBContext.SaveChanges();
        }

        public static void DeleteCriteria(Criteria criteria)
        {
            var set = DAL.SqlRepository.DBContext.GetDbSet<Criteria>();
            var cr = set.Find(criteria.Id);
            if (cr != null)
            {
                set.Remove(cr);
                DAL.SqlRepository.DBContext.SaveChanges();
            }
        }

        public bool IsCompleted()
        {
            return CurrentValue >= TargetValue;
        }
    }
}
