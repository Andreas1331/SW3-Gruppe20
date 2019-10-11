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
    /// Interaction logic for TimesheetEntryCtrl.xaml
    /// </summary>
    public partial class TimesheetEntryCtrl : UserControl
    {
        public int ID;
        public NewTimesheet newTimesheet;
        public TimesheetEntryCtrl(int id, NewTimesheet TS)
        {
            InitializeComponent();
            ID = id;
            newTimesheet = TS;
        }

        private void RemoveEntry_Button_Click(object sender, RoutedEventArgs e)
        {
            newTimesheet.RemoveTimeSheetEntry(ID);
        }
    }
}
