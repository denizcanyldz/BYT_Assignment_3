using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    public class Waiter : Staff
    {
        public static List<Waiter> Waiters = new List<Waiter>();

        public Order Order {  get; set; }
        public double? tipsCollected { get; set; }
        public Waiter(int staffId, string Name, int contactNumber, Order order)
            : base(staffId, Name, contactNumber)
        {
            if (string.IsNullOrEmpty(Name) || contactNumber == null)
            {
                throw new ArgumentException("Invalid waiter details.");
            }

            Order = order;

            Waiters.Add(this);
        }
    }
}
