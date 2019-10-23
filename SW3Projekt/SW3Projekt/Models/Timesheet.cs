using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class Timesheet
    {
        public int DatabaseID { get; set; }
        public int EmployeeID { get; set; }
        public int WeekNumber { get; set; }
        public int Year{ get; set; }

        public List<TimesheetEntry> TSEntries;

        public void AddVismaEnties(List<Rate> rates) { 
            
        }

    }
}
