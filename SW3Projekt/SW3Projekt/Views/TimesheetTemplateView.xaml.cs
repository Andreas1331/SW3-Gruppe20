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
        public TimesheetTemplateView()
        {
            InitializeComponent();
        }

        private void AddTimeSheetEntryMonday_button_click(object sender, RoutedEventArgs e)
        {
            //TimesheetEntryCtrl NewEntry = new TimesheetEntryCtrl(i, this);
            //this.MondayGrid.Children.Add(NewEntry);
            //monday.Add(i, NewEntry);
            //i++;
        }
        private void AddTimeSheetEntryTuesday_button_click(object sender, RoutedEventArgs e)
        {
            //TimesheetEntryCtrl NewEntry = new TimesheetEntryCtrl(i, this);
            //this.TuesdayEntries.Children.Add(NewEntry);
            //tuesday.Add(i, NewEntry);
            //i++;
        }

        public void RemoveTimeSheetEntry(int id)
        {
            //TimesheetEntryCtrl EntryToRemove = monday[id];
            //this.MondayGrid.Children.Remove(EntryToRemove);
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
