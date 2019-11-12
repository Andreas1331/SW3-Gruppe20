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
        //Combo boxes
        public BindableCollection<int> FromWeek { get; set; } = new BindableCollection<int>();
        public BindableCollection<int> FromYear { get; set; } = new BindableCollection<int>();
        public BindableCollection<int> ToWeek { get; set; } = new BindableCollection<int>();
        public BindableCollection<int> ToYear { get; set; } = new BindableCollection<int>();

        public int SelectedFromWeek { get; set; }
        public int SelectedFromYear { get; set; }
        public int SelectedToWeek { get; set; }
        public int SelectedToYear { get; set; }

        //File data
        public string FileName { get; set; }
        public string FilePath { get; set; }

        //Convert week and year to a DateTime
        private DateTime FromValue { get; set; }
        private DateTime ToValue { get; set; }

        //Entries
        private List<TimesheetEntry> TimesheetEntries { get; set; } = new List<TimesheetEntry>();

        //TimesheetEntries and VismaEntries converted
        private List<Combination> Combinations { get; set; } = new List<Combination>();
        private List<Row> Rows { get; set; } = new List<Row>();

        //CONSTRUCTOR
        public ExportViewModel()
        {
            //Initialize combo boxes
            int MinYear = 2018, MaxYear = 2020;

            for (int i = 1; i <= 53; i++) FromWeek.Add(i);
            for (int i = MinYear; i < MaxYear; i++) FromYear.Add(i);

            for (int i = 1; i <= 53; i++) ToWeek.Add(i);
            for (int i = MinYear; i < MaxYear; i++) ToYear.Add(i);

            //Initalize default values 


            SelectedFromWeek = 40;
            SelectedToWeek = 50;

            SelectedFromYear = 2019;
            SelectedToYear = 2019;

            //Get outputlocation
            FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Get Filename
            DateTime d = DateTime.Now;
            FileName = $"{d.Day}{d.Month}{d.Year}";
        }


        //METHODS
        //Select path from folder explorer
        public void BtnGetFilePath()
        {
            //Open folder browser
            var folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            folderBrowser.ShowDialog();

            //If user cancelled, resulting in whitespace
            if (string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                return;

            //Else update value
            FilePath = folderBrowser.SelectedPath;
            NotifyOfPropertyChange(() => FilePath);
        }

        public void BtnExport()
        {
            //Convert input to DateTime for comparison with Entry
            FromValue = WeekNumToDateTime(SelectedFromWeek, SelectedFromYear, 0);
            ToValue = WeekNumToDateTime(SelectedToWeek, SelectedToYear, 6);

            using (var ctx = new DatabaseDir.Database())
            {
                //Find the timesheet entries
                TimesheetEntries = ctx.TimesheetEntries.Where(x => x.Date >= FromValue && x.Date <= ToValue).ToList();

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
            if(Printer.Print(FileName, FilePath) == -1)
                System.Windows.Forms.MessageBox.Show("Vælg et nyt fil navn", "Fejl", System.Windows.Forms.MessageBoxButtons.OK);
            //Ny fil placering kommer
            else
            System.Windows.Forms.MessageBox.Show($"{FileName}.csv er udskrevet til {FilePath}", "Success", System.Windows.Forms.MessageBoxButtons.OK);
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
