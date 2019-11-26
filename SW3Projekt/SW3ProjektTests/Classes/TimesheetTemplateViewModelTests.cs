using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.ViewModels;

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
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.MondayEntries.Add(entryvm1);
            ttvm.MondayEntries.Add(entryvm2);

            //Act.
            ttvm.MondayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.MondayEntries[0].TimesheetEntry.ProjectID, ttvm.MondayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnTuesdayAddEntryTests
        [TestMethod]
        public void BtnTuesdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.TuesdayEntries.Add(entryvm1);
            ttvm.TuesdayEntries.Add(entryvm2);

            //Act.
            ttvm.TuesdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.TuesdayEntries[0].TimesheetEntry.ProjectID, ttvm.TuesdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnWednesdayAddEntryTests
        [TestMethod]
        public void BtnWednesdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.WednesdayEntries.Add(entryvm1);
            ttvm.WednesdayEntries.Add(entryvm2);

            //Act.
            ttvm.WednesdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.WednesdayEntries[0].TimesheetEntry.ProjectID, ttvm.WednesdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnThursdayAddEntryTests
        [TestMethod]
        public void BtnThursdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.ThursdayEntries.Add(entryvm1);
            ttvm.ThursdayEntries.Add(entryvm2);

            //Act.
            ttvm.ThursdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.ThursdayEntries[0].TimesheetEntry.ProjectID, ttvm.ThursdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnFridayAddEntryTests
        [TestMethod]
        public void BtnFridayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.FridayEntries.Add(entryvm1);
            ttvm.FridayEntries.Add(entryvm2);

            //Act.
            ttvm.FridayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.FridayEntries[0].TimesheetEntry.ProjectID, ttvm.FridayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnSaturdayAddEntryTests
        [TestMethod]
        public void BtnSaturdayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.SaturdayEntries.Add(entryvm1);
            ttvm.SaturdayEntries.Add(entryvm2);

            //Act.
            ttvm.SaturdayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.SaturdayEntries[0].TimesheetEntry.ProjectID, ttvm.SaturdayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion

        #region BtnSundayAddEntryTests
        [TestMethod]
        public void BtnSundayAddEntry_WhenAddingNewEntryWithDifferentProjectId_EntryIsAdded()
        {

            // Arrange.
            var ttvm = new TimesheetTemplateViewModel();

            var entryvm1 = new TimesheetEntryViewModel(ttvm, true);
            var entryvm2 = new TimesheetEntryViewModel(ttvm, true);
            entryvm1.TimesheetEntry.ProjectID = "1060";
            ttvm.SundayEntries.Add(entryvm1);
            ttvm.SundayEntries.Add(entryvm2);

            //Act.
            ttvm.SundayEntries[1].TimesheetEntry.ProjectID = "1100";

            // Assert.
            Assert.AreNotEqual(ttvm.SundayEntries[0].TimesheetEntry.ProjectID, ttvm.SundayEntries[1].TimesheetEntry.ProjectID);
        }

        #endregion
    }
}
