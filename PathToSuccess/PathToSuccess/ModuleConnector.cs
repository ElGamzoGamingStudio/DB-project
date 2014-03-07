using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.DAL;

namespace PathToSuccess
{
    public static class ModuleConnector//Через этот класс формы будут общаться с логикой.
    {
        public static IRepository Repository;
        static ModuleConnector()
        {
            Repository=new SqlRepository();
        }
    }

}
