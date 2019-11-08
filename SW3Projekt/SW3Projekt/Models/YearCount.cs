using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class YearCount
    {
        public int WeekNumber { get; set; }
        public double IllnessTotal { get; set; }
        public double TotalHours { get; set; }
        public double Rate1 { get; set; }
        public double Rate2 { get; set; }
        public double Rate3 { get; set; }
        public double Rate4 { get; set; }
        public double Diet { get; set; }
        public double TaxFreeKM1 { get; set; }
        public double TaxFreeKM2 { get; set; }
        public double TaxableKM { get; set; }
        public double SalaryFolder { get; set; }
        public double PaidLeave { get; set; }
    }
}