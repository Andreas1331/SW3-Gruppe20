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
        //List<Rate> rates = new List<Rate>();


        public TimesheetTemplateView()
        {
            InitializeComponent();
        }



        private void WeekBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        //    if (WeekBox.Text != "")
        //        WeekLabel.Visibility = Visibility.Hidden;
        //    else WeekLabel.Visibility = Visibility.Visible;
        }

        private void YearBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        //    if (YearBox.Text != "")
        //        YearLabel.Visibility = Visibility.Hidden;
        //    else YearLabel.Visibility = Visibility.Visible;
        }

        private void SalaryIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        //    if (SalaryIDBox.Text != "")
        //        SalaryLabel.Visibility = Visibility.Hidden;
        //    else SalaryLabel.Visibility = Visibility.Visible;
        }

    }
}
