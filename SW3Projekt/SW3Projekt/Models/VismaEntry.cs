﻿using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("VismaEntries")]
    public class VismaEntry : IValidate
    {
        public int Id { get; set; }
        public int VismaID { get; set; }
        public string Comment { get; set;  }
        public double Value { get; set; }
        public double RateValue { get; set; }
        public int RateID { get; set; }
        public int TimesheetEntryID { get; set; }

        public virtual Rate LinkedRate { get; set; }

        public bool IsValidate()
        {
            if (Id < 0 || VismaID < 0 || Comment == null || Value < 0 || RateValue < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
