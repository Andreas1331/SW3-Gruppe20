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
    /// Interaction logic for NewTimesheet.xaml
    /// </summary>
    public partial class AgreementEntryView : UserControl
    {
        public AgreementEntryView()
        {
            InitializeComponent();
            RatePanel.Visibility = Visibility.Hidden;
        }

        private void ToggleRatePanel(object sender, RoutedEventArgs e) // Show/hide rate panel and change view model height
        {
            if (RatePanel.Visibility == Visibility.Visible)
            {
                //Hide
                RatePanel.Visibility = Visibility.Hidden;
                Height = 100;
            }
            else
            {
                //Show
                RatePanel.Visibility = Visibility.Visible;
                Height = 200;
            }
        }
    }
}
