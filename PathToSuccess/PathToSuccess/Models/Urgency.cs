using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PathToSuccess.Models
{
    [Table("urgency", Schema="public")]
    public class Urgency
    {
        protected bool Equals(Urgency other)
        {
            return string.Equals(UrgencyName, other.UrgencyName) && Value == other.Value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UrgencyName != null ? UrgencyName.GetHashCode() : 0)*397) ^ Value;
            }
        }

        [Key]
        [Column("urgency_name")]
        public string UrgencyName { get; set; }

        [Column("value")]
        public int Value { get; set; }

        public Urgency() { }

        public static Urgency CreateUrgency(int value, string name)
        {
            var u = new Urgency {Value = value, UrgencyName = name};
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
            var urgencies = DAL.SqlRepository.Urgencies.Cast<Urgency>().ToList();
            return urgencies.FirstOrDefault(x => x.Value == urgencies.Min(y => y.Value)).UrgencyName;
        }

        public static Urgency GetLowestUrgency()
        {
            var urgencies = DAL.SqlRepository.Urgencies.Cast<Urgency>().ToList();
            return urgencies.FirstOrDefault(x => x.Value == urgencies.Min(y => y.Value));
        }

        public static Urgency GetHighestUrgency()
        {
            var urgencies = DAL.SqlRepository.Urgencies.Cast<Urgency>().ToList();
            return urgencies.FirstOrDefault(x => x.Value == urgencies.Max(y => y.Value));
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Urgency) obj);
        }
    }
}
