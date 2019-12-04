using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System.Linq;
using System.Collections.Generic;
using System.IO;

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
            string fileName = "CSVtestFile";
            string filePath = Environment.CurrentDirectory;

            File.Delete(filePath + '\\' + fileName + ".csv"); //Clear for test

            string expected = string1 + "\r\n";
            
            //Act
            Printer.Lines.Add(string1);
            Printer.Print(fileName, filePath);

            //Assert
            string actual = System.IO.File.ReadAllText(filePath + "\\" + fileName + ".csv");
            Assert.AreEqual(expected, actual);

            File.Delete(filePath + '\\' + fileName + ".csv"); //Clear after test
        }
    }
}
