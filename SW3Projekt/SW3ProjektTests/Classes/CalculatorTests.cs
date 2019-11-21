using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System.Linq;

namespace SW3ProjektTests.Classes
{
    [TestClass]
    public class CalculatorTests
    {

        #region DaysApplyTests
        [TestMethod]
        public void DaysApply_WhenCompleteConjunction_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            Days daysPeriod = Days.Monday;
            int entryDay = 1; 

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("DaysApply", daysPeriod, entryDay);

            //Assert.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DaysApply_WhenPartialOverlap_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            Days daysPeriod = (Days)127;
            int entryDay = 1;

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("DaysApply", daysPeriod, entryDay);

            //Assert.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DaysApply_WhenCompleteDisjuction_ReturnsFalse()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            Days daysPeriod = Days.Monday;
            int entryDay = 0;

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("DaysApply", daysPeriod, entryDay);

            //Assert.
            Assert.IsFalse(result);
        }

        #endregion

        #region TypesCompatibleTests
        [TestMethod]
        public void TypesCompatible_1stEQFalse2ndEQFalse3rdEQFalse_ReturnsFalse()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Lav ingenting"
            };

            var rate = new Rate
            {
                Type = "Arbejde",
                Name = "Weird"
            };

            //Act.
            bool result = (bool) testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQTrue2ndEQFalse3rdEQFalse_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Arbejde"
            };

            var rate = new Rate
            {
                Type = "Arbejde",
                Name = "Weird"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQFalse2ndEQTrue3rdEQFalse_ReturnsFalse()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Forskudttid"
            };

            var rate = new Rate
            {
                Type = "Arbejde",
                Name = "Weird"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQFalse2ndEQFalse3rdEQTrue_ReturnsFalse()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Lav ingenting"
            };

            var rate = new Rate
            {
                Type = "Arbejde",
                Name = "Normal"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQTrue2ndEQTrue3rdEQFalse_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Forskudttid"
            };

            var rate = new Rate
            {
                Type = "Forskudttid",
                Name = "Weird"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQFalse2ndEQTrue3rdEQTrue_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Forskudttid"
            };

            var rate = new Rate
            {
                Type = "Arbejde",
                Name = "Normal"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQTrue2ndEQFalse3rdEQTrue_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Arbejde"
            };

            var rate = new Rate
            {
                Type = "Arbejde",
                Name = "Normal"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TypesCompatible_1stEQTrue2ndEQTrue3rdEQTrue_ReturnsTrue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                SelectedTypeComboBoxItem = "Forskudttid"
            };

            var rate = new Rate
            {
                Type = "Forskudttid",
                Name = "Normal"
            };

            //Act.
            bool result = (bool)testCalculator.InvokeStatic("TypesCompatible", tsEntry, rate);

            //Assert.
            Assert.IsTrue(result);
        }


        #endregion

        #region CheckAndApplyHourlyRate
        [TestMethod]
        public void CheckAndApplyHourlyRate_WhenEStartLEQREndAndEEndGEQRStart_AppliesHourlyRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            //Act.
            testCalculator.InvokeStatic("CheckAndApplyHourlyRate", tsEntry, rate);

            //Assert.
            Assert.IsTrue(tsEntry.vismaEntries.Any());
        }

        [TestMethod]
        public void CheckAndApplyHourlyRate_WhenEStartGTREndAndEEndGEQRStart_AppliesHourlyRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 5, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 4, 0, 0)
            };

            //Act.
            testCalculator.InvokeStatic("CheckAndApplyHourlyRate", tsEntry, rate);

            //Assert.
            Assert.IsFalse(tsEntry.vismaEntries.Any());
        }

        [TestMethod]
        public void CheckAndApplyHourlyRate_WhenEStartLEQREndAndEEndLTRStart_AppliesHourlyRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 5, 0, 0),
                EndTime = new DateTime(1, 1, 1, 3, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            //Act.
            testCalculator.InvokeStatic("CheckAndApplyHourlyRate", tsEntry, rate);

            //Assert.
            Assert.IsFalse(tsEntry.vismaEntries.Any());
        }

        [TestMethod]
        public void CheckAndApplyHourlyRate_WhenEStartGTREndAndEEndLTRStart_AppliesHourlyRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 5, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 11, 0, 0),
                EndTime = new DateTime(1, 1, 1, 4, 0, 0)
            };

            //Act.
            testCalculator.InvokeStatic("CheckAndApplyHourlyRate", tsEntry, rate);

            //Assert.
            Assert.IsFalse(tsEntry.vismaEntries.Any());
        }
        #endregion

        #region ApplyHourlyRateTests
        [TestMethod]
        public void ApplyHourlyRate_WhenValidTimeRange_VismaEntryIsAddedToVismaEntries()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var addedElement = tsEntry.vismaEntries.FirstOrDefault();

            //Assert.
            Assert.IsNotNull(addedElement);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenNonOverlappingTimeRanges_VismaEntryIsAddedToVismaEntries()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 17, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 17, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var addedElement = tsEntry.vismaEntries.FirstOrDefault();

            //Assert.
            Assert.IsNull(addedElement);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenRateNameIsNormal_BreakIsSubtracted()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0),
                BreakTime = 0.5
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0),
                Name = "Normal"
            };

            var expected = 23.5;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenRateNameIsNotNormal_BreakIsSubtracted()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0),
                BreakTime = 0.5
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0),
                Name = "Weird"
            };

            var expected = 24;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenCalled_VismaEntryVismaIDEqualsRateVismaID()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var expected = rate.VismaID;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().VismaID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenCalled_VismaEntryRateIDEqualsRateID()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var expected = rate.Id;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().RateID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenCalled_VismaEntryRateValueEqualsRateRateValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var expected = rate.RateValue;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().RateValue;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenCalled_VismaEntryTimesheetEntryIDEqualsRateTimesheetEntryID()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var expected = tsEntry.Id;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().TimesheetEntryID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenCalled_VismaEntryLinkedRateEqualsRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 23, 59, 0)
            };

            var expected = rate;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().LinkedRate;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartLTRStartAndEEndLTREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 0, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            var expected = 2;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartGTRStartAndEEndLTREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            var expected = 1;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartLTRStartAndEEndGTREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var expected = 1;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartEQRStartAndEEndLTREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            var expected = 1;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartLTRStartAndEEndEQREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var expected = 1;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartEQRStartAndEEndEQREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 5, 0, 0)
            };

            var expected = 2;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartEQRStartAndEEndGTREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            var expected = 3;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartGTRStartAndEEndEQREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var expected = 3;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyHourlyRate_WhenEStartGTRStartAndEEndGTREnd_AssignsCorrectVismaEntryValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43,
                StartTime = new DateTime(1, 1, 1, 4, 0, 0),
                EndTime = new DateTime(1, 1, 1, 7, 0, 0)
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95,
                StartTime = new DateTime(1, 1, 1, 3, 0, 0),
                EndTime = new DateTime(1, 1, 1, 6, 0, 0)
            };

            var expected = 2;

            //Act.
            testCalculator.InvokeStatic("ApplyHourlyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region ApplyDailyRateTests
        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryIsAddedToVismaEntries()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var addedElement = tsEntry.vismaEntries.FirstOrDefault();

            //Assert.
            Assert.IsNotNull(addedElement);
        }

        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryVismaIDEqualsRateVismaId()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            var expected = rate.VismaID;

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().VismaID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryRateIDEqualsRateId()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            var expected = rate.Id;

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().RateID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryRateValueEqualsRateRateValue()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            var expected = rate.RateValue;

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().RateValue;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryTimesheetEntryIDEqualsTSEntryId()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            var expected = tsEntry.Id;

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().TimesheetEntryID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryLinkedRateEqualsRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            var expected = rate;

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().LinkedRate;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDailyRate_WhenCalled_VismaEntryValueEquals1()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                Id = 43
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5,
                RateValue = 14.95
            };

            var expected = 1;

            //Act.
            testCalculator.InvokeStatic("ApplyDailyRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region ApplyDriveRateTests
        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryIsAddedToVismaEntries()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var addedElement = tsEntry.vismaEntries.FirstOrDefault();

            //Assert.
            Assert.IsNotNull(addedElement);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryVismaIDEqualsRateVismaId()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = rate.VismaID;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().VismaID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryRateIDEqualsRateId()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = rate.Id;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().RateID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryRateValueEqualsTSEntryDriveRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = tsEntry.DriveRate;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().RateValue;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryTimesheetEntryEqualsTSEntryId()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = tsEntry.Id;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().TimesheetEntryID;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryValueEqualsTSEntryKrTextBox()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = tsEntry.KrTextBox / tsEntry.DriveRate;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryLinkedRateEqualsRate()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = rate;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().LinkedRate;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyDriveRate_WhenCalled_VismaEntryCommentEqualsKorselPlusSelectedRoute()
        {

            //Arrange.
            var testCalculator = new PrivateType(typeof(Calculator));

            var tsEntry = new TimesheetEntry
            {
                DriveRate = 42,
                Id = 43,
                KrTextBox = 49.95,
                SelectedRouteComboBoxItem = "R66"
            };

            var rate = new Rate
            {
                VismaID = 1100,
                Id = 5
            };


            var expected = "Kørsel " + tsEntry.SelectedRouteComboBoxItem;

            //Act.
            testCalculator.InvokeStatic("ApplyDriveRate", tsEntry, rate);
            var actual = tsEntry.vismaEntries.First().Comment;

            //Assert.
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region ApplyRemainingRatesTest
        [TestMethod]
        public void ApplyRemainingRates_WhenSaveAsMoneyIsFalse_DoesNotMultiplyValue()
        {

            //Arrange.
            var vismaEntryList = Enumerable.Range(0, 100)
                                           .Select(n => new VismaEntry {Value = n, RateValue = n, LinkedRate = new Rate { SaveAsMoney = (n == 42) } })
                                           .ToList();

            var expected = 42 * 42;

            //Act.
            Calculator.ApplyRemainingRates(vismaEntryList);
            double actual = vismaEntryList[42].Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ApplyRemainingRates_WhenSaveAsMoneyIsFalse_MultipliesValue()
        {

            //Arrange.
            var vismaEntryList = Enumerable.Range(0, 100)
                                           .Select(n => new VismaEntry { Value = n, RateValue = n, LinkedRate = new Rate { SaveAsMoney = (n != 42) } })
                                           .ToList();

            var expected = 42;

            //Act.
            Calculator.ApplyRemainingRates(vismaEntryList);
            double actual = vismaEntryList[42].Value;

            //Assert.
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region RoundToNearest25thTests
        [TestMethod]
        public void RoundToNearest25th_WhenDecimalPartIsZero_AddsZeroToInputNum()
        {

            //Arrange.
            var testNum = 1.0;
            var expected = 1.0;

            //Act.
            double actual = Calculator.RoundToNearest25th(testNum);


            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RoundToNearest25th_WhenDecimalPartIsBelow125_FloorsInputNum()
        {

            //Arrange.
            var testNum = 1.1;
            var expected = 1.0;

            //Act.
            double actual = Calculator.RoundToNearest25th(testNum);


            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RoundToNearest25th_WhenDecimalPartIsBetween125And375_DecimalPartIs25()
        {

            //Arrange.
            var testNum = 1.30;
            var expected = 1.25;

            //Act.
            double actual = Calculator.RoundToNearest25th(testNum);


            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RoundToNearest25th_WhenDecimalPartIsBetween375And625_DecimalPartIs50()
        {

            //Arrange.
            var testNum = 1.42;
            var expected = 1.50;

            //Act.
            double actual = Calculator.RoundToNearest25th(testNum);


            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RoundToNearest25th_WhenDecimalPartIsBetween625And875_DecimalPartIs75()
        {

            //Arrange.
            var testNum = 1.77;
            var expected = 1.75;

            //Act.
            double actual = Calculator.RoundToNearest25th(testNum);


            //Assert.
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RoundToNearest25th_WhenDecimalPartIsAbove875_CeilsInputNum()
        {

            //Arrange.
            var testNum = 1.89;
            var expected = 2.0;

            //Act.
            double actual = Calculator.RoundToNearest25th(testNum);


            //Assert.
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
