using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.IO;
using SW3Projekt.Models;
using SW3Projekt.Tools;

namespace SW3Projekt.ViewModels
{
    public class SettingsViewModel : Caliburn.Micro.Screen
    {
        private string _CSVWorkBox;
        public string CSVWorkBox
        {
            get
            {
                return _CSVWorkBox;
            }
            set
            {
                _CSVWorkBox = value;
                NotifyOfPropertyChange(() => CSVWorkBox);
            }
        }
        private string _CSVSickBox;
        public string CSVSickBox
        {
            get
            {
                return _CSVSickBox;
            }
            set
            {
                _CSVSickBox = value;
                NotifyOfPropertyChange(() => CSVSickBox);
            }
        }
        private int _twentyThousindDayBox;
        public int TwentyThousindDayBox
        {
            get
            {
                return _twentyThousindDayBox;
            }
            set
            {
                _twentyThousindDayBox = value;
                NotifyOfPropertyChange(() => TwentyThousindDayBox);
            }
        }

        private int _sixtyDayBox;
        public int SixtyDayBox
        {
            get
            {
                return _sixtyDayBox;
            }
            set
            {
                _sixtyDayBox = value;
                NotifyOfPropertyChange(() => SixtyDayBox);
            }
        }


        public SettingsViewModel()
        {
            SixtyDayBox = CommonValuesRepository.SixtyDayThreshold;
            TwentyThousindDayBox = CommonValuesRepository.TwentyThousindThreshold;
            CSVSickBox = CommonValuesRepository.ColumnCSick;
            CSVWorkBox= CommonValuesRepository.ColumnCWork;
        }




        public void SaveSettings()
        {
            Cursor.Current = Cursors.WaitCursor;
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath += "\\SIMPayrollConstants.txt";
            if (File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine($"SixtyDayThreshold = {SixtyDayBox}");
                    CommonValuesRepository.SixtyDayThreshold = SixtyDayBox;
                    sw.WriteLine($"TwentyThousindThreshold = {TwentyThousindDayBox}");
                    CommonValuesRepository.TwentyThousindThreshold = TwentyThousindDayBox;
                    sw.WriteLine(CSVSickBox);
                    CommonValuesRepository.ColumnCSick = CSVSickBox;
                    sw.WriteLine(CSVWorkBox);
                    CommonValuesRepository.ColumnCWork = CSVWorkBox;
                }
            }
            else
            {
                new Notification(Notification.NotificationType.Warning, "Programmet skal genstartes for at gendanne manglende filer.", 120);
            }
        }
    }
}
