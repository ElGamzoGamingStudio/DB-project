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

        public static Criteria CreateCriteria(int currentValue, int targetValue, string unit)
        {
            var set = DAL.SqlRepository.Criterias;
            var criteria = (Criteria)set.Create(typeof(Criteria));

            criteria.CurrentValue = currentValue;
            criteria.TargetValue = targetValue;
            criteria.Unit = unit;

            DAL.SqlRepository.Criterias.Add(criteria);
            DAL.SqlRepository.Save();
            return criteria;
        }

        public static void DeleteCriteria(Criteria criteria)
        {
            var set = DAL.SqlRepository.Criterias;
            var cr = set.Find(criteria.Id);
            if (cr != null)
            {
                set.Remove(cr);
                DAL.SqlRepository.Save();
            }
        }

        public void Inc()
        {
            CurrentValue++;
        }
        public bool IsCompleted()
        {
            return CurrentValue >= TargetValue;
        }
    }
}
