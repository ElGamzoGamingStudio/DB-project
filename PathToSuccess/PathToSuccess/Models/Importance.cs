using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.DAL;

namespace PathToSuccess.Models
{
    public class Importance : ComparableNumeric
    {
        private static List<Unit> _availableUnits;
        public static List<Unit> AvailableUnits
        {
            get
            {
                return _availableUnits;
            }
            private set
            {
                _availableUnits = value;
            }
        }

        public Importance(string unitName)
            : base()
        {
            var unit = _availableUnits.FirstOrDefault(x => x.name == unitName);
            if (!unit.Equals(default(Unit)))
                Value = unit;
            else
                Value = Unit.NonExistingUnit;
        }

        public static override void Initialize()
        {
            Importance.AvailableUnits = SqlRepository.GetImportanceUnits();
        }
    }
}
