using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public class Step : CompletableItem
    {
        /// <summary>
        /// the lowest class of an objective
        /// </summary>

        #region Fields
        private CompletableItem _parent;
        
        public CompletableItem Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                OnParentChanged();
            }
        }

        #endregion

        #region Constructurs

        public Step(CompletionCriteria criteria, DateTime startTime, DateTime deadLine, TimeSpan timeToDo, Importance i, Urgency u, CompletableItem parent) 
            : base(criteria, startTime, deadLine, timeToDo, i, u)
        {
            _parent = parent;
        }

        #endregion

        #region Methods
        
        #endregion

        #region Events

        public CompletableItemEvent OnParentChanged;

        #endregion
    }
}
