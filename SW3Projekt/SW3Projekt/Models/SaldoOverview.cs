using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class SaldoOverview : Screen
    {
        public int EmployeeId {get; set;}
        public string EmployeeName {get; set;}
        //public string Comment {get; set;}
        public double PaidLeave {get; set;}
        public double HolidayFree {get; set;}
        public double Holiday {get; set;}
        public double Illness {get; set;}
        public double WorkHours {get; set;}
        public string EmployeePhonenumber {get; set;}
        public bool IsEmployeeFired { get; set; }
        public double PercentIllness {get; set;}
    }
}
