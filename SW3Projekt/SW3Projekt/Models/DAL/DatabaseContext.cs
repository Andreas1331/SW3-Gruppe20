using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base(@"server=boxengaming.dk;database=PrototypeDB;uid=0122;password=Nice24")
        {
            Console.WriteLine("Bool: " + this.Configuration.LazyLoadingEnabled);
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }
}
