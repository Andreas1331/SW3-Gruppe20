using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("Employees")]
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Firstname { get; set; } = "";
        public string Surname { get; set; }
        public string PhoneNumber{ get; set; }
        public string Email { get; set; }
        public DateTime DateHired { get; set; }

        public virtual List<Route> Routes { get; set; }
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
