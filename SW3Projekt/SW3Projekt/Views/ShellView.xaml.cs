using Caliburn.Micro;
using SW3Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SW3Projekt.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        //EVENT FOR WHEN DRAGGING OR DOUBLECLICKING THE TOPBAR
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (App.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    App.Current.MainWindow.WindowState = WindowState.Normal;
                }
                else if (App.Current.MainWindow.WindowState == WindowState.Normal)
                {
                    App.Current.MainWindow.WindowState = WindowState.Maximized;
                }
            }
            else
            {
                App.Current.MainWindow.DragMove();
            }
        }

        //EVENT TO MINIMIZE THE WINDOW
        private void BtnMinimizeProgramTopBar_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        //EVENT TO MAXIMIZE THE WINDOW
        private void BtnMaximizeProgramTopBar_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                App.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else if (App.Current.MainWindow.WindowState == WindowState.Normal)
            {
                App.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        //EVENT TO EXIT THE PROGRAM
        private void BtnExitProgramTopBar_Click(object sender, RoutedEventArgs e)
        {
            string caption = "Vil du lukke programmet?";
            string message = "Alt ikke-gemt data vil gå tabt";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = System.Windows.Forms.MessageBox.Show(message, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        //EVENT FOR WHENEVER THE WINDOW IS RESIZED, WE DO THIS BECAUSE OTHERWISE THE WINDOW WILL GO OUTSIDE THE MONITOR
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new System.Windows.Thickness(6);
            }
            else
            {
                this.BorderThickness = new System.Windows.Thickness(0);
            }
        }

        private void BtnOpenOverviewInNewWindow_Click(object sender, RoutedEventArgs e)
        {
            IWindowManager manager = new WindowManager();
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            settings.ResizeMode = ResizeMode.CanResize;
            settings.AllowTransparency = true;
            settings.WindowStyle = WindowStyle.ToolWindow;
            settings.MinWidth = 800;
            settings.MinHeight = 450;
            settings.Title = "Sammentælling oversigt";

            manager.ShowWindow(new OverviewViewModel(), null, settings);
        }
    }
}
