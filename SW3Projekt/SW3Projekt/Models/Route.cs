﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("Routes")]
    public class Route
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }

        [ForeignKey("LinkedWorkplace")]
        public int WorkplaceID { get; set; }
        public float Distance { get; set; }
        public float RateValue { get; set; }

        public virtual Employee LinkedEmployee { get; set; }
        public virtual Workplace LinkedWorkplace { get; set; }

        public string WorkplaceMaxPayout
        {
            get
            {
                return LinkedWorkplace.MaxPayout.ToString();
            }
        }
    }
}
