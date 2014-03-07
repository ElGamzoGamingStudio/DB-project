using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.DAL
{
    public interface IRepository
    {
        int GetNextID();
        List<Unit> GetImportanceUnits();
        List<Unit> GetUrgencyUnits();
    }
}
