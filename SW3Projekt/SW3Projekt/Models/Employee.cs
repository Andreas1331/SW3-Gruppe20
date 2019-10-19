using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class Employee
    {
        public int DatabaseID { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public int EmployeeID { get; set; }
        public int PhoneNumber{ get; set; }
        public string Email { get; set; }
        public DateTime DateHired { get; set; }

        [NotMapped]
        public List<Route> Routes;
        [NotMapped]
        public List<Timesheet> Timesheets;

        [NotMapped]
        public string Fullname
        {
            get
            {
                return $"{Firstname} {Surname}";
            }
        }
    }
}
