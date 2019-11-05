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
            Console.WriteLine(Timesheet_EmployeeID.Text);
            if (!(Timesheet_EmployeeID.Text == "")) {
                ConfirmNumberEvent();
            }
        }
        private void ConfirmNumberEvent() {
            BtnConfirmNumber.IsEnabled = false;
            BtnConfirmNumber.Visibility = Visibility.Hidden;
            PanelList.IsEnabled = true;
            PanelList.Visibility = Visibility.Visible;
            BtnBeregn.IsEnabled = true;
            BtnBeregn.Visibility = Visibility.Visible;
            Timesheet_EmployeeID.IsReadOnly = true;
        }

        private void ConfirmNumber(object sender, RoutedEventArgs e)
        {
            ConfirmNumberEvent();   
        }
    }
}
