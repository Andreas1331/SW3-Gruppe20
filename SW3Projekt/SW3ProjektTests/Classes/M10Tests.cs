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

        public static Workplace testWorkplace = new Workplace
        {
            Name = "UnitTestWorkplace",
            MaxPayout = 100,
            Abbreviation = "dd",
            Address = "Adress",
            Archived = false,
        };


        [TestInitialize]
        public void Setup()
        {
            //Add test data
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //Employee
                    Employee dbEmployee = ctx.Employees.Find(TestEmployeeID);
                if (dbEmployee != null)
                    ctx.Employees.Remove(dbEmployee);
                ctx.Employees.Add(testEmployee);

                //Workplace
                ctx.Workplaces.Add(testWorkplace);
                ctx.SaveChanges();

                TestWorkplaceID = ctx.Workplaces.FirstOrDefault(x => x.Name == testWorkplace.Name).Id; //Store the id by comparing workplace name
            }
        }

        [TestCleanup]
        public void TearDown()
        {
            //Remove test data
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                ctx.Employees.Remove(ctx.Employees.Find(TestEmployeeID));
                ctx.Workplaces.Remove(ctx.Workplaces.Find(TestWorkplaceID));
                ctx.SaveChanges();
            }
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

            //Add test data
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //Route1
                ctx.Routes.Add(testRouteOne);

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

                //Cleanup
                ctx.Routes.Remove(ctx.Routes.Find(TestRouteIDOne));
                ctx.Routes.Remove(ctx.Routes.Find(TestRouteIDTwo));

                ctx.SaveChanges();
            }

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
