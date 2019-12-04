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
    public class DateHelperTests
    {
        [TestMethod]
        public void GetWeekNumber_InputDateTime_CorrectOutputWeekNumber() 
        {
            //Arrange
            DateTime dateTime = new DateTime(2019, 12, 2);
            int expected = 49;

            //Act
            int actual = DateHelper.GetWeekNumber(dateTime);

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WeekNumToDateTime_InputWeekNumber_CorrectDateTime()
        {
            //Arrange
            int weekNumber = 49;
            int year = 2019;
            int dayInWeek = 0; //Monday

            DateTime expected = new DateTime(2019, 12, 2);

            //Act
            DateTime actual = DateHelper.WeekNumToDateTime(weekNumber, year, dayInWeek);

            //Assert
            Assert.AreEqual(expected.ToShortDateString(), actual.ToShortDateString());
        }

        [TestMethod]
        public void WeekNumToDateTime_FirstWeekIsOne_CorrectDateTime()
        {
            //Arrange
            int weekNumber = 1;
            int year = 2020;
            int dayInWeek = 0;

            DateTime expected = new DateTime(2019, 12, 30);

            //Act
            DateTime actual = DateHelper.WeekNumToDateTime(weekNumber, year, dayInWeek);

            //Assert
            Assert.AreEqual(expected.ToShortDateString(), actual.ToShortDateString());
        }
    }
}
