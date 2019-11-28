using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("Rates")]
    public class Rate : IValidate
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int VismaID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double RateValue { get; set; }
        public int CollectiveAgreementID { get; set; }
        public Days DaysPeriod { get; set; }
        public string Type { get; set; }
        public bool SaveAsMoney { get; set; }

        public bool CheckFlag(Days days)
        {
            return (DaysPeriod & days) == days;
        }

        public bool IsValidate()
        {
            if (Name == string.Empty || Name == null || VismaID < 0 || RateValue < 0 || StartTime == null || EndTime == null || Type == string.Empty || Type == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    [Flags]
    public enum Days
    {
        Sunday = 1 << 0,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6
    }
}
