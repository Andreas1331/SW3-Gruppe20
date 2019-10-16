using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class TimesheetEntry
    {
        public int DatabaseID { get; set; }
        public int ProjectID { get; set; }
        public string Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int BreakTime { get; set; }
        public string Comment { get; set; }

    }
}
