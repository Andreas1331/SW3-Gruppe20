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
    }

    [Flags]
    public enum Days
    {
        Monday = 1 << 0,
        Tuesday = 1 << 1,
        Wednesday = 1 << 2,
        Thursday = 1 << 3,
        Friday = 1 << 4,
        Saturday = 1 << 5,
        Sunday = 1 << 6
    }

}
