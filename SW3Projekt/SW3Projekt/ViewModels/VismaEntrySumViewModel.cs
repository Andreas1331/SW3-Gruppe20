using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace SW3Projekt.ViewModels
{
    public class VismaEntrySumViewModel : Screen
    {
        public BindableCollection<TextBox> Ids { get; set; } = new BindableCollection<TextBox>();
        public BindableCollection<TextBox> Sums { get; set; } = new BindableCollection<TextBox>();

        public Dictionary<int, double> SumEntries { get; set; }

        public VismaEntrySumViewModel (Dictionary<int, double> sumEntries)
        {
            SumEntries = sumEntries;

            // Starts by creating two standard textboxes which contains the "ID" text and the "Sum" text at the start of every row of sums.
            Ids.Add(new TextBox
            {
                Text = "Visma ID",
                FontWeight = FontWeights.Bold,
                Width = 60,
                Background = Brushes.WhiteSmoke,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsTabStop = false
            });

            Sums.Add(new TextBox
            {
                Text = "Sum",
                FontWeight = FontWeights.Bold,
                Width = 60,
                Background = Brushes.WhiteSmoke,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsTabStop = false
            });

            // Then it generates the textboxes containing the sum and id pairs based on the dictionary that was sent.
            GenerateTextBoxes();
        }

        private void GenerateTextBoxes()
        {
            foreach (KeyValuePair<int, double> entry in SumEntries)
            {
                #region Create the textbox standard
                var idTextbox = new TextBox();
                var sumTextbox = new TextBox();
                idTextbox.Width = sumTextbox.Width = 60;
                idTextbox.HorizontalContentAlignment = sumTextbox.HorizontalContentAlignment = HorizontalAlignment.Center;

                idTextbox.Background = Brushes.WhiteSmoke;
                sumTextbox.Background = Brushes.WhiteSmoke;
                #endregion

                // Adds the text to the textboxes.
                idTextbox.Text = entry.Key.ToString();
                sumTextbox.Text = entry.Value.ToString();

                // Makes them not table.
                idTextbox.IsTabStop = false;
                sumTextbox.IsTabStop = false;

                // Adds them to the collections.
                Ids.Add(idTextbox);
                Sums.Add(sumTextbox);
            }
        }
    }
}
