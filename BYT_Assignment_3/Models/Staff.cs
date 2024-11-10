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