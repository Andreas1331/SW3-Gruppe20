using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("Workplaces")]
    public class Workplace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Address { get; set; }
        public bool Archived { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, Abbreviation);
        }
    }
}
