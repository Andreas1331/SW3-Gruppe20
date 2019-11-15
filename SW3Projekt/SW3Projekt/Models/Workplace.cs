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

        private string _name;
        public string Name {
            get { return _name; }
            set { 
                if (value != null && value.Length > 50) {
                    _name = value.Substring(0, 50);
                    return;
                }
                _name = (value == null) ? "" : value; 
            }
        }

        private string _abbreviation;
        public string Abbreviation {
            get { return _abbreviation; }
            set {
                if (value != null && value.Length > 50)
                {
                    _abbreviation = value.Substring(0, 50);
                    return;
                }
                _abbreviation = (value == null) ? "" : value;
            }
        }

        private string _address;
        public string Address {
            get { return _address; }
            set {
                if (value != null && value.Length > 50)
                {
                    _address = value.Substring(0, 50);
                    return;
                }
                _address = (value == null) ? "" : value;
            }
        }
        public bool Archived { get; set; }
        public double MaxPayout { get; set; }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, Abbreviation);
        }
    }
}
