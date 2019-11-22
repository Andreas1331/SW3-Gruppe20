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
    }
}
