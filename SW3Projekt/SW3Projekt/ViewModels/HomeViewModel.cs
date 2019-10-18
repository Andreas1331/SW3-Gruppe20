using Caliburn.Micro;
using System;
using System.Globalization;

namespace SW3Projekt.ViewModels
{
    public class HomeViewModel : Screen
    {
        public string CurrentDate {
            get {
                CultureInfo culInfo = new CultureInfo("da-DK");

                // Instantiate a new calender based on the danish culture.
                Calendar cal = culInfo.Calendar;

                // Get the current weeknumber based on the danish calender and current time.
                string weekNumber = cal.GetWeekOfYear(DateTime.Now, 
                    DateTimeFormatInfo.CurrentInfo.CalendarWeekRule, 
                    DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek).ToString();

                // Return the formatted date
                return DateTime.Now.ToString("dd/MM/yyyy", culInfo) + " (Uge: " + weekNumber + ")";
            }
        }
    }
}
