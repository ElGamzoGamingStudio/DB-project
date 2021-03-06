﻿using System;
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
            ChangesBuffer.Initialize();
            Log.Initialize();
            RawSqlPusher.Initialize();
            PhoneSync.InfoSender.Initialize();
            //ScheduleManager.Initialize();
            //DbTest.Try();
            //DbTest.Seed();

        }

        public static int Hash(string toHash)
        {
            int result = 0;
            foreach (char c in toHash)
            {
                result = ((result*1664525) + (int) c + 1013904223)%int.MaxValue;
            }
            return Math.Abs(result);
        }
    }
}
