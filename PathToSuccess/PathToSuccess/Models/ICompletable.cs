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
        CompletionCriteria completionCriteria;//musthave
        TimeRule timeRule;//to be able to extend an array of tasks e.g. extend english classes for another month and so on. just a link to a static class, null by default;
        void Complete();
    }
}
