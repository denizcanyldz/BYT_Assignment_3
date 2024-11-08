namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class WaiterBartender : Staff
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
        private string? section;
        private string? licenseNumber;
        private double? bonus;

        /// <summary>
        /// Gets or sets the section for WaiterBartender if assigned as a Waiter.
        /// </summary>
        public string? Section
        {
            get => section;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 100)
                    throw new ArgumentException("Section length cannot exceed 100 characters.");
                section = value;
            }
        }

        /// <summary>
        /// Gets or sets the license number for WaiterBartender if assigned as a Bartender.
        /// </summary>
        public string? LicenseNumber
        {
            get => licenseNumber;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ArgumentException("LicenseNumber length cannot exceed 50 characters.");
                licenseNumber = value;
            }
        }

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
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the WaiterBartender class with mandatory and optional attributes.
        /// </summary>
        /// <param name="staffID">The unique identifier for the staff member.</param>
        /// <param name="name">The name of the staff member.</param>
        /// <param name="section">The section assigned if the staff member is acting as a waiter.</param>
        /// <param name="licenseNumber">The license number if the staff member is acting as a bartender.</param>
        /// <param name="shift">The shift assigned to the staff member.</param>
        /// <param name="bonus">The bonus assigned to the staff member, if any.</param>
        public WaiterBartender(int staffID, string name, string? section = null, string? licenseNumber = null, string? shift = null, double? bonus = null)
            : base(staffID, name, shift)
        {
            Section = section;
            LicenseNumber = licenseNumber;
            Bonus = bonus;

            // Add to WaiterBartender extent
            waiterBartenders.Add(this);
            TotalWaiterBartenders = waiterBartenders.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public WaiterBartender() : base() { }
    }
}