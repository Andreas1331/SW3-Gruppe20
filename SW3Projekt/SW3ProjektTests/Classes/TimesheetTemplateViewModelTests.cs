using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.ViewModels;
using SW3Projekt.Models;

namespace SW3ProjektTests.Classes
{
    [TestClass]
    public class TimesheetTemplateViewModelTests
    {
        #region GetDateTests
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
        #endregion

        #region BtnMondayAddEntryTests
        [TestMethod]
        public void BtnMondayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnMondayAddEntry();
            ttvm.MondayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnMondayAddEntry();

            //Act.
            ttvm.MondayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.MondayEntries[0].TimesheetEntry.ProjectID, ttvm.MondayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnMondayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnMondayAddEntry();
            ttvm.MondayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnMondayAddEntry();

            //Act.
            ttvm.MondayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.MondayEntries[0].TimesheetEntry.ProjectID, ttvm.MondayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnTuesdayAddEntryTests
        [TestMethod]
        public void BtnTuesdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnTuesdayAddEntry();
            ttvm.TuesdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnTuesdayAddEntry();

            //Act.
            ttvm.TuesdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.TuesdayEntries[0].TimesheetEntry.ProjectID, ttvm.TuesdayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnTuesdayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnTuesdayAddEntry();
            ttvm.TuesdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnTuesdayAddEntry();

            //Act.
            ttvm.TuesdayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.TuesdayEntries[0].TimesheetEntry.ProjectID, ttvm.TuesdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnWednesdayAddEntryTests
        [TestMethod]
        public void BtnWednesdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnWednesdayAddEntry();
            ttvm.WednesdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnWednesdayAddEntry();

            //Act.
            ttvm.WednesdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.WednesdayEntries[0].TimesheetEntry.ProjectID, ttvm.WednesdayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnWednesdayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnWednesdayAddEntry();
            ttvm.WednesdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnWednesdayAddEntry();

            //Act.
            ttvm.WednesdayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.WednesdayEntries[0].TimesheetEntry.ProjectID, ttvm.WednesdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnThursdayAddEntryTests
        [TestMethod]
        public void BtnThursdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnThursdayAddEntry();
            ttvm.ThursdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnThursdayAddEntry();

            //Act.
            ttvm.ThursdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.ThursdayEntries[0].TimesheetEntry.ProjectID, ttvm.ThursdayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnThursdayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnThursdayAddEntry();
            ttvm.ThursdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnThursdayAddEntry();

            //Act.
            ttvm.ThursdayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.ThursdayEntries[0].TimesheetEntry.ProjectID, ttvm.ThursdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnFridayAddEntryTests
        [TestMethod]
        public void BtnFridayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnFridayAddEntry();
            ttvm.FridayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnFridayAddEntry();

            //Act.
            ttvm.FridayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.FridayEntries[0].TimesheetEntry.ProjectID, ttvm.FridayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnFridayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnFridayAddEntry();
            ttvm.FridayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnFridayAddEntry();

            //Act.
            ttvm.FridayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.FridayEntries[0].TimesheetEntry.ProjectID, ttvm.FridayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnSaturdayAddEntryTests
        [TestMethod]
        public void BtnSaturdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnSaturdayAddEntry();
            ttvm.SaturdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnSaturdayAddEntry();

            //Act.
            ttvm.SaturdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.SaturdayEntries[0].TimesheetEntry.ProjectID, ttvm.SaturdayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnSaturdayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnSaturdayAddEntry();
            ttvm.SaturdayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnSaturdayAddEntry();

            //Act.
            ttvm.SaturdayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.SaturdayEntries[0].TimesheetEntry.ProjectID, ttvm.SaturdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnSundayAddEntryTests
        [TestMethod]
        public void BtnSundayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnSundayAddEntry();
            ttvm.SundayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnSundayAddEntry();

            //Act.
            ttvm.SundayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.SundayEntries[0].TimesheetEntry.ProjectID, ttvm.SundayEntries[1].TimesheetEntry.ProjectID);
        }

        [TestMethod]
        public void BtnSundayAddEntry_WhenAddingNewEntryWithSameProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel { EmployeeRoutes = new List<Route>() };
            ttvm.BtnSundayAddEntry();
            ttvm.SundayEntries[0].TimesheetEntry.ProjectID = "1060";
            ttvm.BtnSundayAddEntry();

            //Act.
            ttvm.SundayEntries[1].TimesheetEntry.ProjectID = "1060";

            // Assert.
            Assert.AreEqual(ttvm.SundayEntries[0].TimesheetEntry.ProjectID, ttvm.SundayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion
    }
}
