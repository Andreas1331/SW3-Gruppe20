using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("CollectiveAgreements")]
    public class CollectiveAgreement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = false;
        public bool IsArchived { get; set; } = false;

        public virtual List<Rate> Rates { get; set; } = new List<Rate>();
    }
}
