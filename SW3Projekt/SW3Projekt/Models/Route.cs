using System;
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
        public int WorkplaceID { get; set; }
        public double Distance { get; set; }
        public double RateValue { get; set; }
    }
}
