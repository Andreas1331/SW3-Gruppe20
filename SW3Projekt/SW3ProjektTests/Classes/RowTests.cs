using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System.Linq;
using System.Collections.Generic;

namespace SW3ProjektTests.Classes
{
    [TestClass]
    public class RowTests
    {
        [TestMethod]
        public void FormatDateTimeToDate_OneDegitDayTwoDigitMonth_OneDegitDayTwoDigitMonthFormat()
        {
            //Arrange
            int day = 2;
            int month = 10;
            int year = 2019;

            DateTime testDateTime = new DateTime(year, month, day);

            string expected = $"{day}{month}{year}";

            Row testRow = new Row(
                new TimesheetEntry
                {
                    EmployeeID = 12,
                    Date = testDateTime,
                    ProjectID = "23"
                },
                new VismaEntry
                {
                    VismaID = 34,
                    Comment = "test",
                    LinkedRate = new Rate { Type = "Arbejde" }
                },
                false
                );

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            string actual = (string)testRowObj.Invoke("FormatDateTimeToDate", testDateTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FormatDateTimeToDate_TwoDegitDayTwoDigitMonth_TwoDegitDayTwoDigitMonthFormat()
        {
            //Arrange
            int day = 20;
            int month = 10;
            int year = 2019;

            DateTime testDateTime = new DateTime(year, month, day);

            string expected = $"{day}{month}{year}";

            Row testRow = new Row(
            new TimesheetEntry
            {
                EmployeeID = 12,
                Date = testDateTime,
                ProjectID = "23"
            },
            new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                LinkedRate = new Rate { Type = "Arbejde" }
            },
            false
            );

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            string actual = (string)testRowObj.Invoke("FormatDateTimeToDate", testDateTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FormatDateTimeToDate_OneDegitDayOneDigitMonth_OneDegitDayTwoDigitMonthFormat()
        {
            //Arrange
            int day = 4;
            int month = 2;
            int year = 2019;

            DateTime testDateTime = new DateTime(year, month, day);

            string expected = $"{day}0{month}{year}";

            Row testRow = new Row(
            new TimesheetEntry
            {
                EmployeeID = 12,
                Date = testDateTime,
                ProjectID = "23"
            },
            new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                LinkedRate = new Rate { Type = "Arbejde" }
            },
            false
            );

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            string actual = (string)testRowObj.Invoke("FormatDateTimeToDate", testDateTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FormatDateTimeToDate_TwoDegitDayOneDigitMonth_TwoDegitDayTwoDigitMonthFormat()
        {
            //Arrange
            int day = 24;
            int month = 4;
            int year = 2019;

            DateTime testDateTime = new DateTime(year, month, day);

            string expected = $"{day}0{month}{year}";

            Row testRow = new Row(
            new TimesheetEntry
            {
                EmployeeID = 12,
                Date = testDateTime,
                ProjectID = "23"
            },
            new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                LinkedRate = new Rate { Type = "Arbejde" }
            },
            false
            );

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            string actual = (string)testRowObj.Invoke("FormatDateTimeToDate", testDateTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignValue_SaveAsMoney_SaveInColumnK()
        {
            //Arrange
            double testValue = 2;
            string expected = "2";

            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = DateTime.Now,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = testValue,
                LinkedRate = new Rate { Type = "Arbejde", SaveAsMoney = true } //Type of arbejde
            };

            Row testRow = new Row(tse, ve, false);

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            testRowObj.Invoke("AssignValue", ve);
            string actual = (string)testRowObj.GetField("K");

            //Assert

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignValue_NotSaveAsMoney_SaveInColumnI()
        {
            //Arrange
            double testValue = 2;
            string expected = "2";

            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = DateTime.Now,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = testValue,
                LinkedRate = new Rate { Type = "Kørsel" } //Type of arbejde
            };

            Row testRow = new Row(tse, ve, false);

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            testRowObj.Invoke("AssignValue", ve);
            string actual = (string)testRowObj.GetField("I");

            //Assert

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignRate_RateTypeOfKørsel_SaveInColumnJ()
        {
            //Arrange
            double testRateValue = 42000;
            string expected = testRateValue.ToString();

            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = DateTime.Now,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = 2,
                RateValue = testRateValue,
                LinkedRate = new Rate { Type = "Kørsel" } //Type of arbejde
            };

            Row testRow = new Row(tse, ve, false);

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            testRowObj.Invoke("AssignValue", ve);
            string actual = (string)testRowObj.GetField("J");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AssignRate_RateTypeOfNotKørsel_NoSaveInColumnJ()
        {
            //Arrange
            double testRateValue = 42000;
            string expected = "";

            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = DateTime.Now,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = 2,
                RateValue = testRateValue,
                LinkedRate = new Rate { Type = "Arbejde" } //Type of arbejde
            };

            Row testRow = new Row(tse, ve, false);

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            testRowObj.Invoke("AssignValue", ve);
            string actual = (string)testRowObj.GetField("J");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetLine_normalrow_correctFormat()
        {
            int day = 12;
            int month = 8;
            int year = 2019;

            DateTime exampleDateTime = new DateTime(year, month, day);

            //Arrange
            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = exampleDateTime,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = 2,
                RateValue = 20,
                LinkedRate = new Rate { Type = "Arbejde" }
            };

            Row testRow = new Row(tse, ve, false);

            string expected = $"1242;1;MLE-40-LONA;{tse.EmployeeID};{day}0{8}{2019};;{ve.VismaID};{ve.Comment};{ve.Value};;;;;;;;;;;;;;;;;;;{tse.ProjectID};";

            //Act
            string actual = testRow.GetLine();


            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetLine_sickrow_correctFormat()
        {
            int day = 12;
            int month = 8;
            int year = 2019;

            DateTime exampleDateTime = new DateTime(year, month, day);

            //Arrange
            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = exampleDateTime,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = 2,
                RateValue = 20,
                LinkedRate = new Rate { Type = "Arbejde" }
            };

            Row testRow = new Row(tse, ve, true);

            string expected = $"1242;1;MLE-40-FRAV;{tse.EmployeeID};{day}0{8}{2019};{day}0{8}{2019};{ve.VismaID};{ve.Comment};{ve.Value};;;;;;;;;;;;;;;;;;;{tse.ProjectID};";

            //Act
            string actual = testRow.GetLine();


            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Row_CommentIsNull_FormatWithoutAComment()
        {
            int day = 12;
            int month = 8;
            int year = 2019;

            DateTime exampleDateTime = new DateTime(year, month, day);

            //Arrange
            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = exampleDateTime,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = null,
                Value = 2,
                RateValue = 20,
                LinkedRate = new Rate { Type = "Arbejde" }
            };

            Row testRow = new Row(tse, ve, true);

            string expected = $"1242;1;MLE-40-FRAV;{tse.EmployeeID};{day}0{8}{2019};{day}0{8}{2019};{ve.VismaID};;{ve.Value};;;;;;;;;;;;;;;;;;;{tse.ProjectID};";

            //Act
            string actual = testRow.GetLine();


            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Row_ProjectIsNull_FormatWithoutAProject()
        {
            int day = 12;
            int month = 8;
            int year = 2019;

            DateTime exampleDateTime = new DateTime(year, month, day);

            //Arrange
            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date = exampleDateTime,
                ProjectID = null
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 34,
                Comment = "test",
                Value = 2,
                RateValue = 20,
                LinkedRate = new Rate { Type = "Arbejde" }
            };

            Row testRow = new Row(tse, ve, true);

            string expected = $"1242;1;MLE-40-FRAV;{tse.EmployeeID};{day}0{8}{2019};{day}0{8}{2019};{ve.VismaID};{ve.Comment};{ve.Value};;;;;;;;;;;;;;;;;;;;";

            //Act
            string actual = testRow.GetLine();


            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Row_VismaID510_SaveInColoumnR()
        {
            //Arrange
            DateTime testDateTime = new DateTime(2019, DateTime.Now.Month, DateTime.Now.Day);
            string expected = (testDateTime.Year - 1).ToString();

            TimesheetEntry tse = new TimesheetEntry
            {
                EmployeeID = 12,
                Date =  testDateTime,
                ProjectID = "23"
            };

            VismaEntry ve = new VismaEntry
            {
                VismaID = 510,
                Comment = "test",
                Value = 2,
                RateValue = 203,
                LinkedRate = new Rate { Type = "Kørsel" } //Type of arbejde
            };

            Row testRow = new Row(tse, ve, false);

            PrivateObject testRowObj = new PrivateObject(testRow);

            //Act
            testRowObj.Invoke("AssignValue", ve);
            string actual = (string)testRowObj.GetField("R");

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}