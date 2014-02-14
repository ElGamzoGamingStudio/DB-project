using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.Models
{
    public interface ICompletable
    {
        CompletionCriteria completionCriteria;
        TimeRule timeRule;
        void Complete();
    }
}
