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
        public double PaidLeave {get; set;}
        public double HolidayFree {get; set;}
        public double Holiday {get; set;}
        public double Illness {get; set;}
        public double WorkHours {get; set;}
        public string EmployeePhonenumber {get; set;}
        public bool IsEmployeeFired { get; set; }
        public double PercentIllness {get; set;}

        public string TranslateProperties(string propToTranslate)
        {
            switch (propToTranslate)
            {
                case "EmployeeId":
                    return "ID";
                case "EmployeeName":
                    return "Navn";
                case "PaidLeave":
                    return "Afs.";
                case "HolidayFree":
                    return "FerieFri";
                case "Holiday":
                    return "Ferie";
                case "Illness":
                    return "Syg";
                case "WorkHours":
                    return "Arb. timer";
                case "EmployeePhonenumber":
                    return "Telefon";
                case "IsEmployeeFired":
                    return "Aftrådt";
                case "PercentIllness":
                    return "Syg i alt";
                default:
                    return "Property not found!";
            }
        }
    }
}
