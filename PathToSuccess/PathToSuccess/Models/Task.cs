using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathToSuccess.Models
{
    public class Task : ICompletable//задача
    {
        public DateTime DeadLine;
        public int ID
        {
            private set;
            get;
        }

        public Importance Importance;
        public Urgency Urgency;
        CompletionCriteria completionCriteria;
        TimeRule timeRule;

        public void Complete()
        {
            throw new NotImplementedException();
        }
    }
}
