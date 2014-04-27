using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.DAL;
using PathToSuccess.Models;

namespace PathToSuccess.TaskTree
{
    public class TaskTree
    {
        Models.User user;
        MultichildTree<CompletableItem> tree;
        private DAL.SqlRepository repository;
        public TaskTree()
        {
            repository=new SqlRepository();
        }

        //...//
    }
}
