using System;
using System.Globalization;

namespace SW3Projekt.Tools
{
    public static class DateHelper
    {
        public static int GetWeekNumber(DateTime date)
        {
            CultureInfo culInfo = new CultureInfo("da-DK");
            DateTimeFormatInfo dfi = culInfo.DateTimeFormat;
            Calendar cal = culInfo.Calendar;

            return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        //Convert the entered weeknumber to a datetime
        public static DateTime WeekNumToDateTime(int weekNum, int year, int dayInWeek)
        {
            var cal = CultureInfo.CurrentCulture.Calendar;
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffsetThursday = DayOfWeek.Thursday - jan1.DayOfWeek;
            DateTime firstThursday = jan1.AddDays(daysOffsetThursday);
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            if (firstWeek == 1)
                weekNum -= 1;

            return firstThursday.AddDays((weekNum * 7) - 3 + dayInWeek);
        }
    }
}