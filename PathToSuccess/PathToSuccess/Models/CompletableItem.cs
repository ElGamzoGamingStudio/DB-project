using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;
using PathToSuccess.DAL;
using System.Reflection;

namespace PathToSuccess.Models
{
    public abstract class CompletableItem
    {
        CompletionCriteria completionCriteria;
        public string timeRule;
        public DateTime StartTime;
        public DateTime DeadLine;
        public TimeSpan TimeToDo;
        public readonly int ID;
       
        private Importance _importance;
        public Importance Importance
        {
            get { return _importance; }
            set
            {
                _importance = value;
                OnImportanceChanged();
            }
        }

        private Urgency _urgency;
        public Urgency Urgency
        {
            get { return _urgency; }
            set
            {
                _urgency = value;
                OnUrgencyChanged();
            }
        }

        public delegate void CompletableItemEvent();
        public event CompletableItemEvent OnCompleted;
        public event CompletableItemEvent OnImportanceChanged;
        public event CompletableItemEvent OnUrgencyChanged;

        public CompletableItem(CompletionCriteria criteria, DateTime startTime, DateTime deadLine, TimeSpan timeToDo, Importance i, Urgency u)
        {
            ID = ModuleConnector.Repository.GetNextID();
            completionCriteria = criteria;
            StartTime = startTime;
            DeadLine = deadLine;
            TimeToDo = timeToDo;
            Importance = i;
            Urgency = u;
        }

        public virtual void Complete()
        {
            if (completionCriteria.Completed == true) return;
            completionCriteria.Completed = true;
            OnCompleted();
        }

        public virtual Type GetTimeRule()
        {
            return Type.GetType(timeRule, false, false);
        }

        public virtual void UpdateUrgency()
        {
            // ... //
        }


    }
}
