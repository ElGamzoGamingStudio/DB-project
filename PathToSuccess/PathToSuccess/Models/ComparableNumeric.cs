using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public struct Unit
    {
        string name;
        int numericValue;

        public static Unit NonExistingUnit = new Unit { name = "empty", numericValue = -1 };
    }
    public abstract class ComparableNumeric
    {
        public static virtual void Initialize()
        {
            //...//
        }

        public Unit Value { get; protected set; }
        public ComparableNumeric()
        {
           //...//
        }

    }
}
