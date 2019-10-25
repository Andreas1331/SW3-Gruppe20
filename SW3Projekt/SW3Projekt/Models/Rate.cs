using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("Rates")]
    public class Rate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VismaID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double RateValue { get; set; }
        public int CollectiveAgreementID { get; set; }
        public Days DaysPeriod { get; set; }

        [Flags]
        public enum Days
        {
            Monday    = 1 << 1,
            Tuesday   = 1 << 2,
            Wednesday = 1 << 3,
            Thursday  = 1 << 4,
            Friday    = 1 << 5,
            Saturday  = 1 << 6,
            Sunday    = 1 << 7
        }

        public Rate()
        {
            Console.WriteLine("Val: " + gj(3));
        }

        public bool gj(int day)
        {
            DaysPeriod = (Days.Monday | Days.Tuesday | Days.Wednesday);
            Console.WriteLine("DA: " + (int)DaysPeriod);
            Console.WriteLine("day: " + (Days)Math.Pow(2, day));
            bool k = (DaysPeriod & (Days)Math.Pow(day,2)) > 0;
            return k;
        }
    }
}
