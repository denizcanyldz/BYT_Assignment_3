using System;
using System.Collections.Generic;
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
        /// Gets or sets the total number of restaurants.
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
        public int RestaurantId { get; set; }

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

        public List<Menu> Menus { get; set; } = new List<Menu>();

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private readonly List<Staff> staffMembers = new List<Staff>();
        
        /// <summary>
        /// Gets a read-only list of staff members associated with the restaurant.
        /// </summary>
        public IReadOnlyList<Staff> StaffMembers => staffMembers.AsReadOnly();

        // -------------------------------
        // Additional Attributes
        // -------------------------------
        public Dictionary<string, string> OpeningHours { get; set; } = new Dictionary<string, string>();

        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Adds a Staff member to the Restaurant.
        /// </summary>
        public void AddStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentException("Staff cannot be null.");

            if (!staffMembers.Contains(staff))
            {
                staffMembers.Add(staff);
                staff.SetRestaurant(this);
            }
        }

        /// <summary>
        /// Removes a Staff member from the Restaurant.
        /// </summary>
        public void RemoveStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentException("Staff cannot be null.");

            if (staffMembers.Contains(staff))
            {
                staffMembers.Remove(staff);
                staff.RemoveRestaurant();
            }
        }

        /// <summary>
        /// Updates a Staff member's association with the Restaurant.
        /// </summary>
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

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Restaurant class with mandatory and optional attributes.
        /// </summary>
        public Restaurant(int restaurantId, string name, string address, string contactNumber, List<Menu> menus, Dictionary<string, string>? openingHours = null)
        {
            RestaurantId = restaurantId;
            Name = name;
            Address = address;
            ContactNumber = contactNumber;
            Menus = menus ?? throw new ArgumentException("Menus list cannot be null.");

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
