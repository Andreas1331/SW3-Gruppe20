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
        }
        public void ConfirmNumberEvent() 
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                // If an employee with the ID entered isn't found, the focus moves to the IDTextBox for another try.
                if (!ctx.Employees.Where(emp => emp.Id.ToString() == Timesheet_EmployeeID.Text).Any())
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
