namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Waiter : Staff, IWaiter
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalWaiters = 0;

        /// <summary>
        /// Gets or sets the total number of waiters.
        /// </summary>
        public static int TotalWaiters
        {
            get => totalWaiters;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalWaiters cannot be negative.");
                totalWaiters = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Waiter> waiters = new List<Waiter>();

        /// <summary>
        /// Gets a read-only list of all waiters.
        /// </summary>
        public static IReadOnlyList<Waiter> GetAll()
        {
            return waiters.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire waiter list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Waiter> loadedWaiters)
        {
            waiters = loadedWaiters ?? new List<Waiter>();
            TotalWaiters = waiters.Count;
            Staff.SetAll(new List<Staff>(waiters));
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private double tipsCollected;

        /// <summary>
        /// Gets or sets whether tips have been collected.
        /// </summary>
        public double TipsCollected
        {
            get => tipsCollected;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Tips cannot be negative.");
                tipsCollected = value;
            }
        }

        // -------------------------------
        // Waiter Order Association
        // -------------------------------
        private List<Order> orders = new List<Order>();
        public IReadOnlyList<Order> Orders => orders.AsReadOnly();

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            if (orders.Contains(order))
                throw new ArgumentException("Order is already associated with this waiter.");
            if (order.Waiter != null && order.Waiter != this)
                throw new ArgumentException("Order is already associated with another waiter.");

            orders.Add(order);
            order.SetWaiter(this, false); // Ensure reverse association
        }

        public void RemoveOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            if (!orders.Contains(order))
                throw new ArgumentException("Order is not associated with this waiter.");

            orders.Remove(order);
            order.RemoveWaiter(false); // Ensure reverse disassociation
        }

        public void UpdateOrder(Order oldOrder, Order newOrder)
        {
            if (oldOrder == null || newOrder == null)
                throw new ArgumentNullException("Orders cannot be null.");
            if (!orders.Contains(oldOrder))
                throw new ArgumentException("Old order is not associated with this waiter.");
            if (orders.Contains(newOrder))
                throw new ArgumentException("New order is already associated with this waiter.");

            RemoveOrder(oldOrder);
            AddOrder(newOrder);
        }


        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Waiter class with mandatory and optional attributes.
        /// </summary>
        public Waiter(int staffID, string name, string? contactNumber = null, Restaurant? restaurant = null, double initialTips = 0)
            : base(staffID, name, contactNumber, restaurant)
        {
            CurrentRole = StaffRole.Waiter;
            TipsCollected = initialTips;

            // Add to waiter extent
            waiters.Add(this);
            TotalWaiters = waiters.Count;
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Waiter() : base() { }


        // -------------------------------
        // IWaiter Methods
        // -------------------------------
        public void TakeOrder()
        {
            Console.WriteLine($"{Name} (Waiter) is taking an order.");
        }

        public void ServeOrder()
        {
            Console.WriteLine($"{Name} (Waiter) is serving an order.");
        }

        public void ProcessPayment()
        {
            Console.WriteLine($"{Name} (Waiter) is processing a payment.");
        }
        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Waiter other)
            {
                return base.Equals(other) &&
                       TipsCollected == other.TipsCollected;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), TipsCollected);
        }
    }
}