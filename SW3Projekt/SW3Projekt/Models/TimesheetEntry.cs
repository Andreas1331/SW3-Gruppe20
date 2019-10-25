﻿using System;
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
        public int EmployeeID { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public string ProjectID { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public float BreakTime { get; set; }

        [NotMapped]
        public List<VismaEntry> vismaEntries;

        [NotMapped]
        public ComboBoxItem SelectedRouteComboBoxItem { get; set; }
        [NotMapped] 
        public string KmTextBox { get; set; }
        [NotMapped] 
        public ComboBoxItem SelectedTypeComboBoxItem { get; set; }
        [NotMapped] 
        public string DietTextBox { get; set; }
        [NotMapped] 
        public ComboBoxItem SelectedDisplacedHoursComboBoxItem { get; set; }
        [NotMapped] 
        public string ValueTextbox { get; set; }
        [NotMapped] 
        public ComboBoxItem SelectedMiscellaneousComboBoxItem { get; set; }
        [NotMapped] 
        public string ValueMiscellaneousTextBox { get; set; }

    }
}
