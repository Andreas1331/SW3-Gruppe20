using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW3Projekt.Models;

namespace SW3Projekt.ViewModels
{

    public class ExportViewModel : Screen
    {
        //PROPERTIES
        //Inputs read from comboBoxed upon Export button click
        public int FromWeekValue { get; set; }
        public int FromYearValue { get; set; }
        public int ToWeekValue { get; set; }
        public int ToYearValue { get; set; }

        //Period. Week and year converted to DateTime used to compare with all the entries's datetimes
        private DateTime FromValue;
        private DateTime ToValue;

        //Entries
        private List<TimesheetEntry> TimesheetEntries { get; set; }

        //TimesheetEntries and VismaEntries converted
        private List<Combination> Combinations { get; set; } //List of timesheet entries with their derived visma entries
        private List<Row> Rows { get; set; } //Strings ready to be printed 

        //CONSTRUCTOR
        public ExportViewModel()
        {
            TimesheetEntries = new List<TimesheetEntry>();
            Combinations = new List<Combination>();
            Rows = new List<Row>();
        }

        //METHODS
        public void BtnExport()
        {
            FromValue = WeekNumToDateTime(FromWeekValue, FromYearValue, 0); //Find monday of the week
            ToValue = WeekNumToDateTime(ToWeekValue, ToYearValue, 6); //Find monday + whole week

            using (var ctx = new DatabaseDir.Database())
            {
                //Find the timesheet
                TimesheetEntries = ctx.TimesheetEntries.Where(x => x.Date > FromValue && x.Date < ToValue).ToList();

                //Assign each timesheetentry its own list containing it vismaentries
                foreach (TimesheetEntry Tse in TimesheetEntries)
                    Combinations.Add(new Combination(Tse, new List<VismaEntry>()));

                //Find visma entries for each timesheet entry
                foreach (Combination comp in Combinations)
                {
                    int TseID = comp.TimesheetEntry.Id;
                    List<VismaEntry> vismaEntries = new List<VismaEntry>();

                    //Compare Id. If true, add visma entry to the timesheetentry's visma entries
                    vismaEntries = ctx.VismaEntries.Where(x => x.TimesheetEntryID == comp.TimesheetEntry.Id).ToList(); 
                    comp.VismaEntries = vismaEntries;
                }
            }

            //Convert every visma entry to a row.
            foreach (Combination comb in Combinations) //Every timesheet entry
                foreach (VismaEntry ve in comb.VismaEntries) //Every timesheet's visma entry
                    Rows.Add(new Row(comb.TimesheetEntry, ve));

            //Send rows as strings to exporter
            foreach (Row row in Rows)
                Printer.Lines.Add(row.GetLine());

            //Finally Export
            Printer.Print(/*Name, Filepath TODO*/);
        }

        //Convert the entered weeknumber to a datetime for comparison
        public DateTime WeekNumToDateTime(int weekNum, int year, int DaysToAdd)
        {
            var cal = CultureInfo.CurrentCulture.Calendar;
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffsetThursday = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffsetThursday);
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            if (firstWeek == 1)
                weekNum -= 1;

            return firstThursday.AddDays((weekNum * 7) - 3 + DaysToAdd);
        }

        //CLASSES
        //Private helping class for ExportViewModel to group every timesheet entries with their derived vista entries
        private class Combination
        {
            //PROPERTIES
            public TimesheetEntry TimesheetEntry { get; set; }
            public List<VismaEntry> VismaEntries { get; set; }

            //CONSTRUCTOR
            public Combination(TimesheetEntry timesheetEntry, List<VismaEntry> vismaEntries)
            {
                TimesheetEntry = timesheetEntry;
                VismaEntries = vismaEntries;
            }
        }
    }
}
