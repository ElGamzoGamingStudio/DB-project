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

        public static string GetLowestImportanceLevelName()
        {
            var importancies = DAL.SqlRepository.Urgencies.Cast<Importance>();
            return importancies.Where(x => x.Value == importancies.Min(y => y.Value)).FirstOrDefault().ImportanceName;
        }

        public static Importance GetLowestImportance()
        {
            var impotancies = DAL.SqlRepository.Urgencies.Cast<Importance>();
            return impotancies.Where(x => x.Value == impotancies.Min(y => y.Value)).FirstOrDefault();
        }

        public static List<Importance> GetViableImportanceLevels()
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .ToList<Importance>();
        }

        public static bool ValueAlreadyUsed(int value)
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .FirstOrDefault(x => x.Value == value) != null;
        }

        public static int GetMaxValue()
        {
            return DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .Max(x => x.Value);
        }

        public static int GetValueByName(string name)
        {
            var item = DAL.SqlRepository.DBContext.GetDbSet<Importance>()
                .Cast<Importance>()
                .FirstOrDefault(x => x.ImportanceName == name);
            return item == null ? -1 : item.Value;
        }
    }
}
