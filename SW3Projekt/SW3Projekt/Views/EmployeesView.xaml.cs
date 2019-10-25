using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : UserControl
    {
        public EmployeesView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(((TextBox)sender).Text + e.Text);
        }

        public static bool IsTextNumeric(string str)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str))
                return false;

            Console.WriteLine("Not null or empty");
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(str);
        }
    }
}
