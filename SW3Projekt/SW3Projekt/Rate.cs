using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt
{
    class Rate
    {
        public int DatabaseID { get; set; }
        public int RateID { get; set; }
        public string Name { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public double Wage { get; set; }
        public Days DaysPeriod { get; set; }

        [Flags]
        public enum Days
        {
            Monday    = 1 << 0,
            Tuesday   = 1 << 1,
            Wednesday = 1 << 2,
            Thursday  = 1 << 3,
            Friday    = 1 << 4,
            Saturday  = 1 << 5,
            Sunday    = 1 << 6
        }
    }
}
