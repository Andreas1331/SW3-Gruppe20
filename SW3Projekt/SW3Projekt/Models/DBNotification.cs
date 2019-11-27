using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Models
{
    [Table("Notifications")]
    public class DBNotification : IValidate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public bool IsValidate()
        {
            if (Type == string.Empty || Type == null || Message == string.Empty || Message == null || Date == null)
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
