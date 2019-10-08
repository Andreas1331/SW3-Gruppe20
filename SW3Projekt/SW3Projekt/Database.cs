using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt
{
    class Database : DbContext
    {

        public Database() : base(@"server=boxengaming.dk;database=SampleDB;uid=0122;password=Nice24")
        {

        }
    }
}
