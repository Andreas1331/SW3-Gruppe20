using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.DatabaseDir
{
    public class Database : DbContext
    {
        //YOU OPENED THE WRONG FILE BITCH!
        // DO NOT FUCKING CHANGE ANYTHING! RIGHT?
        public Database() : base(@"server=boxengaming.dk;database=PrototypeDB;uid=0122;password=Nice24")
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<CollectiveAgreement> CollectiveAgreements { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
        //public DbSet<VismaEntry> VismaEntries { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
    }
}
