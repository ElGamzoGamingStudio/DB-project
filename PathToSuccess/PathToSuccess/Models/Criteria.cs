using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("criteria", Schema="public")]
    public class Criteria
    {
        protected bool Equals(Criteria other)
        {
            return Id == other.Id && CurrentValue == other.CurrentValue && TargetValue == other.TargetValue && string.Equals(Unit, other.Unit);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode*397) ^ CurrentValue;
                hashCode = (hashCode*397) ^ TargetValue;
                hashCode = (hashCode*397) ^ (Unit != null ? Unit.GetHashCode() : 0);
                return hashCode;
            }
        }

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
        public Criteria(Criteria toCopy)
        {
            Id = toCopy.Id;
            Unit = toCopy.Unit;
            TargetValue = toCopy.TargetValue;
            CurrentValue = toCopy.CurrentValue;
        }
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Criteria) obj);
        }
    }
}
