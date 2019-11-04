using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SW3Projekt.ViewModels
{
    public class VismaEntrySumViewModel : Screen
    {
        public BindableCollection<TextBox> Ids { get; set; } = new BindableCollection<TextBox>();
        public BindableCollection<TextBox> Sums { get; set; } = new BindableCollection<TextBox>();

        public Dictionary<int, float> SumEntries { get; set; }

        public VismaEntrySumViewModel (Dictionary<int, float> sumEntries)
        {
            SumEntries = sumEntries;

            Ids.Add(new TextBox
            {
                Text = "ID",
                FontWeight = FontWeights.Bold,
                Width = 60,
                HorizontalContentAlignment = HorizontalAlignment.Center
            });

            Sums.Add(new TextBox
            {
                Text = "Sum",
                FontWeight = FontWeights.Bold,
                Width = 60,
                HorizontalContentAlignment = HorizontalAlignment.Center
            });

            GenerateTextBoxes();
        }

        private void GenerateTextBoxes()
        {

            foreach (KeyValuePair<int, float> entry in SumEntries)
            {
                var idTextbox = new TextBox();
                var sumTextbox = new TextBox();

                idTextbox.Width = sumTextbox.Width = 60;
                idTextbox.HorizontalContentAlignment = sumTextbox.HorizontalContentAlignment = HorizontalAlignment.Center;
                

                idTextbox.Text = entry.Key.ToString();
                sumTextbox.Text = entry.Value.ToString();

                Ids.Add(idTextbox);
                Sums.Add(sumTextbox);
            }

        }


    }
}
