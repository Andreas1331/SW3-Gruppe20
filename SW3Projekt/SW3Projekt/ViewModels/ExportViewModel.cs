using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                //find visma entries
                foreach (Combination comp in Combinations)
                {
                    int TseID = comp.TimesheetEntry.Id;
                    List<VismaEntry> vismaEntries = new List<VismaEntry>();
                    // = ctx.VismaEntry.Where(x => x.TimesheetEntryID).ToList(); TOOOODOOOOO
                    comp.VismaEntries = vismaEntries;
                }
            }

            //Combine/Map
            foreach (Combination comb in Combinations)
            {
                foreach (VismaEntry ve in comb.VismaEntries)
                {
                    Row row = Map(comb.TimesheetEntry, ve);
                    Rows.Add(row);
                }
            }

            //Send to print
            foreach (Row row in Rows)
                Printer.AddCalculationToList(row);

            Printer.Print(/*Name, Filepath*/);
        }

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

        public Row Map(TimesheetEntry tse, VismaEntry ve)
        {
            Row row = new Row();
            return row;
        }
    }
}

public class Combination
{
    //PROPERTIES
    public TimesheetEntry TimesheetEntry { get; set; }
    public List<VismaEntry> VismaEntries { get; set; } //List of derived vismaEntries from TimesheetEntry

    //CONSTRUCTOR
    public Combination(TimesheetEntry timesheetEntry, List<VismaEntry> vismaEntries)
    {
        TimesheetEntry = timesheetEntry;
        VismaEntries = vismaEntries;
    }
}
