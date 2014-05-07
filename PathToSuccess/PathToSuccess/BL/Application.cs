using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    public static class Application
    {
        public static User CurrentUser { get; set; }

        public static Tree CurrentTree { get; set; }

        public static void SetUp()
        {
            DAL.SqlRepository.Initialize();
            Log.Initialize();
<<<<<<< HEAD
            //RawSqlPusher.Initialize();
=======
            RawSqlPusher.Initialize();
            //DbTest.Seed();
>>>>>>> 5b5cf5181ee645210e4870d16e85e26f8f95c5ee
        }
    }
}
