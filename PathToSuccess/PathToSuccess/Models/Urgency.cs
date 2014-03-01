using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.DAL;

namespace PathToSuccess.Models
{
    public class Urgency : ComparableNumeric
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

        public Urgency(Unit unit)
            : base()
        {
            Value =
                _availableUnits.Contains(unit) ?
                unit : Unit.NonExistingUnit;
        }

        public static override void Initialize()
        {
            Urgency.AvailableUnits = SqlRepository.GetUrgencyUnits();
        }
    }
}
