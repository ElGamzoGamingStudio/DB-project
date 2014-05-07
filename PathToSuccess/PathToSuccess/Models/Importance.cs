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

        public static Importance CreateImportance(int value, string name)
        {
            var i = new Importance();
            i.Value = value;
            i.ImportanceName = name;
            DAL.SqlRepository.Importancies.Add(i);
            DAL.SqlRepository.Save();
            return i;
        }

        public static void DeleteImportance(string name)
        {
            var item = DAL.SqlRepository.Importancies.Find(name);
            if (item != null)
            {
                DAL.SqlRepository.Importancies.Remove(item);
                DAL.SqlRepository.Save();
            }
        }

        public static string GetLowestImportanceLevelName()
        {
            var importancies = DAL.SqlRepository.Importancies.Cast<Importance>();
            return importancies.Where(x => x.Value == importancies.Min(y => y.Value)).FirstOrDefault().ImportanceName;
        }

        public static Importance GetLowestImportance()
        {
            var impotancies = DAL.SqlRepository.Importancies.Cast<Importance>();
            return impotancies.Where(x => x.Value == impotancies.Min(y => y.Value)).FirstOrDefault();
        }

        public static Importance GetHighestImportance()
        {
            var impotancies = DAL.SqlRepository.Urgencies.Cast<Importance>();
            return impotancies.Where(x => x.Value == impotancies.Max(y => y.Value)).FirstOrDefault();
        }

        public static List<Importance> GetViableImportanceLevels()
        {
            return DAL.SqlRepository.Importancies
                .Cast<Importance>()
                .ToList<Importance>();
        }

        public static bool ValueAlreadyUsed(int value)
        {
            return DAL.SqlRepository.Importancies
                .Cast<Importance>()
                .FirstOrDefault(x => x.Value == value) != null;
        }

        public static int GetMaxValue()
        {
            return DAL.SqlRepository.Importancies
                .Cast<Importance>()
                .Max(x => x.Value);
        }

        public static int GetValueByName(string name)
        {
            var item = DAL.SqlRepository.Importancies
                .Cast<Importance>()
                .FirstOrDefault(x => x.ImportanceName == name);
            return item == null ? -1 : item.Value;
        }
    }
}
