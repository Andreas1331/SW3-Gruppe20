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
    /// Interaction logic for VismaEntryView.xaml
    /// </summary>
    public partial class VismaEntryView : UserControl
    {
        public VismaEntryView()
        {
            InitializeComponent();
        }
        public void OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selecteditem = e.Source as ComboBoxItem;
            Console.WriteLine(selecteditem.Content);


            //Entry.VismaID = TimesheetEntry.tsentry.timesheet.rates
            //                .Where(rate => (string)rate.Name == (string)selecteditem.Content)
            //                .Select(rate => rate.VismaID).FirstOrDefault();
        }
    }
}
