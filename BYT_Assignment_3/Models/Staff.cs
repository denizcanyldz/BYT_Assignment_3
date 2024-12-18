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
// -------------------------------
        // Association Attribute
        // -------------------------------
        private Restaurant restaurant;

        /// <summary>
        /// Gets the Restaurant associated with the Staff member.
        /// </summary>
        public Restaurant Restaurant => restaurant;

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Sets the Restaurant for the Staff member.
        /// </summary>
        public void SetRestaurant(Restaurant restaurant)
        {
            if (restaurant == null)
                throw new ArgumentException("Restaurant cannot be null.");

            if(this.restaurant != null && this.restaurant != restaurant)
            {
                // Remove from previous restaurant
                this.restaurant.RemoveStaff(this);
            }

            this.restaurant = restaurant;

            // Ensure reverse connection
            if (!restaurant.StaffMembers.Contains(this))
            {
                restaurant.AddStaff(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Restaurant.
        /// </summary>
        public void RemoveRestaurant()
        {
            if (restaurant != null)
            {
                var oldRestaurant = restaurant;
                restaurant = null;

                // Remove reverse connection
                if (oldRestaurant.StaffMembers.Contains(this))
                {
                    oldRestaurant.RemoveStaff(this);
                }
            }
        }

        // -------------------------------
        // Reflexive Association Attributes
        // -------------------------------
        private Staff? supervisor;
        private Staff? subordinate;

        /// <summary>
        /// Gets the supervisor of the Staff member.
        /// </summary>
        public Staff? Supervisor => supervisor;

        /// <summary>
        /// Gets the subordinate of the Staff member.
        /// </summary>
        public Staff? Subordinate => subordinate;

        // -------------------------------
        // Reflexive Association Methods
        // -------------------------------
        /// <summary>
        /// Assigns a supervisor to the Staff member.
        /// </summary>
        public void AssignSupervisor(Staff newSupervisor)
        {
            if (newSupervisor == null)
                throw new ArgumentException("Supervisor cannot be null.");
            if (newSupervisor == this)
                throw new ArgumentException("Staff member cannot supervise themselves.");
            if (this.subordinate != null && this.subordinate != newSupervisor)
                throw new InvalidOperationException("This Staff member already has a subordinate.");
            if (newSupervisor.supervisor != null && newSupervisor.supervisor != this)
                throw new InvalidOperationException("The new supervisor already has a different supervisor.");

            // Remove existing supervisor if any
            if (this.supervisor != null)
            {
                var oldSupervisor = this.supervisor;
                this.supervisor = null;
                oldSupervisor.RemoveSubordinate();
            }

            // Assign new supervisor
            this.supervisor = newSupervisor;

            // Set reverse connection
            if (newSupervisor.subordinate != this)
            {
                newSupervisor.AssignSubordinate(this);
            }
        }

        /// <summary>
        /// Removes the supervisor from the Staff member.
        /// </summary>
        public void RemoveSupervisor()
        {
            if (this.supervisor != null)
            {
                var oldSupervisor = this.supervisor;
                this.supervisor = null;
                if (oldSupervisor.subordinate == this)
                {
                    oldSupervisor.RemoveSubordinate();
                }
            }
        }

        /// <summary>
        /// Assigns a subordinate to the Staff member.
        /// </summary>
        private void AssignSubordinate(Staff newSubordinate)
        {
            if (newSubordinate == null)
                throw new ArgumentException("Subordinate cannot be null.");
            if (newSubordinate == this)
                throw new ArgumentException("Staff member cannot be subordinate to themselves.");
            if (this.supervisor != null && this.supervisor != newSubordinate)
                throw new InvalidOperationException("This Staff member already has a supervisor.");
            if (newSubordinate.subordinate != null && newSubordinate.subordinate != this)
                throw new InvalidOperationException("The new subordinate already has a different subordinate.");

            // Remove existing subordinate if any
            if (this.subordinate != null)
            {
                var oldSubordinate = this.subordinate;
                this.subordinate = null;
                oldSubordinate.RemoveSupervisor();
            }

            // Assign new subordinate
            this.subordinate = newSubordinate;

            // Set reverse connection
            if (newSubordinate.supervisor != this)
            {
                newSubordinate.AssignSupervisor(this);
            }
        }

        /// <summary>
        /// Removes the subordinate from the Staff member.
        /// </summary>
        private void RemoveSubordinate()
        {
            if (this.subordinate != null)
            {
                var oldSubordinate = this.subordinate;
                this.subordinate = null;
                if (oldSubordinate.supervisor == this)
                {
                    oldSubordinate.RemoveSupervisor();
                }
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Staff class with mandatory and optional attributes.
        /// </summary>
        protected Staff(int staffID, string name, string? contactNumber = null, Restaurant? restaurant = null)
        {
            StaffID = staffID;
            Name = name;
            ContactNumber = contactNumber;

            // Add to class extent
            staffMembers.Add(this);
            TotalStaff = staffMembers.Count;

            if (restaurant != null)
            {
                SetRestaurant(restaurant);
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        protected Staff() { }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Staff other)
            {
                return StaffID == other.StaffID &&
                       Name == other.Name &&
                       ContactNumber == other.ContactNumber &&
                       Restaurant.Equals(other.Restaurant);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StaffID, Name, ContactNumber, Restaurant);
        }
    }
}