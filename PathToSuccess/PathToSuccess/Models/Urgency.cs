﻿using System;
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

        public Urgency(string unitName)
            : base()
        {
            var unit = _availableUnits.FirstOrDefault(x => x.Name == unitName);
            Value = !unit.Equals(default(Unit)) ? unit : Unit.NonExistingUnit;
        }

        public override void Initialize()
        {
            Urgency.AvailableUnits = ModuleConnector.Repository.GetUrgencyUnits();
        }
    }
}
