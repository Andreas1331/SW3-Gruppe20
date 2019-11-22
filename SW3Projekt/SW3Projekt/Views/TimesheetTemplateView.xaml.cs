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
        public TimesheetTemplateView()
        {
            InitializeComponent();
            //Console.WriteLine(Timesheet_EmployeeID.Text);

            // ConfirmNumberEvent doesn't happen when going back from confirmation, because an ID is set.
            if (!(Timesheet_EmployeeID.Text == "")) 
            {
                ConfirmNumberEvent();
            }
        }
        public void ConfirmNumberEvent() 
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                // If an employee with the ID entered is found, relevant proprties are modified to show the entry page.
                if (ctx.Employees.Where(emp => emp.Id.ToString() == Timesheet_EmployeeID.Text).Any())
                {
                    //BtnConfirmNumber.IsEnabled = false;
                    //BtnConfirmNumber.Visibility = Visibility.Hidden;
                    //PanelList.IsEnabled = true;
                    //PanelList.Visibility = Visibility.Visible;
                    //BtnBeregn.IsEnabled = true;
                    //BtnBeregn.Visibility = Visibility.Visible;
                    //Timesheet_EmployeeID.IsReadOnly = true;
                }
                // Otherwise focus moves to the IDTextBox for another try.
                else
                {
                    Timesheet_EmployeeID.Focus();
                }
            }
        }

        // This event fires when the ConfirmNumber button is clicked.
        private void ConfirmNumber(object sender, RoutedEventArgs e)
        {
            ConfirmNumberEvent();   
        }
    }
}
