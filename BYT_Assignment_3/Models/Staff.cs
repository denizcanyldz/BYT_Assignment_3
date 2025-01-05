using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    [XmlInclude(typeof(Chef))]
    [XmlInclude(typeof(Bartender))]
    [XmlInclude(typeof(Waiter))]
    [XmlInclude(typeof(WaiterBartender))]
    [XmlInclude(typeof(Manager))]
    public abstract class Staff
    {
        // -------------------------------
        // Class/Static Attributes
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
        public int StaffID { get; private set; }

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
        // Association Attributes
        // -------------------------------
        private Restaurant? restaurant;

        /// <summary>
        /// Gets the Restaurant associated with the Staff member.
        /// </summary>
        public Restaurant? Restaurant => restaurant;

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

            if (restaurant != null)
            {
                SetRestaurantInternal(restaurant);
            }

            // Add to class extent
            staffMembers.Add(this);
            TotalStaff = staffMembers.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        protected Staff() { }

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Sets the Restaurant for the Staff member, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="newRestaurant">The Restaurant to associate with.</param>
        internal void SetRestaurantInternal(Restaurant? newRestaurant)
        {
            if (this.restaurant == newRestaurant)
                return; // No change needed

            // Remove from current restaurant if exists
            if (this.restaurant != null)
            {
                this.restaurant.RemoveStaff(this);
            }

            this.restaurant = newRestaurant;

            // Add to the new restaurant's staff list if not already present
            if (newRestaurant != null && !newRestaurant.StaffMembers.Contains(this))
            {
                newRestaurant.AddStaff(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Restaurant, maintaining bidirectional consistency.
        /// </summary>
        internal void RemoveRestaurantInternal()
        {
            if (this.restaurant != null)
            {
                var oldRestaurant = this.restaurant;
                this.restaurant = null;

                // Remove this staff from the old restaurant's staff list
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

        /// <summary>
        /// Gets the supervisor of the Staff member.
        /// </summary>
        public Staff? Supervisor => supervisor;

        private List<Staff> subordinates = new List<Staff>();

        /// <summary>
        /// Gets a read-only list of subordinates supervised by the Staff member.
        /// </summary>
        [XmlIgnore]
        public IReadOnlyList<Staff> Subordinates => subordinates.AsReadOnly();

        // -------------------------------
        // Reflexive Association Methods
        // -------------------------------
        /// <summary>
        /// Assigns a supervisor to the Staff member.
        /// </summary>
        /// <param name="newSupervisor">The Staff member to assign as supervisor.</param>
        public void AssignSupervisor(Staff newSupervisor)
        {
            if (newSupervisor == null)
                throw new ArgumentNullException(nameof(newSupervisor), "Supervisor cannot be null.");
            if (newSupervisor == this)
                throw new ArgumentException("Staff member cannot supervise themselves.");
            if (IsCircularSupervision(newSupervisor))
                throw new InvalidOperationException("Assigning this supervisor creates a circular supervisory relationship.");

            // Remove existing supervisor if any
            if (this.supervisor != null)
            {
                this.supervisor.subordinates.Remove(this);
            }

            // Assign new supervisor
            this.supervisor = newSupervisor;

            // Add this staff to the new supervisor's subordinates list if not already present
            if (!newSupervisor.subordinates.Contains(this))
            {
                newSupervisor.subordinates.Add(this);
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

                // Remove this staff from the old supervisor's subordinates list
                if (oldSupervisor.subordinates.Contains(this))
                {
                    oldSupervisor.subordinates.Remove(this);
                }
            }
        }

        /// <summary>
        /// Adds a subordinate to the Staff member.
        /// </summary>
        /// <param name="subordinate">The Staff member to add as subordinate.</param>
        public void AddSubordinate(Staff subordinate)
        {
            if (subordinate == null)
                throw new ArgumentNullException(nameof(subordinate), "Subordinate cannot be null.");
            if (subordinate == this)
                throw new ArgumentException("Staff member cannot be subordinate to themselves.");
            if (IsCircularSupervision(subordinate))
                throw new InvalidOperationException("Adding this subordinate creates a circular supervisory relationship.");

            // Assign this staff as the supervisor of the subordinate
            subordinate.AssignSupervisor(this);
        }

        /// <summary>
        /// Removes a subordinate from the Staff member.
        /// </summary>
        /// <param name="subordinate">The Staff member to remove as subordinate.</param>
        public void RemoveSubordinate(Staff subordinate)
        {
            if (subordinate == null)
                throw new ArgumentNullException(nameof(subordinate), "Subordinate cannot be null.");
            if (!subordinates.Contains(subordinate))
                throw new ArgumentException("Subordinate not found.");

            // Remove the supervisor from the subordinate
            subordinate.RemoveSupervisor();
        }

        /// <summary>
        /// Checks if assigning the new supervisor creates a circular supervisory relationship.
        /// </summary>
        /// <param name="potentialSupervisor">The Staff member to check.</param>
        /// <returns>True if a circular relationship is detected; otherwise, false.</returns>
        private bool IsCircularSupervision(Staff potentialSupervisor)
        {
            Staff? currentSupervisor = potentialSupervisor;
            while (currentSupervisor != null)
            {
                if (currentSupervisor == this)
                    return true;
                currentSupervisor = currentSupervisor.supervisor;
            }
            return false;
        }

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
                       ((Restaurant == null && other.Restaurant == null) || (Restaurant?.RestaurantId == other.Restaurant?.RestaurantId));
                // Excluding Supervisor and Subordinates for simplicity
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StaffID, Name, ContactNumber, Restaurant?.RestaurantId);
        }

        // -------------------------------
        // RemoveStaff Method
        // -------------------------------
        /// <summary>
        /// Removes a Staff member from the class extent.
        /// </summary>
        /// <param name="staff">The Staff member to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when staff is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the staff member is not found.</exception>
        public static void RemoveStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentNullException(nameof(staff), "Staff member cannot be null.");

            if (!staffMembers.Contains(staff))
                throw new ArgumentException("Staff member not found.", nameof(staff));

            // Handle supervisor-subordinate relationships
            if (staff.Supervisor != null)
            {
                staff.Supervisor.subordinates.Remove(staff);
                staff.supervisor = null;
            }

            // Reassign or remove subordinates
            foreach (var subordinate in staff.subordinates.ToList())
            {
                subordinate.RemoveSupervisor();
                // Alternatively, you could assign them to another supervisor
            }

            // Remove associations with Restaurant
            staff.RemoveRestaurantInternal();

            // Remove from staffMembers list and update TotalStaff
            staffMembers.Remove(staff);
            TotalStaff = staffMembers.Count;
        }
    }
}
