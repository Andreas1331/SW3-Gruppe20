using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    public class CollectiveAgreement
    {
        public int DatabaseID { get; set; }
        public string Name { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public List<Rate> Rates;

    }
}
