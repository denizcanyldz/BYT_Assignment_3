using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    public abstract class Staff
    {
        public int staffId { get; set; }
        public string name { get; set; }
        public int contactNumber { get; set; }
        public Staff supervisor { get; set; } //association

        public Staff(int StaffId, string Name, int ContactNumber)
        {
            staffId = StaffId;
            name = Name;
            contactNumber = ContactNumber;
        }
    }
}
