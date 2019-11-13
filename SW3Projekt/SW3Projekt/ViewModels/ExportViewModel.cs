using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW3Projekt.Models;
using System.Data.Entity;

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

        //Include these type to other csv. and exclude from first
        private readonly List<string> SelectedTypes = new List<string>() { "Sygdom", "FerieFri", "Logi" };

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
            List<TimesheetEntry> TimesheetEntries = new List<TimesheetEntry>();

            //Convert input to DateTime for comparison with Entry
            DateTime fromValue = WeekNumToDateTime(SelectedFromWeek, SelectedFromYear, 0);
            DateTime toValue = WeekNumToDateTime(SelectedToWeek, SelectedToYear, 6);

            //Rows for normal and sick
            List<Row> normalRows = new List<Row>();
            List<Row> sickRows = new List<Row>();

            //Find the timesheet entries and their vismaentries and their linked rates
            using (var ctx = new DatabaseDir.Database())
                TimesheetEntries = ctx.TimesheetEntries.Include(p => p.vismaEntries.Select(k => k.LinkedRate)).Where(ts => ts.Date >= fromValue && ts.Date <= toValue).ToList();

            //Split and convert every vismaentry to a row by type.
            foreach (TimesheetEntry tse in TimesheetEntries)
                foreach (VismaEntry ve in tse.vismaEntries)
                    if (SelectedTypes.Contains(ve.LinkedRate.Type))
                        sickRows.Add(new Row(tse, ve));
                    else
                        normalRows.Add(new Row(tse, ve));

            //Export and check for error
            if (Export(normalRows, FileName, FilePath) != 0 )
                System.Windows.Forms.MessageBox.Show("Fejl", "Fejl", System.Windows.Forms.MessageBoxButtons.OK);
            if (Export(sickRows, FileName + "Sick", FilePath) != 0)
                System.Windows.Forms.MessageBox.Show("Fejl", "Fejl", System.Windows.Forms.MessageBoxButtons.OK);
            else
                System.Windows.Forms.MessageBox.Show($"{FileName}.csv er udskrevet til {FilePath}", "Success", System.Windows.Forms.MessageBoxButtons.OK);
        }

        //Convert the entered weeknumber to a datetime for comparison
        private DateTime WeekNumToDateTime(int weekNum, int year, int DaysToAdd)
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

        private int Export(List<Row> rows, string name, string path)
        {
            //Send rows as strings to exporter
            foreach (Row row in rows)
                Printer.Lines.Add(row.GetLine());

            //Finally Export
            return Printer.Print(name, path); 
        }
    }
}
