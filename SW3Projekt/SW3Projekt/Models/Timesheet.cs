using SW3Projekt.Tools;
using SW3Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SW3Projekt.Models.Rate;

namespace SW3Projekt.Models
{
    public class Timesheet
    {
        public int DatabaseID { get; set; }
        public int EmployeeID { get; set; }
        public int WeekNumber { get; set; }
        public int Year { get; set; } = DateTime.Now.Year;

        public List<TimesheetEntry> TSEntries = new List<TimesheetEntry>();

        public List<Rate> rates = Calculator.GetRates();


        public Timesheet()
        {
            var cal = new GregorianCalendar();
            WeekNumber = cal.GetWeekOfYear(DateTime.Now.AddDays(-14), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

        }


    }
}
