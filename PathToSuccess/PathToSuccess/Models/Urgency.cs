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

        public static Urgency CreateUrgency(int value, string name)
        {
            var u = new Urgency();
            u.Value = value;
            u.UrgencyName = name;
            DAL.SqlRepository.Urgencies.Add(u);
            DAL.SqlRepository.Save();
            return u;
        }

        public static void DeleteImportance(string name)
        {
            var item = DAL.SqlRepository.Urgencies.Find(name);
            if (item != null)
            {
                DAL.SqlRepository.Urgencies.Remove(item);
                DAL.SqlRepository.Save();
            }
        }

        public static string GetLowestUrgencyLevelName()
        {
            var urgencies = DAL.SqlRepository.Urgencies.Cast<Urgency>();
            return urgencies.Where(x => x.Value == urgencies.Min(y => y.Value)).FirstOrDefault().UrgencyName;
        }

        public static Urgency GetLowestUrgency()
        {
            var urgencies = DAL.SqlRepository.Urgencies.Cast<Urgency>();
            return urgencies.Where(x => x.Value == urgencies.Min(y => y.Value)).FirstOrDefault();
        }

        public static Urgency GetHighestUrgency()
        {
            var urgencies = DAL.SqlRepository.Urgencies.Cast<Urgency>();
            return urgencies.Where(x => x.Value == urgencies.Max(y => y.Value)).FirstOrDefault();
        }

        public static List<Urgency> GetViableUrgencyLevels()
        {
            return DAL.SqlRepository.Urgencies
                .Cast<Urgency>()
                .ToList<Urgency>();
        }

        public static bool ValueAlreadyUsed(int value)
        {
            return DAL.SqlRepository.Urgencies
                .Cast<Urgency>()
                .FirstOrDefault(x => x.Value == value) != null;
        }

        public static int GetMaxValue()
        {
            return DAL.SqlRepository.Urgencies
                .Cast<Urgency>()
                .Max(x => x.Value);
        }

        public static int GetValueByName(string name)
        {
            var item = DAL.SqlRepository.Urgencies
                .Cast<Urgency>()
                .FirstOrDefault(x => x.UrgencyName == name);
            return item == null ? -1 : item.Value;
        }
    }
}
