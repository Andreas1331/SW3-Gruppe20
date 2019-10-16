using SW3Projekt.Sider;
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
        List<String> StyleNames = new List<string>();
        int theme = 0; //0 = default 1 = light


        public MainWindow()
        {
            InitializeComponent();
            //loading in the two Themes
            StyleNames.Add("TextBlockLayout");
            StyleNames.Add("ComboBoxItemLayout");
            StyleNames.Add("ComboBoxValueBoxLayout");
            StyleNames.Add("TextBoxLayout");
            StyleNames.Add("ButtonLayout");
            StyleNames.Add("PageLayout");

        }

        private void NewTimeSheet(object sender, RoutedEventArgs e)
        {
            //Window timesheet = new Window();
            UserInterface.Content = new NewTimesheet(this);
            //this.Content = new NewTimesheet(this);
            //Do something
        }

        public void CloseProgram(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeProgram(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeProgram(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            //change to light theme
            if (theme == 0)
            {
                for (int i = 0; i < StyleNames.Count(); i++)
                {
                    Application.Current.Resources["Current" + StyleNames[i]] = Application.Current.Resources["Dark" + StyleNames[i]];
                }
                theme = 1;
            }
            //change to dark theme
            else if (theme == 1)
            {
                for (int i = 0; i < StyleNames.Count(); i++)
                {
                    Application.Current.Resources["Current" + StyleNames[i]] = Application.Current.Resources["Light" + StyleNames[i]];
                }
                theme = 0;
            }
        }
        private void MoveWindow(object sender, RoutedEventArgs e)
        {
            DragMove();
        }
    }
}