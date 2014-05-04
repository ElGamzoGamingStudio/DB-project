using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("urgency", Schema="public")]
    public class Urgency
    {
        [Key]
        [Column("urgency_name")]
        public string UrgencyName { get; set; }

        [Column("value")]
        public int Value { get; set; }

        public Urgency() { }

        public Urgency(string name, int value)
        {
            UrgencyName = name;
            Value = value;
        }

        public List<Urgency> GetViableUrgencyLevels()
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Urgency>()
                .Cast<Urgency>()
                .ToList<Urgency>();
        }

        public bool ValueAlreadyUsed(int value)
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Urgency>()
                .Cast<Urgency>()
                .FirstOrDefault(x => x.Value == value) != null;
        }

        public int GetMaxValue()
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Urgency>()
                .Cast<Urgency>()
                .Max(x => x.Value);
        }

        public int GetValueByName(string name)
        {
            var item = DAL.SqlRepository.DBContext.GetDbSet<Urgency>()
                .Cast<Urgency>()
                .FirstOrDefault(x => x.UrgencyName == name);
            return item == null ? -1 : item.Value;
        }
    }
}
