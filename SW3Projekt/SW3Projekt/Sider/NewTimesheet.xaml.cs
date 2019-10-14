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

namespace SW3Projekt.Sider
{
    /// <summary>
    /// Interaction logic for NewTimesheet.xaml
    /// </summary>
    public partial class NewTimesheet : UserControl
    {
        public Dictionary<int, TimesheetEntryCtrl> monday = new Dictionary<int, TimesheetEntryCtrl>();
        int i;
        MainWindow mainwindow;
        public NewTimesheet(MainWindow window)
        {
            InitializeComponent();
            mainwindow = window;
        }

        private void AddTimeSheetEntry_button_click(object sender, RoutedEventArgs e)
        {
            TimesheetEntryCtrl NewEntry = new TimesheetEntryCtrl(i, this);
            this.MondayEntries.Children.Add(NewEntry);
            monday.Add(i, NewEntry);
            i++;
        }
        public void RemoveTimeSheetEntry(int id) {
            TimesheetEntryCtrl EntryToRemove = monday[id];
            this.MondayEntries.Children.Remove(EntryToRemove);
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

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            mainwindow.CloseProgram(sender,e);
        }
    }
}
