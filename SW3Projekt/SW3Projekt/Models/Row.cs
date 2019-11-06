using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class Row //Representerer en række i csv filen
    {
        //PROPERTIES
        private string A = "1242"; //Forekommer igennem alle
        private string B = "1"; //Forekommer igennem alle
        private string C = "MLE-40-LONA"; //Forekommer igennem alle
        private string D = "";//Medarbejder nummer ex. 272
        private string E = ""; //Dato ex. 25062018
        //F er tom
        private string G = ""; //Art ex. 1100 for løn og 9010 for kørsel
        private string H = ""; //Kommentar
        private string I = ""; //Værdi: -1 for 510; timer for 1100 1311 1312 1316 1318 1319 1371 1373 1400 1410; km for 9010;
        private string J = ""; //Sats for 9010 ex. 3.54
        private string K = ""; //Værdi: penge for 1181 (skure penge, NJV); penge til 4483 (feriefri); penge for 9020 9031 9100 9470
        //L M N O P Q er alle tomme
        private string R = ""; //Bruges noglegange i 1100 9010 ??? 2017 forekommer i disse tilfælde
        //S T U V X Y Z AA
        private string AB = ""; //Indeholder Projekt nummer. Ikke alle har et

        //CONSTRUCTOR
        public Row(TimesheetEntry tse, VismaEntry ve)
        {
            D = tse.EmployeeID.ToString();
            E = FormatDateTimeToDate(tse.Date);
            G = ve.VismaID.ToString();
            H = ve.Comment = ve.Comment ?? "";
            I = FindColI(ve);
            J = FindColJ(ve);
            K = FindColK(ve);
            //R ved ikke hvorfor kollone R nogengange indehoolder 2017
            AB = tse.ProjectID ?? "";
        }

        //METHODS
        public string GetLine() //Line for csv file
        {
            return ($"{A};{B};{C};{D};{E};;{G};{H};{I};{J};{K};;;;;;;{R};;;;;;;;;;{AB};");
        }

        private string FormatDateTimeToDate(DateTime dateTime) //Converts DateTime to date format used in csv file.
        {
            string day = dateTime.Day.ToString();
            string month = dateTime.Month < 10 ? dateTime.Month.ToString() : 0.ToString() + dateTime.Month.ToString();
            string year = dateTime.Year.ToString();

            return day + month + year;
        }

        private string ConvertToString(VismaEntry ve)
        {
            if (ve.Comment == null)
                return "";
            return ve.Comment.ToString();
        }

        private string FindColI(VismaEntry ve)
        {
            switch (ve.VismaID)
            {
                case 510:
                    return "-1"; //Always -1 for 510

                case 1010: //Timer
                case 1100:
                case 1311:
                case 1312:
                case 1313:
                case 1314:
                case 1315:
                case 1316:
                case 1317:
                case 1318:
                case 1319:
                case 1371:
                case 1372:
                case 1373:
                case 1400:
                case 1410: //Timer
                case 9010: //Penge
                    return ve.Value.ToString(); //Get value
                default:
                    return null;
            }
        }
        private string FindColJ(VismaEntry ve)
        {
            switch (ve.VismaID)
            {
                case 9010: //Penge
                    return ve.RateValue.ToString(); //Get value
                default:
                    return null;
            }
        }
        private string FindColK(VismaEntry ve)
        {
            switch (ve.VismaID)
            {
                case 1181:
                case 4483:
                case 9020:
                case 9031:
                case 9100:
                case 9470:
                    return ve.Value.ToString();
                default:
                    return null;
            }
        }
    }
}
