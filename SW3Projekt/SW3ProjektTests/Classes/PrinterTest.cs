using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System.Linq;
using System.Collections.Generic;

namespace SW3ProjektTests.Classes
{
    [TestClass]
    public class PrinterTest
    {
        [TestMethod]
        public void Print_StandardPrint_FileIsPrinted()
        {
            //Arrange
            string string1 = "test1";
            string string2 = "test2";
            string fileName = "CSVtestFile";
            string filePath = Environment.CurrentDirectory;

            string expected = string1 + Environment.NewLine + string2;

            Console.WriteLine(expected + "___" + filePath + "\\" + fileName);
            
            //Act
            Printer.Lines.Add(string1);
            Printer.Lines.Add(string2);
            Printer.Print(fileName, filePath);

            //Assert
            string actual = System.IO.File.ReadAllText(filePath + "\\" + fileName + ".csv");
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
