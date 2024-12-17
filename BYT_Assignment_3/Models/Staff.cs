using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    [XmlInclude(typeof(Chef))]
    [XmlInclude(typeof(Bartender))]
    [XmlInclude(typeof(Waiter))]
    [XmlInclude(typeof(WaiterBartender))]
    public abstract class Staff
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalStaff = 0;

        /// <summary>
        /// Gets or sets the total number of staff members.
        /// </summary>
        public static int TotalStaff
        {
            get => totalStaff;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalStaff cannot be negative.");
                totalStaff = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Staff> staffMembers = new List<Staff>();

        /// <summary>
        /// Gets a read-only list of all staff members.
        /// </summary>
        public static IReadOnlyList<Staff> GetAll()
        {
            return staffMembers.AsReadOnly();
        }

        /// <summary>
        /// Adds a staff member to the staff list.
        /// </summary>
        protected void AddStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentException("Staff cannot be null.");
            staffMembers.Add(staff);
            TotalStaff = staffMembers.Count;
        }

        /// <summary>
        /// Removes a staff member from the staff list.
        /// </summary>
        public static void RemoveStaff(Staff staff)
        {
            if (staff == null || !staffMembers.Contains(staff))
                throw new ArgumentException("Staff member not found.");
            staffMembers.Remove(staff);
            TotalStaff = staffMembers.Count;
        }

        /// <summary>
        /// Sets the entire staff list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Staff> loadedStaff)
        {
            staffMembers = loadedStaff ?? new List<Staff>();
            TotalStaff = staffMembers.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int StaffID { get; set; }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty.");
                name = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? contactNumber;

        public string? ContactNumber
        {
            get => contactNumber;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ArgumentException("ContactNumber length cannot exceed 50 characters.");
                contactNumber = value;
            }
        }

        // ---- Association: Staff -> Restaurant ----
        private Restaurant? restaurant;  // null if not currently employed

        /// <summary>
        /// Read-only property to see which Restaurant this Staff belongs to.
        /// </summary>
        public Restaurant? Restaurant => restaurant;

        /// <summary>
        /// Internal method to set the Restaurant reference. 
        /// We keep it internal or private so only our code can call it.
        /// </summary>
        internal void SetRestaurant(Restaurant? newRestaurant)
        {
            // Prevent infinite recursion:
            if (restaurant == newRestaurant) return;

            // If this Staff was previously employed by some other Restaurant, remove it there.
            if (restaurant != null)
            {
                restaurant.RemoveStaff(this);
            }

            // Assign the new restaurant
            restaurant = newRestaurant;

            // If newRestaurant is not null, ensure the reverse reference is also created
            if (restaurant != null && !restaurant.GetStaff().Contains(this))
            {
                restaurant.AddStaff(this);
            }
        }
        // -------------------------------
        // Reflexive Association Attributes
        // -------------------------------
        private Staff? supervisor; // A staff member can have at most one supervisor
        private List<Staff> supervisees = new List<Staff>(); // A staff member can supervise many others

        /// <summary>
        /// Gets the supervisor of the staff.
        /// </summary>
        public Staff? Supervisor => supervisor;

        /// <summary>
        /// Gets a read-only list of supervisees.
        /// </summary>
        public IReadOnlyList<Staff> Supervisees => supervisees.AsReadOnly();

        // -------------------------------
        // Reflexive Association Methods
        // -------------------------------

        /// <summary>
        /// Sets a supervisor for this staff member. Ensures no self-reference.
        /// </summary>
        public void SetSupervisor(Staff newSupervisor)
        {
            if (newSupervisor == null)
                throw new ArgumentNullException(nameof(newSupervisor), "Supervisor cannot be null.");

            if (newSupervisor == this)
                throw new InvalidOperationException("A staff member cannot supervise themselves.");

            // Remove existing supervisor reference
            RemoveSupervisor();

            // Set new supervisor
            supervisor = newSupervisor;

            // Add this staff member to the new supervisor's supervisees list
            if (!newSupervisor.supervisees.Contains(this))
            {
                newSupervisor.supervisees.Add(this);
            }
        }

        /// <summary>
        /// Removes the current supervisor reference.
        /// </summary>
        public void RemoveSupervisor()
        {
            if (supervisor != null)
            {
                supervisor.supervisees.Remove(this); // Remove from old supervisor's supervisees
                supervisor = null; // Clear supervisor reference
            }
        }

        /// <summary>
        /// Adds a supervisee to this staff member's supervisee list.
        /// </summary>
        public void AddSupervisee(Staff supervisee)
        {
            if (supervisee == null)
                throw new ArgumentNullException(nameof(supervisee), "Supervisee cannot be null.");

            if (supervisee == this)
                throw new InvalidOperationException("A staff member cannot supervise themselves.");

            if (!supervisees.Contains(supervisee))
            {
                supervisees.Add(supervisee);
                supervisee.SetSupervisor(this); // Ensure reverse reference
            }
        }

        /// <summary>
        /// Removes a supervisee from this staff member's supervisee list.
        /// </summary>
        public void RemoveSupervisee(Staff supervisee)
        {
            if (supervisee == null)
                throw new ArgumentNullException(nameof(supervisee), "Supervisee cannot be null.");

            if (supervisees.Contains(supervisee))
            {
                supervisees.Remove(supervisee);
                supervisee.RemoveSupervisor(); // Clear reverse reference
            }
        }
        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Staff class with mandatory and optional attributes.
        /// </summary>
        protected Staff(int staffID, string name, string? contactNumber = null)
        {
            StaffID = staffID;
            Name = name;
            ContactNumber = contactNumber;

            // Add to class extent
            AddStaff(this);
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        protected Staff() { }
    }
}