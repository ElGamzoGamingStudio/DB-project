using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public class Step : ICompletable
    {
        #region Fields
        public bool Completed { get; private set; }
        #endregion

        #region Methods
        public void Complete()
        {
            Completed = false;
        }
        #endregion
    }
}
