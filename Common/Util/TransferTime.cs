using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public static class TransferTime
    {
        public static string ValueToTime(int value)
        {
            int hour = value / 60;
            int minute = value % 60;    
            return hour + ":" + minute;
        }
        public static int TimeToValue(string time)
        {
            string[] strings = time.Split(':');
            int hour = 0;
            int minute = 0;
            hour = Convert.ToInt32(strings[0]);
            minute = Convert.ToInt32(strings[1]);
            return hour * 60 + minute;

        }
    }
}
