//TEST CODE TIL Print() - Ligges i MainWindow. Priter to calculations til skrivebord
/*
        public MainWindow()
        {
            //Eksempel som tager to calculations og udskriver dem i en csv fil til skrivebord
            Calculation cal = new Calculation();
            cal.H = "ddddd";

            Calculation cal2 = new Calculation();
            cal2.G = "gggggg";

            Printer.AddCalculationToList(cal); //Kaldes for hver linje.
            Printer.AddCalculationToList(cal2);

            Printer.Print(); //Kaldes endeligt for at printe resultaterne ud

            InitializeComponent();
        }
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt
{
    static class Printer
    {
        private static string OutputLocation { get; set; }
        private static string FileName { get; set; }

        private static List<string> Lines = new List<string>(); //List of rows formatted as string ready to be exported

        //Format ONE calculation from calculator to a string and
        public static void AddCalculationToList(Calculation c)
        {
            Lines.Add($"{c.A};{c.B};{c.C};{c.D};{c.E};{c.F};{c.G};{c.H};{c.I};{c.J};{c.K};;;;;;;{c.R};;;;;;;;;{c.AB};");
        }

        //Prints a list of strings to a file
        public static void Print()
        {
            //Get outputlocation
            OutputLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //Test data (kommer fra instillinger)

            //Get Filename
            DateTime d = DateTime.Now;
            FileName = $"\\{d.Day}{d.Month}{d.Year}"; //Names the file, the date of export

            //Find a new file name if file name is already found. Like myFile(i).csv
            string path = OutputLocation + FileName + ".csv";

            for (int i = 0; i < 100; i++) //Only checks 100 times. Meaning if 100 it doesnt print if there are 100 files with the same name
            {
                if (File.Exists(path)) //Make a new name if file exists
                {
                    path = OutputLocation + FileName + $"({i})" + ".csv";
                }
            }

            if (File.Exists(path)) //Final check
                throw new Exception("Choose new name");

            //Now print
            File.AppendAllLines(path, Lines); //Write all lines to the new file
            Lines.Clear(); //Clear out list for next print
        }
    }

    class Calculation //Representerer kun en række i csv filen. IKKE ALLE BEHØVES AT BLIVE UDFYLDT
    {
        //Sat op efter csv filen samt forklaring hvad de enkelte kollonner bruges til i csv filen.

        //Kolonner
        public string A = "1242"; //Forekommer igennem alle
        public string B = "1"; //Forekommer igennem alle
        public string C = "MLE-40-LONA"; //Forekommer igennem alle
        public string D = "";//Medarbejder nummer ex. 272
        public string E = ""; //Dato ex. 25062018
        public string F = ""; //Tomt igennem alle
        public string G = ""; //Art ex. 1100 for løn og 9010 for kørsel
        public string H = ""; //Kommentar
        public string I = ""; //Værdi: -1 for 510; timer for 1100 1311 1312 1316 1318 1319 1371 1373 1400; km for 9010;
        public string J = ""; //Sats for 9010 ex. 3.54
        public string K = ""; //Værdi: penge for 1181 (skure penge, NJV); penge til 4483 (feriefri); penge for 9020 9031 9100 9470
        //L M N O P Q er alle tomme
        public string R = ""; //Bruges noglegange i 1100 9010 ??? 2017 forekommer i disse tilfælde
        //S T U V X Y Z AA
        public string AB = ""; //Indeholder Projekt nummer. Ikke alle har et
    }
}