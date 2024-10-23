using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    public class Chef : Staff
    {
        public static List<Chef> Chefs = new List<Chef>();

        public List<MenuItem> MenuItemsPrepared { get; set; } = new List<MenuItem>(); //association with menu item

        public string Specialty { get; set; }
        public Chef(int staffId, string Name, int contactNumber, string specialty)
            : base(staffId, Name, contactNumber)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(specialty) )
            {
                throw new ArgumentException("Invalid chef details.");
            }


            Specialty = specialty;

            Chefs.Add(this);
        }

        public void PrepareMenuItem(MenuItem menuItem)
        {
            if (menuItem != null)
            {
                MenuItemsPrepared.Add(menuItem);
                menuItem.PreparedBy = this;
            }
        }
    }
}
