using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("TimesheetEntries")]
    public class TimesheetEntry
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public int VismaID { get; set; }
        public string Comment { get; set; }
        public float Value { get; set; }
        public float RateValue { get; set; }
        public float RateMoney { get; set; }
        public string ProjectID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public float BreakTime { get; set; }
        public int RateID { get; set; }

    }
}
