using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetConfirmationViewModel : Conductor<object>
    {


        public TimesheetTemplateViewModel Timesheet { get; set; }



        public BindableCollection<TimesheetEntryConfirmationViewModel> MondayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> TuesdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> WednesdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> ThursdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> FridayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> SaturdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> SundayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public List<BindableCollection<TimesheetEntryConfirmationViewModel>> WeekEntries { get; set; } = new List<BindableCollection<TimesheetEntryConfirmationViewModel>>();
        public string WeekBox { get; set; }
        public string YearBox { get; set; }
        public string SalaryIDBox { get; set; }


        public TimesheetConfirmationViewModel(TimesheetTemplateViewModel timesheet)
        {
            Timesheet = timesheet;

            foreach (TimesheetEntryViewModel entry in Timesheet.MondayEntries) 
            {
                MondayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.TuesdayEntries) 
            {
                TuesdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.WednesdayEntries) 
            {
                WednesdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.ThursdayEntries) 
            {
                ThursdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }

            foreach (TimesheetEntryViewModel entry in Timesheet.FridayEntries)
            {
                FridayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }

            foreach (TimesheetEntryViewModel entry in Timesheet.SaturdayEntries) 
            {
                SaturdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }
            
            foreach (TimesheetEntryViewModel entry in Timesheet.SundayEntries) 
            {
                SundayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }

            WeekBox = timesheet.Timesheet.WeekNumber.ToString();
            YearBox = timesheet.Timesheet.Year.ToString();
            SalaryIDBox = timesheet.Timesheet.EmployeeID.ToString();
        }

        public void BtnBack ()
        {
            WeekEntries.Add(MondayEntries);
            WeekEntries.Add(TuesdayEntries);
            WeekEntries.Add(WednesdayEntries);
            WeekEntries.Add(ThursdayEntries);
            WeekEntries.Add(FridayEntries);
            WeekEntries.Add(SaturdayEntries);
            WeekEntries.Add(SundayEntries);
            removeTimesheetEntriesToList();
            Timesheet.ShellViewModel.ActivateItem(Timesheet);
        }

        public void removeTimesheetEntriesToList()
        {
            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    tsentry.TimesheetEntry.vismaEntries.Clear();
                }
            }
        }
    }
}
