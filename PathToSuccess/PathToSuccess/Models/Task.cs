using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public class Task : CompletableItem//задача
    {
        //private Task parentTask;
        public List<CompletableItem> RelatedSteps { get; private set; }


        public Task(CompletionCriteria criteria, DateTime startTime, DateTime deadLine, TimeSpan timeToDo, Importance i, Urgency u) : 
            base(criteria, startTime, deadLine, timeToDo, i, u)
        {
            RelatedSteps = new List<CompletableItem>();
        }

        public Task(CompletionCriteria criteria, DateTime startTime, DateTime deadLine, TimeSpan timeToDo, Importance i,
            Urgency u, List<CompletableItem> related) :
                base(criteria, startTime, deadLine, timeToDo, i, u)
        {
            RelatedSteps = related;
        }


        public void AddStep(CompletableItem step)
        {
            if (!CheckTypeValidation(step)) return; //type of adding doesnt match type of list elements

            RelatedSteps.Add(step);
            OnStepAdd();
        }

        public void RemoveStep(CompletableItem step)
        {
            RelatedSteps.Remove(step);
            OnStepRemove();
        }

        /// <summary>
        /// checks type to be identical as the type of the whole list
        /// </summary>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        private bool CheckTypeValidation(CompletableItem toCheck)
        {

            return !RelatedSteps.Any() || RelatedSteps[0].GetType() == toCheck.GetType();
        }

        public CompletableItemEvent OnStepAdd;
        public CompletableItemEvent OnStepRemove;
    }
}
