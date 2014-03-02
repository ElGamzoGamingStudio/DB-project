using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public static class TaskInitializer
    {
        public static Task CreateTask(CompletionCriteria criteria, DateTime startTime, DateTime deadLine, TimeSpan timeToDo, Importance i, Urgency u)
        {
            return new Task(criteria, startTime, deadLine, timeToDo, i, u);
        }

        /// <summary>
        /// realted completableItem list is used as a reference!
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="startTime"></param>
        /// <param name="deadLine"></param>
        /// <param name="timeToDo"></param>
        /// <param name="i"></param>
        /// <param name="u"></param>
        /// <param name="related">reference, not a copy</param>
        /// <returns></returns>
        public static Task CreateTask(CompletionCriteria criteria, DateTime startTime, DateTime deadLine,
            TimeSpan timeToDo, Importance i, Urgency u, List<CompletableItem> related)
        {
            return new Task(criteria, startTime, deadLine, timeToDo, i, u, related);
        }
    }
}
