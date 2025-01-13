namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class WaiterBartender : Bartender, IWaiter
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalWaiterBartenders = 0;

        /// <summary>
        /// Gets or sets the total number of WaiterBartenders.
        /// </summary>
        public static int TotalWaiterBartenders
        {
            get => totalWaiterBartenders;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalWaiterBartenders cannot be negative.");
                totalWaiterBartenders = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<WaiterBartender> waiterBartenders = new List<WaiterBartender>();

        /// <summary>
        /// Gets a read-only list of all WaiterBartender instances.
        /// </summary>
        public static IReadOnlyList<WaiterBartender> GetAll()
        {
            return waiterBartenders.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire waiterBartender list (used during deserialization).
        /// </summary>
        public static void SetAll(List<WaiterBartender> loadedWaiterBartenders)
        {
            waiterBartenders = loadedWaiterBartenders ?? new List<WaiterBartender>();
            TotalWaiterBartenders = waiterBartenders.Count;
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private double? bonus;

        /// <summary>
        /// Gets or sets the bonus for the WaiterBartender. Must be non-negative.
        /// </summary>
        public double? Bonus
        {
            get => bonus;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Bonus cannot be negative.");
                bonus = value;
            }
        }

        // -------------------------------
        // IWaiter.TipsCollected
        // (Separate internal field for tips)
        // -------------------------------
        private double tipsCollected;

        double IWaiter.TipsCollected
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
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the WaiterBartender class with mandatory and optional attributes.
        /// </summary>
        /// <param name="staffID">The unique identifier for the staff member.</param>
        /// <param name="name">The name of the staff member.</param>
        /// <param name="bonus">The bonus assigned to the staff member, if any.</param>
        /// <param name="contactNumber">The contact number for the staff member.</param>
        public WaiterBartender(int staffID, string name, double? bonus = null, string? contactNumber = null)
            : base(staffID, name, contactNumber)
        {
            CurrentRole = StaffRole.WaiterBartender;
            Bonus = bonus;

            // Add to WaiterBartender extent
            waiterBartenders.Add(this);
            TotalWaiterBartenders = waiterBartenders.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public WaiterBartender() : base()
        {
            CurrentRole = StaffRole.WaiterBartender;
        }

        // -------------------------------
        // IWaiter Methods
        // -------------------------------
        void IWaiter.TakeOrder()
        {
            Console.WriteLine($"{Name} (WaiterBartender) is taking an order.");
        }

        void IWaiter.ServeOrder()
        {
            Console.WriteLine($"{Name} (WaiterBartender) is serving an order.");
        }

        void IWaiter.ProcessPayment()
        {
            Console.WriteLine($"{Name} (WaiterBartender) is processing a payment.");
        }





        /// <summary>
        /// Determines whether the specified object is equal to the current WaiterBartender.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is WaiterBartender other)
            {
                return StaffID == other.StaffID &&
                       Name == other.Name &&
                       ContactNumber == other.ContactNumber &&
                       Bonus == other.Bonus;
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(StaffID, Name, ContactNumber, Bonus);
        }


    }
}