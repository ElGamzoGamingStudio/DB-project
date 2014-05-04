using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("importance", Schema="public")]
    public class Importance
    {
        [Key]
        [Column("importance_name")]
        public string ImportanceName { get; set; }

        [Column("value")]
        public int Value { get; set; }

        public Importance() { }

        public Importance(string name, int value)
        {
            ImportanceName = name;
            Value = value;
        }

        public List<Importance> GetViableUrgencyLevels()
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .ToList<Importance>();
        }

        public bool ValueAlreadyUsed(int value)
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .FirstOrDefault(x => x.Value == value) != null;
        }

        public int GetMaxValue()
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .Max(x => x.Value);
        }

        public int GetValueByName(string name)
        {
            var item = DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .FirstOrDefault(x => x.ImportanceName == name);
            return item == null ? -1 : item.Value;
        }
    }
}
