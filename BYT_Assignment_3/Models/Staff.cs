using System.Runtime.InteropServices.Marshalling;
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

        /// <summary>
        /// Reflex Association Implementation
        /// </summary>
        
        private Staff _supervisor;

        private List<Staff> _slaves = new List<Staff>();

        public void startManaging(Staff slave)
        {
            if (slave == null)
                throw new ArgumentException("Staff to be managed cannot be null.");

            if (slave == this)
                throw new InvalidOperationException("A staff member cannot supervise itself.");

            if (!_slaves.Contains(slave))
            {
                _slaves.Add(slave);
            }
            else
            {
                return;
            }

            if (slave.GetSupervisor() == this)
                return;
            else
                slave.setSupervisor(this);
        }

        public void setSupervisor(Staff supervisor)
        {

            if (this._supervisor == supervisor)
                return;

            if (supervisor == null)
            {
                _supervisor = null;
                return;
            }

            if (this._supervisor != null)
                throw new InvalidOperationException("This staff member already has a supervisor.");
            
            if (supervisor == this)
                throw new InvalidOperationException("A staff member cannot supervise itself.");

            this._supervisor = supervisor;
            supervisor.startManaging(this);
        }


        public void removeManagedStaff(Staff slave)
        {
            if (slave == null)
                throw new ArgumentException("The staff member to be removed cannot be null.");

            if (!_slaves.Contains(slave))
                throw new InvalidOperationException("The specified staff member is not being managed by this supervisor.");

            _slaves.Remove(slave);
            slave.setSupervisor(null);
        }

        public void removeSupervisor()
        {
            if (this._supervisor == null)
                throw new InvalidOperationException("This staff member does not have a supervisor to remove.");
            if (!this._supervisor._slaves.Contains(this))
                throw new ArgumentException("This staff member is not being supervised b");

            this._supervisor._slaves.Remove(this);
            this._supervisor = null;
        }

        public void modifyManagedStaff(Staff oldSlave, Staff newSlave)
        {
            if (oldSlave == null || newSlave == null)
                throw new ArgumentException("Staff members cannot be null.");

            if (!_slaves.Contains(oldSlave))
                throw new InvalidOperationException("The specified staff member to be replaced is not being managed by this supervisor.");

            removeManagedStaff(oldSlave);
            startManaging(newSlave);
        }

        public void modifySupervisor(Staff newSupervisor)
        {
            if (newSupervisor == null)
                throw new ArgumentException("The new supervisor cannot be null.");

            if (this._supervisor == newSupervisor)
                throw new InvalidOperationException("This staff member is already supervised by the specified supervisor.");

            if (newSupervisor == this)
                throw new InvalidOperationException("A staff member cannot supervise itself.");

            if (this._supervisor != null)
            {
                removeSupervisor();
            }

            setSupervisor(newSupervisor);
        }

        /// <summary>
        /// Returns a readonly list of managed staff members.
        /// </summary>
        public IReadOnlyList<Staff> GetManagedStaff()
        {
            return _slaves.AsReadOnly();
        }

        /// <summary>
        /// Returns the supervisor of the staff member.
        /// </summary>
        public Staff? GetSupervisor()
        {
            return _supervisor;
        }


        private Restaurant _restaurant;
        public Restaurant? GetRestaurant()
        {
            return _restaurant;
        }
        
        public void AddRestaurant(Restaurant rest)
        {
            if (rest == null)
            {
                throw new ArgumentException("Restaurant cannot be null.");
            }

            if (this._restaurant == rest)
                return;

            _restaurant = rest;
            if (!rest.GetStaff().Contains(this))
            {
                rest.AddStaff(this);
            }
        }

        public void RemoveRest(Restaurant rest)
        {
            if (rest == null)
                throw new ArgumentException("Restaurant cannot be null.");

            if (_restaurant != rest)
                throw new KeyNotFoundException("The specified restaurant is not associated with this staff.");

            _restaurant = null;

            if (rest.GetStaff().Contains(this))
            {
                rest.RemoveStaff(this);
            }
        }

        public void ModifyRestaurant(Restaurant newRestaurant)
        {
            if (newRestaurant == null)
                throw new ArgumentException("New restaurant cannot be null.");

            if (_restaurant != null)
            {
                _restaurant.RemoveStaff(this);
            }

            AddRestaurant(newRestaurant);
        }

    }
}