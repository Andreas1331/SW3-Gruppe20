using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt
{
    class User
    {
        //User information
        public int DatabaseID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //User settings
        public int SixtyDayThreshold { get; set; } = 50;

    }
}
