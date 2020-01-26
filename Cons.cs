using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calender
{
    class Cons
    {
        public static int buttonWidth = 75;
        public static int buttonHeight = 53;
        public static int buttonMargin = 6;

        public static int DayOfWeek = 7;
        public static int DayOfColumn = 6;

        public static List<string> ListStatus = new List<string>() { "DONE", "DOING", "COMING", "MISSED" };

        public static int notifyTime = 1;
        public static int notifyTimeOut = 10000;
    }

    public enum EPlanItem
    {
        DONE,
        DOING,
        COMING,
        MISSED
    }

}