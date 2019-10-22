using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SW3Projekt.Views
{
    /// <summary>
    /// Interaction logic for TimesheetTemplateView.xaml
    /// </summary>
    public partial class TimesheetTemplateView : UserControl
    {
        Dictionary<int, TimesheetEntryView> mondayEntries = new Dictionary<int, TimesheetEntryView>();
        Dictionary<int, TimesheetEntryView> tuesdayEntries = new Dictionary<int, TimesheetEntryView>();
        Dictionary<int, TimesheetEntryView> wednesdayEntries = new Dictionary<int, TimesheetEntryView>();
        Dictionary<int, TimesheetEntryView> thursdayEntries = new Dictionary<int, TimesheetEntryView>();
        Dictionary<int, TimesheetEntryView> fridayEntries = new Dictionary<int, TimesheetEntryView>();
        Dictionary<int, TimesheetEntryView> saturdayEntries = new Dictionary<int, TimesheetEntryView>();
        Dictionary<int, TimesheetEntryView> sundayEntries = new Dictionary<int, TimesheetEntryView>();

        List<Rate> rates = new List<Rate>();

        int i = 0;

        public TimesheetTemplateView()
        {
            InitializeComponent();
        }



        private void AddTimeSheetEntry_button_click(object sender, RoutedEventArgs e)
        {
            TimesheetEntryView NewEntry = new TimesheetEntryView(i, this);
            Button button = (Button)sender;
            if (button.Name == "BtnMondayAddEntry")
            {
                MondayGrid.Children.Add(NewEntry);
                mondayEntries.Add(i, NewEntry);
            }
            else if (button.Name == "BtnTuesdayAddEntry")
            {
                TuesdayGrid.Children.Add(NewEntry);
                tuesdayEntries.Add(i, NewEntry);
            }
            else if (button.Name == "BtnWednesdayAddEntry")
            {
                WednesdayGrid.Children.Add(NewEntry);
                wednesdayEntries.Add(i, NewEntry);
            }
            else if (button.Name == "BtnThursdayAddEntry")
            {
                ThursdayGrid.Children.Add(NewEntry);
                thursdayEntries.Add(i, NewEntry);
            }
            else if (button.Name == "BtnFridayAddEntry")
            {
                FridayGrid.Children.Add(NewEntry);
                fridayEntries.Add(i, NewEntry);
            }
            else if (button.Name == "BtnSaturdayAddEntry")
            {
                SaturdayGrid.Children.Add(NewEntry);
                saturdayEntries.Add(i, NewEntry);
            }
            else if (button.Name == "BtnSundayAddEntry")
            {
                SundayGrid.Children.Add(NewEntry);
                sundayEntries.Add(i, NewEntry);
            }
            i++;
        }


        public void RemoveTimeSheetEntry(int id)
        {
            if (mondayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = mondayEntries[id];
                this.MondayGrid.Children.Remove(EntryToRemove);
            }
            else if (tuesdayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = tuesdayEntries[id];
                this.TuesdayGrid.Children.Remove(EntryToRemove);
            }
            else if (wednesdayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = wednesdayEntries[id];
                this.WednesdayGrid.Children.Remove(EntryToRemove);
            }
            else if (thursdayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = thursdayEntries[id];
                this.ThursdayGrid.Children.Remove(EntryToRemove);
            }
            else if (fridayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = fridayEntries[id];
                this.FridayGrid.Children.Remove(EntryToRemove);
            }
            else if (saturdayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = saturdayEntries[id];
                this.SaturdayGrid.Children.Remove(EntryToRemove);
            }
            else if (sundayEntries.ContainsKey(id))
            {
                TimesheetEntryView EntryToRemove = sundayEntries[id];
                this.SundayGrid.Children.Remove(EntryToRemove);
            }

        }

        public List<Rate> GetRates() {
            List<Rate> returnList = new List<Rate>();
            
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                
                var activeAgreement = ctx.CollectiveAgreements.FirstOrDefault(agreement => agreement.IsActive);

                returnList = ctx.Rates.Where(rate => rate.CollectiveAgreementID == activeAgreementAgreement.ID).ToList();
            }
            return returnList;

        }










        private void WeekBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (WeekBox.Text != "")
                WeekLabel.Visibility = Visibility.Hidden;
            else WeekLabel.Visibility = Visibility.Visible;
        }

        private void YearBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (YearBox.Text != "")
                YearLabel.Visibility = Visibility.Hidden;
            else YearLabel.Visibility = Visibility.Visible;
        }

        private void SalaryIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (SalaryIDBox.Text != "")
                SalaryLabel.Visibility = Visibility.Hidden;
            else SalaryLabel.Visibility = Visibility.Visible;
        }

    }
}
