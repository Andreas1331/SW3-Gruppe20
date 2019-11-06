using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SW3Projekt.Models;

namespace SW3Projekt
{
    static class Printer
    {
        //PROPERTIES
        private static string OutputLocation { get; set; }
        private static string FileName { get; set; }

        public static List<string> Lines = new List<string>(); //List of rows formatted as string ready to be exported

        //METHODS
        public static void Print()//Prints Lines to a file
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
}