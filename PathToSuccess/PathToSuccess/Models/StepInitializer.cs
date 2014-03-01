using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public static class StepInitializer
    {
        public static Step CreateStep(CompletionCriteria criteria, DateTime startTime, DateTime deadLine, 
            TimeSpan timeToDo, Importance i, Urgency u, CompletableItem parent)
        {
            return new Step(criteria, startTime, deadLine, timeToDo, i, u, parent);
        }

        public static Step CreateStep(CompletionCriteria criteria, DateTime startTime, DateTime deadLine,
            TimeSpan timeToDo, Importance i, Urgency u)
        {
            return new Step(criteria, startTime, deadLine, timeToDo, i, u, null);
        }
    }
}
