using SW3Projekt.Tools;
using System;

namespace SW3Projekt.Models
{
    //Represent a row in the csv. file
    public class Row
    {
        //PROPERTIES
        private string A = "1242";
        private string B = "1";
        private string C = "";
        private string D = ""; //Employee ID
        private string E = ""; //Date
        private string F = ""; //Date. Only used for sick file, no idea why it is there twice...
        private string G = ""; //Rate ID
        private string H = ""; //Comment
        private string I = ""; //Value field
        private string J = ""; //Value field. Only used for the driving rate
        private string K = ""; //Value field
        //L M N O P Q empty
        private string R = ""; //???
        //S T U V X Y Z AA empty
        private string AB = ""; //Project Number

        //CONSTRUCTOR
        public Row(TimesheetEntry tse, VismaEntry ve, bool sickFlag)
        {
            if (sickFlag)
                C = CommonValuesRepository.ColumnCSick;
            else
                C = CommonValuesRepository.ColumnCWork;

            D = tse.EmployeeID.ToString();
            E = FormatDateTimeToDate(tse.Date);
            if (sickFlag)
            {
                F = E; // it has to have the same information as coloumn E
            }
            G = ve.VismaID.ToString();
            H = ve.Comment ?? "";

            //Find og tildel entry værdierne i en kollonne.
            AssignValue(ve);
            AssignRate(ve);
            if (ve.VismaID == 510)
            {
                R = (tse.Date.Year - 1).ToString();
            }
            //R ved ikke hvorfor kollone R nogengange indehoolder 2017
            AB = tse.ProjectID ?? "";
        }

        //METHODS
        public string GetLine() //Line for csv file
        {
            return ($"{A};{B};{C};{D};{E};{F};{G};{H};{I};{J};{K};;;;;;;{R};;;;;;;;;;{AB};");
        }

        private string FormatDateTimeToDate(DateTime dateTime) //Converts DateTime to date format used in csv file.
        {
            string day = dateTime.Day.ToString();
            string month = dateTime.Month < 10 ? 0.ToString() + dateTime.Month.ToString() : dateTime.Month.ToString();
            string year = dateTime.Year.ToString();
            return day + month + year;
        }

        private void AssignValue(VismaEntry ve)
        {
            if (ve.LinkedRate.SaveAsMoney && ve.LinkedRate.Type != "Kørsel")
                K = ve.Value.ToString();
            else
                I = ve.Value.ToString();
        }

        private void AssignRate(VismaEntry ve)
        {
            if (ve.LinkedRate.Type == "Kørsel")
                J = ve.RateValue.ToString();
        }
    }
}
