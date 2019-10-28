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



        private void WeekBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Timesheet_WeekNumber.Text == "" || Timesheet_WeekNumber.Text == "0") 
            { 
                Timesheet_WeekNumber.Text = "";
                WeekLabel.Visibility = Visibility.Visible;
            }
            else 
                WeekLabel.Visibility = Visibility.Hidden;
        }

        private void YearBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Timesheet_Year.Text == "" || Timesheet_Year.Text == "0") { 
                YearLabel.Visibility = Visibility.Visible;
                Timesheet_Year.Text = "";
            }
            else 
                YearLabel.Visibility = Visibility.Hidden;
        }

        private void SalaryIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Timesheet_EmployeeID.Text == ""|| Timesheet_EmployeeID.Text == "0")
            {
                SalaryLabel.Visibility = Visibility.Visible;
                Timesheet_EmployeeID.Text = "";
            }
            else 
                SalaryLabel.Visibility = Visibility.Hidden;
        }

    }
}
