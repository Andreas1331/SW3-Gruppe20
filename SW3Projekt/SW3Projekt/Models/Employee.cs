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
        [NotMapped]
        private int _phoneNumber;
        [NotMapped]
        public string PhoneNumber { 
        get
            {
                if (_phoneNumber == 0)
                {
                    return "";
                } else
                {
                    return _phoneNumber.ToString();
                }
            }
        set
            {
                if (int.TryParse(value, out _phoneNumber)) { }
            }
        }
        public string Email { get; set; }
        public DateTime DateHired { get; set; } = DateTime.Now;
        public string DateHiredToString
        {
            get {
                return DateHired.ToString("dd/MM/yyyy");
            }
        }
        public bool IsFired { get; set; }
        public string IsFiredStr { 
            get
            {
                return (IsFired ? "Ikke ansat" : "Ansat");
            } 
        }

        public virtual List<Route> Routes { get; set; }

        public List<Timesheet> Timesheets;
        

        [NotMapped]
        public string Fullname
        {
            get
            {
                return $"{Firstname} {Surname}";
            }
        }

        public override string ToString()
        {
            return $"{Fullname} #{Id}";
        }
    }
}
