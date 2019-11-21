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


        #region ApplyRemainingRatesTest
        [TestMethod]
        public void ApplyRemainingRates_WhenSaveAsMoneyIsFalse_DoesNotMultiplyValue()
        {

            //Arrange.
            var vismaEntryList = Enumerable.Range(0, 100)
                                           .Select(n => new VismaEntry() {Value = n, RateValue = n, LinkedRate = new Rate() { SaveAsMoney = (n == 42) } })
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
                                           .Select(n => new VismaEntry() { Value = n, RateValue = n, LinkedRate = new Rate() { SaveAsMoney = (n != 42) } })
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
