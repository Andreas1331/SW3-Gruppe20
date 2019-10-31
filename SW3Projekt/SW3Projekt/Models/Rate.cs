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
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public double RateValue { get; set; }
        public int CollectiveAgreementID { get; set; }
        public Days DaysPeriod { get; set; }

        public bool CheckFlag(Days days)
        {
            return (DaysPeriod & days) == days;
        }
    }

    [Flags]
    public enum Days
    {
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6,
        Sunday = 1 << 7
    }
}
