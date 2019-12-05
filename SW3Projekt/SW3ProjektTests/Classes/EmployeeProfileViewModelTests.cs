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
        public static int TestID { get; set; } = -2;

        public static int TestEmployeeID { get; set; } = TestID;
        public static int TestWorkplaceID { get; set; }
        public static int TestCollectiveAgreementID { get; set; }
        public static int TestRouteID { get; set; }
        public static int TestrateID { get; set; }
        public static int TestTimesheetEntryID { get; set; }

        public static Employee TestEmployee = new Employee()
        {
            Id = TestEmployeeID,
            Firstname = "John",
            Surname = "Johnson",
            PhoneNumber = "12345678"
        };

        public static Workplace TestWorkplace = new Workplace
        {
            Name = "UnitTestWorkplace",
            MaxPayout = 100,
            Abbreviation = "dd",
            Address = "Adress",
            Archived = false,
        };

        public static CollectiveAgreement TestCollectiveAgreement = new CollectiveAgreement
        {
            Name = "UnitTestCollectiveAgreement",
            IsActive = false,
            IsArchived = false
        };

        public static Rate testrate = new Rate
        {
            Type = "Kørsel",
            Name = "UnitTestRate",
        };

        public static TimesheetEntry testTimesheetentry = new TimesheetEntry()
        {
            EmployeeID = TestEmployeeID,
            Comment = "UnitTesttimesheetEntry",
            StartTime = new DateTime(),
            EndTime = new DateTime(),
            Date = new DateTime(),
            ProjectID = "TestProject"
        };


        //        public static VismaEntry testVismaEntry = new VismaEntry()
        //        {
        //            LinkedRate = testrate
        //        };



        public static Route testRoute = new Route
        {
            Distance = 50
        };

        [TestInitialize]
        public void Setup()
        {
            //Add test data (ROUND 1)
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //Employee
                Employee dbEmployee = ctx.Employees.Find(TestEmployeeID);
                if (dbEmployee != null)
                    ctx.Employees.Remove(dbEmployee);
                ctx.Employees.Add(TestEmployee);

                //Workplace
                ctx.Workplaces.Add(TestWorkplace);

                //Collective agreement
                ctx.CollectiveAgreements.Add(TestCollectiveAgreement);

                ctx.SaveChanges();

                TestWorkplaceID = ctx.Workplaces.FirstOrDefault(x => x.Name == TestWorkplace.Name).Id;
                TestCollectiveAgreementID = ctx.CollectiveAgreements.FirstOrDefault(x => x.Name == TestCollectiveAgreement.Name).Id;
            }

            //Add test data (ROUND 2) with link to data from ROUND1
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                testrate.CollectiveAgreementID = TestCollectiveAgreementID;
                ctx.Rates.Add(testrate);

                //testTimesheetentry.WorkplaceID = TestWorkplaceID;
                //ctx.TimesheetEntries.Add(testTimesheetentry);

                ctx.SaveChanges();
                TestrateID = ctx.Rates.FirstOrDefault(x => x.Name == testrate.Name).Id;
                //TestTimesheetEntryID = ctx.TimesheetEntries.FirstOrDefault(x => x.Comment == testTimesheetentry.Comment).Id;
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            //Remove test data
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //ctx.TimesheetEntries.Remove(ctx.TimesheetEntries.Find(TestTimesheetEntryID));
                ctx.Employees.Remove(ctx.Employees.Find(TestEmployeeID));
                //ctx.Workplaces.Remove(ctx.Workplaces.Find(TestWorkplaceID));
                //ctx.Rates.Remove(ctx.Rates.Find(TestrateID));
                //ctx.CollectiveAgreements.Remove(ctx.CollectiveAgreements.Find(TestCollectiveAgreementID));
                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void _sixtyDayHolders_OverSixtyDayRule_Warning()
        {
            //            //Arrange
            //            int expected = 2019;

            //            //Act
            //            EmployeeProfileViewModel testEmployeeProfileViewModel = new EmployeeProfileViewModel(testEmployee);

            //            //int actual = testEmployeeProfileViewModel.SixtyDayCollection.FirstOrDefault<SixtyDayRow>(x => x.Year == 2019).Year;
            //            //Assert
            //            //Assert.AreEqual(expected, actual);
            //        }

            //        public void _sixtyDayHolders_OverSixtyDayRule_NoWarning()
            //        {

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
