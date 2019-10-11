using SW3Projekt.Pages;
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

namespace SW3Projekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AddTimesheet ATSWindow = new AddTimesheet();
        ViewHiredEmployees VHEWindow = new ViewHiredEmployees();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Do something
        }

        private void BtnClickCreateTimesheet(object sender, RoutedEventArgs e)
        {
            Main.Content = ATSWindow;
        }

        private void BtnClickViewHiredEmployees(object sender, RoutedEventArgs e)
        {
            Main.Content = VHEWindow;
        }
    }
}
