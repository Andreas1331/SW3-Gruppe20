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
        // Constructor vi anvender hver gang vi skal forbinde til sql serveren
        public Database() : base(@"server=boxengaming.dk;database=PrototypeDB;uid=0122;password=Nice24")
        {
        }

        //Disse properties svarer til de tables vi har i databasen
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CollectiveAgreement> CollectiveAgreements { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }
        public DbSet<VismaEntry> VismaEntries { get; set; }
        public DbSet<Workplace> Workplaces { get; set; }
        public DbSet<DBNotification> Notifications { get; set; }
    }
}
