using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.DAL
{
    public class SqlRepository : IRepository
    {
        public int GetNextID()
        {
            return 0;
        }
        public List<Unit> GetImportanceUnits()
        {
            return null;
        }
        public List<Unit> GetUrgencyUnits()
        {
            return null;
        }
        
    }
}
