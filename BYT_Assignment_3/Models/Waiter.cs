namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Waiter : Staff
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
        private bool tipsCollected;

        /// <summary>
        /// Gets or sets whether tips have been collected.
        /// </summary>
        public bool TipsCollected
        {
            get => tipsCollected;
            set
            {
                tipsCollected = value;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Waiter class with mandatory and optional attributes.
        /// </summary>
        public Waiter(int staffID, string name, string? contactNumber = null, Restaurant? restaurant = null, bool tipsCollected = false)
            : base(staffID, name, contactNumber, restaurant)
        {
            TipsCollected = tipsCollected;

            // Add to waiter extent
            waiters.Add(this);
            TotalWaiters = waiters.Count;
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Waiter() : base() { }

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