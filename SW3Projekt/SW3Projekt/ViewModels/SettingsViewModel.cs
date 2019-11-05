using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SW3Projekt.ViewModels
{
    public class SettingsViewModel : Screen
    {
        public void BtnChangeBgColor()
        {
            SolidColorBrush slb = new SolidColorBrush();
            slb = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffaacc"));

            Application.Current.Resources["ShellGrey"] = slb;
        }

    }
}
