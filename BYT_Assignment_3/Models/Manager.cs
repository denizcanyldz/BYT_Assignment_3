using BYT_Assignment_3.Interfaces;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Manager : Staff, IManager, IRole
    {
        public string RoleName => "Manager";

        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalManagers = 0;

        /// <summary>
        /// Gets or sets the total number of managers.
        /// </summary>
        public static int TotalManagers
        {
            get => totalManagers; 
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalManagers cannot be negative.");
                totalManagers = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Manager> managers = new List<Manager>();

        /// <summary>
        /// Gets a read-only list of all managers.
        /// </summary>
        public static IReadOnlyList<Manager> GetAll()
        {
            return managers.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire manager list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Manager> loadedManagers)
        {
            if (loadedManagers == null)
                throw new ArgumentNullException(nameof(loadedManagers), "Loaded managers list cannot be null.");

            managers = loadedManagers ?? new List<Manager>();
            TotalManagers = managers.Count;
            Staff.SetAll(new List<Staff>(managers));
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? department;

        public string? Department
        {
            get => department;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 100)
                    throw new ArgumentException("Department length cannot exceed 100 characters.");
                department = value;
            }
        }

        // -------------------------------
        // Multi-Aspect Inheritance Attributes
        // -------------------------------
        // Example: Managers can have fixed roles like "HR" or "Operations"
        // These roles are assigned using the AssignFixedRole method from Staff

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Manager class with mandatory and optional attributes.
        /// </summary>
        public Manager(int staffID, string name, string? contactNumber = null, Restaurant? restaurant = null, string? department = null)
            : base(staffID, name, contactNumber, restaurant)
        {
            Department = department;
            // Assign fixed role if needed
            AssignFixedRole(this); // Assuming Manager itself represents a fixed role

            // Add to manager extent
            managers.Add(this);
            TotalManagers = managers.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Manager() : base() 
        {
            // Assign fixed role if needed
            AssignFixedRole(this); // Assuming Manager itself represents a fixed role
        }

        // -------------------------------
        // IManager Implementation
        // -------------------------------
        public void AssignStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentNullException(nameof(staff), "Staff cannot be null.");
            // Implement logic to assign staff to the manager
            // For example, setting the manager as the supervisor
            staff.AssignSupervisor(this);
        }

        public void RemoveStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentNullException(nameof(staff), "Staff cannot be null.");
            // Implement logic to remove staff from the manager
            // For example, removing the manager as the supervisor
            if (staff.Supervisor == this)
            {
                staff.RemoveSupervisor();
            }
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Manager other)
            {
                return base.Equals(other) &&
                       Department == other.Department;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Department);
        }
    }
}
