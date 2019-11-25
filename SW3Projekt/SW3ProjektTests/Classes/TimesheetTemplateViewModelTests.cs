using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.ViewModels;

namespace SW3ProjektTests.Classes
{
    [TestClass]
    public class TimesheetTemplateViewModelTests
    {

        [TestMethod]
        public void GetDate_WhenWeek1AndDaysToAdd0_ReturnsMondayWeek1()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            ttvm.Timesheet.Year = 2020;
            ttvm.Timesheet.WeekNumber = 1;
            var daysToAdd = 0;
            var expected = new DateTime(2019, 12, 30);


            // Act.
            DateTime actual = ttvm.GetDate(daysToAdd);

            // Assert.
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetDate_WhenWeek1IsInPreviousYear_ReturnsCorrectDate()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            ttvm.Timesheet.Year = 2020;
            ttvm.Timesheet.WeekNumber = 1;
            var daysToAdd = 1;
            var expected = new DateTime(2019, 12, 31);


            // Act.
            DateTime actual = ttvm.GetDate(daysToAdd);

            // Assert.
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetDate_WhenMiddleDayOfYear_ReturnsMiddleDayOfYear()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            ttvm.Timesheet.Year = 2019;
            ttvm.Timesheet.WeekNumber = 27;
            var daysToAdd = 0;
            var expected = new DateTime(2019, 7, 1);


            // Act.
            DateTime actual = ttvm.GetDate(daysToAdd);

            // Assert.
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetDate_WhenADayFarIntheFuture_ReturnsCorrectDate()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            ttvm.Timesheet.Year = 3000;
            ttvm.Timesheet.WeekNumber = 43;
            var daysToAdd = 3;
            var expected = new DateTime(3000, 10, 23);


            // Act.
            DateTime actual = ttvm.GetDate(daysToAdd);

            // Assert.
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void GetDate_WhenWeekNumberIsBeyondYearsWeekCount_OverflowsToNextYear()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            // NB: Only 52 weeks in year 2022.
            ttvm.Timesheet.Year = 2022;
            ttvm.Timesheet.WeekNumber = 53;
            var daysToAdd = 0;
            var expected = new DateTime(2023, 1, 2);


            // Act.
            DateTime actual = ttvm.GetDate(daysToAdd);

            // Assert.
            Assert.AreEqual(expected, actual);
        }
    }
}
