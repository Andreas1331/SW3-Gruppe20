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

        public static List<string> Lines = new List<string>(); //List of rows formatted as string ready to be exported

        //METHODS
        public static int Print(string FileName, string OutputLocation)//Prints Lines to a file
        {
            //Find a new file name if file name is already found. Like myFile(i).csv
            string path = OutputLocation + '\\' + FileName + ".csv";

            for (int i = 1; i <= 100; i++) //Only checks 100 times. Meaning if 100 it doesnt print if there are 100 files with the same name
                if (File.Exists(path)) //Make a new name if file exists
                    path = OutputLocation + '\\'+ FileName + $"({i})" + ".csv";
                else
                    break;

            if (File.Exists(path)) //Final check
                return -1;

            //Now print
            File.AppendAllLines(path, Lines); //Write all lines to the new file
            Lines.Clear(); //Clear out list for next print

            return 0;
        }
    }
}