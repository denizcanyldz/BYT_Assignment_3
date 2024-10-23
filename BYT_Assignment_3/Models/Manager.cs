using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    public class Manager : Staff
    {
        public static List<Manager> Managers = new List<Manager>();

        public Manager(int staffId, string Name, int ContactNumber) 
            : base(staffId, Name, ContactNumber)
        {
            if (string.IsNullOrEmpty(Name) || contactNumber == null)
            {
                throw new ArgumentException("Invalid waiter details.");
            }

            Managers.Add(this);
        }
    }
}
