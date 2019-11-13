using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SW3Projekt.Models
{
    [Table("TimesheetEntries")]
    public class TimesheetEntry
    {
        public int Id { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string ProjectID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double BreakTime { get; set; } = 0.5f;
        public int WorkplaceID { get; set; }
        public virtual Workplace LinkedWorkplace { get; set; }
        public virtual List<VismaEntry> vismaEntries { get; set; } = new List<VismaEntry>();

        [NotMapped]
        public Timesheet timesheet { get; set; }
        [NotMapped]
        public string SelectedRouteComboBoxItem { get; set; }
        [NotMapped] 
        public double KmTextBox { get; set; }
        [NotMapped]
        public double DriveRate { get; set; }
        [NotMapped] 
        public string SelectedTypeComboBoxItem { get; set; }
        [NotMapped] 
        public string DietTextBox { get; set; }


        public TimesheetEntry()
        {
            var defaultStartTime = new DateTime();
            defaultStartTime = defaultStartTime.AddHours(7);
            StartTime = defaultStartTime;

            var defaultEndTime = new DateTime();
            defaultEndTime = defaultEndTime.AddHours(15);
            EndTime = defaultEndTime;
        }

    }
}
