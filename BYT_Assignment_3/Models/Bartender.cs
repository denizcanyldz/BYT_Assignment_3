using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    public class Bartender : Staff
    {
        public static List<Bartender> Bartenders = new List<Bartender>();
        public Bartender(int staffId, string Name, int ContactNumber)
            : base(staffId, Name, ContactNumber)
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new ArgumentException("Invalid bartender details.");
            }

            Bartenders.Add(this);
        }
    }
}
