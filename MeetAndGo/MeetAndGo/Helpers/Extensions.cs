using System;
using System.Collections.Generic;
using System.Text;

namespace MeetAndGo.Helpers
{
    public static class Extensions
    {
        public static string DoubleToString(this double value) {
            return value.ToString().Replace(",", ".");
        }
    }
}
