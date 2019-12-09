using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using SW3Projekt.ViewModels;

namespace SW3ProjektTests.Classes
{
    [TestClass]
    public class EmployeeProfileViewModelTests
    {
        [TestMethod]
        public void _sixtyDayHolders_20SickHours_Get20SickHours()
        {
            //Arrange
            PrivateObject TestemployeeProfileViewModel = new PrivateObject(typeof(EmployeeProfileViewModel));
            double testHours = 20.0;
            double expected = testHours;

            //Act
            TestemployeeProfileViewModel.SetProperty("NumberOfSickHours", testHours);

            double actual = (double)TestemployeeProfileViewModel.GetProperty("NumberOfSickHours");

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void _sixtyDayHolders_OverSixtyDayRule_GetValueOverSixtyDayRule()
        {
            //Arrange
            PrivateObject TestemployeeProfileViewModel = new PrivateObject(typeof(EmployeeProfileViewModel));

            string TestWorkplaceName = "TestWorkplace";
            int TestWorkplaceID = 200;
            int TestYear = 2019;

            SixtyDayRow TestsixtyDayRow = new SixtyDayRow(TestWorkplaceName, TestWorkplaceID, TestYear);
            TestsixtyDayRow.TotalForTheYear = 61;
            int expected = TestsixtyDayRow.TotalForTheYear;

            List<SixtyDayRow> TestsixtyDayRowList = new List<SixtyDayRow> { TestsixtyDayRow };

            //Act
            TestemployeeProfileViewModel.SetField("_sixtyDayHolders", TestsixtyDayRowList);

            List<SixtyDayRow> sixtyDayRow = (List<SixtyDayRow>)TestemployeeProfileViewModel.GetField("_sixtyDayHolders");
            int actual = sixtyDayRow[0].TotalForTheYear;

            //Assert
            Assert.AreEqual(expected, actual);
        }

    }
}








/* 
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
    public class M10Tests
    {
        public static int TestID { get; set; } = -1;

        public static int TestEmployeeID { get; set; } = TestID;
        public static int TestWorkplaceID { get; set; }


        public static Employee testEmployee = new Employee()
        {
            Id = TestEmployeeID,
            Firstname = "John",
            Surname = "Johnson",
            PhoneNumber = "12345678"
        };



        [TestCleanup]
        public void TearDown()
        {

        }

        [TestMethod]
        public void TwoRoutesForOneEmployee_()
        {
            //Arrange
            int expected = 2;
            int actual;

            int TestRouteIDOne;
            int TestRouteIDTwo;

            Route testRouteOne = new Route
            {
                Distance = -50,
                EmployeeID = TestEmployeeID,
                WorkplaceID = TestWorkplaceID
            };

            Route testRouteTwo = new Route
            {
                Distance = -75,
                EmployeeID = TestEmployeeID,
                WorkplaceID = TestWorkplaceID
            };

            //    //Add test data
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //Route1
                ctx.Routes.Add(testRouteOne);
                Console.WriteLine("dis");
                
                //Route2
                ctx.Routes.Add(testRouteTwo);

                //Retrieve the id for deletions
                ctx.SaveChanges();
                TestRouteIDOne = ctx.Routes.FirstOrDefault(x => x.Distance == testRouteOne.Distance).Id; //Store the id by comparing distance
                TestRouteIDTwo = ctx.Routes.FirstOrDefault(x => x.Distance == testRouteTwo.Distance).Id; //Store the id by comparing distance
            }

            //Act
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //How many records with the same employee (address) and workplace (destination)
                actual = ctx.Routes.Where(x => x.EmployeeID == TestEmployeeID && x.WorkplaceID == TestWorkplaceID).Count();

                ctx.Routes.Remove(ctx.Routes.Find(TestRouteIDOne));
                ctx.Routes.Remove(ctx.Routes.Find(TestRouteIDTwo));

                ctx.SaveChanges();
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}

     
     
     
     */
