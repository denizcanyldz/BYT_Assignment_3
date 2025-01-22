using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Restaurant
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalRestaurants = 0;

        /// <summary>
        /// Gets the total number of restaurants.
        /// </summary>
        public static int TotalRestaurants
        {
            get => totalRestaurants;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalRestaurants cannot be negative.");
                totalRestaurants = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Restaurant> restaurants = new List<Restaurant>();

        /// <summary>
        /// Gets a read-only list of all restaurants.
        /// </summary>
        public static IReadOnlyList<Restaurant> GetAll()
        {
            return restaurants.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire restaurant list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Restaurant> loadedRestaurants)
        {
            if (loadedRestaurants == null)
                throw new ArgumentNullException(nameof(loadedRestaurants), "Loaded restaurants list cannot be null.");
            restaurants = loadedRestaurants;
            TotalRestaurants = restaurants.Count;

            // Update Staff extent based on loaded restaurants
            List<Staff> allStaff = new List<Staff>();
            foreach (var restaurant in restaurants)
            {
                allStaff.AddRange(restaurant.StaffMembers);
            }
            Staff.SetAll(allStaff);
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int RestaurantId { get; private set; }

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

        private string address;
        public string Address
        {
            get => address;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address cannot be null or empty.");
                address = value;
            }
        }

        private string contactNumber;
        public string ContactNumber
        {
            get => contactNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ContactNumber cannot be null or empty.");
                contactNumber = value;
            }
        }

        public List<Menu> Menus { get; private set; } = new List<Menu>();

        // -------------------------------
        // Aggregation Attributes
        // -------------------------------
        private readonly List<Staff> staffMembers = new List<Staff>();

        /// <summary>
        /// Gets a read-only list of staff members associated with the restaurant.
        /// </summary>
        public IReadOnlyList<Staff> StaffMembers => staffMembers.AsReadOnly();

        // -------------------------------
        // Additional Attributes
        // -------------------------------
        public Dictionary<string, string> OpeningHours { get; private set; } = new Dictionary<string, string>();

        public List<Order> Orders { get; private set; } = new List<Order>();
        public List<Payment> Payments { get; private set; } = new List<Payment>();
        public List<Feedback> Feedbacks { get; private set; } = new List<Feedback>();
        public List<Reservation> Reservations { get; private set; } = new List<Reservation>();

        // -------------------------------
        // One-to-One Relationship Attribute
        // -------------------------------
        private Inventory? inventory;

        /// <summary>
        /// Gets the Inventory managed by the Restaurant.
        /// </summary>
        public Inventory? Inventory
        {
            get => inventory;
            private set
            {
                inventory = value;
            }
        }

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Adds a Staff member to the Restaurant.
        /// </summary>
        /// <param name="staff">The Staff member to add.</param>
        public void AddStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentException("Staff cannot be null.");
            if (!staffMembers.Contains(staff))
            {
                staffMembers.Add(staff);
                staff.SetRestaurantInternal(this);
            }
        }

        /// <summary>
        /// Removes a Staff member from the Restaurant.
        /// </summary>
        /// <param name="staff">The Staff member to remove.</param>
        public void RemoveStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentException("Staff cannot be null.");

            if (staffMembers.Contains(staff))
            {
                staffMembers.Remove(staff);
                staff.RemoveRestaurantInternal();
            }
        }

        /// <summary>
        /// Updates a Staff member's association with the Restaurant.
        /// </summary>
        /// <param name="oldStaff">The Staff member to be replaced.</param>
        /// <param name="newStaff">The new Staff member to associate.</param>
        public void UpdateStaff(Staff oldStaff, Staff newStaff)
        {
            if (oldStaff == null || newStaff == null)
                throw new ArgumentException("Staff cannot be null.");
            if (!staffMembers.Contains(oldStaff))
                throw new ArgumentException("Old Staff member not found in the Restaurant.");

            // Remove old staff
            RemoveStaff(oldStaff);

            // Add new staff
            AddStaff(newStaff);
        }

        /// <summary>
        /// Adds an Order to the Restaurant.
        /// </summary>
        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order cannot be null.");
            Orders.Add(order);
        }

        /// <summary>
        /// Adds a Payment to the Restaurant.
        /// </summary>
        public void AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentException("Payment cannot be null.");
            Payments.Add(payment);
        }

        /// <summary>
        /// Adds a Feedback to the Restaurant.
        /// </summary>
        public void AddFeedback(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentException("Feedback cannot be null.");
            Feedbacks.Add(feedback);
        }

        /// <summary>
        /// Adds a Reservation to the Restaurant.
        /// </summary>
        public void AddReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentException("Reservation cannot be null.");
            Reservations.Add(reservation);
        }

        /// <summary>
        /// Assigns an Inventory to the Restaurant.
        /// </summary>
        public void AssignInventory(Inventory inventory)
        {
            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            if (this.inventory != null)
                throw new InvalidOperationException("This Restaurant already has an Inventory assigned.");

            if (inventory.Restaurant != null)
                throw new InvalidOperationException("This Inventory is already assigned to another Restaurant.");

            this.inventory = inventory;
            inventory.SetRestaurantInternal(this);
        }

        /// <summary>
        /// Unassigns the Inventory from the Restaurant.
        /// </summary>
        public void UnassignInventory()
        {
            if (this.inventory == null)
                return;

            var oldInventory = this.inventory;
            this.inventory = null;
            oldInventory.SetRestaurantInternal(null);
        }

        /// <summary>
        /// Internal method to set Inventory without causing recursion.
        /// </summary>
        internal void SetInventoryInternal(Inventory inventory)
        {
            this.inventory = inventory;
        }

        /// <summary>
        /// Internal method to remove Inventory without causing recursion.
        /// </summary>
        internal void RemoveInventoryInternal(Inventory inventory)
        {
            if (this.inventory == inventory)
                this.inventory = null;
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Restaurant class with mandatory and optional attributes.
        /// </summary>
        public Restaurant(int restaurantId, string name, string address, string contactNumber, List<Menu> menus, Dictionary<string, string>? openingHours = null)
        {
            if (restaurantId <= 0)
                throw new ArgumentException("RestaurantId must be positive.", nameof(restaurantId));
            RestaurantId = restaurantId;
            Name = name;
            Address = address;
            ContactNumber = contactNumber;
            Menus = menus ?? throw new ArgumentException("Menus list cannot be null.", nameof(menus));

            if (openingHours != null)
            {
                OpeningHours = openingHours;
            }

            // Add to class extent and update total
            restaurants.Add(this);
            TotalRestaurants = restaurants.Count;
        }

        /// <summary>
        /// Initializes a new instance of the Restaurant class with mandatory attributes only.
        /// </summary>
        public Restaurant(int restaurantId, string name, string address, string contactNumber)
            : this(restaurantId, name, address, contactNumber, new List<Menu>(), null)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the Restaurant class with mandatory and optional attributes, including initial Staff members.
        /// Ensures that the Restaurant has at least one Staff member upon creation.
        /// </summary>
        /// <param name="restaurantId">Unique identifier for the Restaurant.</param>
        /// <param name="name">Name of the Restaurant.</param>
        /// <param name="address">Address of the Restaurant.</param>
        /// <param name="contactNumber">Contact number of the Restaurant.</param>
        /// <param name="menus">List of Menus offered by the Restaurant.</param>
        /// <param name="initialStaff">One or more initial Staff members to associate with the Restaurant.</param>
        /// <param name="openingHours">Optional opening hours of the Restaurant.</param>
        public Restaurant(
            int restaurantId,
            string name,
            string address,
            string contactNumber,
            List<Menu> menus,
            IEnumerable<Staff> initialStaff,
            Dictionary<string, string>? openingHours = null)
            : this(restaurantId, name, address, contactNumber, menus, openingHours)
        {
            if (initialStaff == null || !initialStaff.Any())
                throw new ArgumentException("At least one Staff member must be provided.", nameof(initialStaff));

            foreach (var staff in initialStaff)
            {
                AddStaff(staff);
            }
        }
        
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Restaurant()
        {
            // Initialize Menus and OpeningHours
            Menus = new List<Menu>();
            OpeningHours = new Dictionary<string, string>();
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Restaurant other)
            {
                return RestaurantId == other.RestaurantId &&
                       Name == other.Name &&
                       Address == other.Address &&
                       ContactNumber == other.ContactNumber;
                // Excluding StaffMembers and Menus for simplicity
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RestaurantId, Name, Address, ContactNumber);
        }
    }
}
