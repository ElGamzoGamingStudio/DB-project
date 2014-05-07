using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathToSuccess.Models;

namespace PathToSuccess.BL
{
    public static class DbTest
    {
        private static User[] users;
        private static Urgency[] urgencies;
        private static Importance[] importancies;
        private static Interval[] intervals;
        private static Criteria[] criterias;
        private static Models.Schedule[] schedules;
        private static TimeRule[] timeRules;
        private static TimeBinding[] timeBindings;
        private static Models.Task[] tasks;
        private static Step[] steps;
        private static Tree[] trees;

        //PLEASE DO NOT CALL THIS METHOD IT IS TOTALLY BAD.
        public static void Seed()
        {
        }
    }
}
