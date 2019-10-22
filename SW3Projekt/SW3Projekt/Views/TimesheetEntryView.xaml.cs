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
    /// Interaction logic for TimesheetEntryView.xaml
    /// </summary>
    public partial class TimesheetEntryView : UserControl
    {
        TimesheetTemplateView TS;
        int ID;
        public TimesheetEntryView(int id, TimesheetTemplateView TS)
        {
            InitializeComponent();
            this.TS = TS;
            ID = id;
        }

        private void BtnRemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            TS.RemoveTimeSheetEntry(ID);
        }
    }
}
