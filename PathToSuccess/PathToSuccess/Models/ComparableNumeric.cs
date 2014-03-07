using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public struct Unit
    {
        public string Name;
        public int NumericValue;

        public static Unit NonExistingUnit = new Unit { Name = "empty", NumericValue = -1 };
    }
    public abstract class ComparableNumeric
    {
        public virtual void Initialize()
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
