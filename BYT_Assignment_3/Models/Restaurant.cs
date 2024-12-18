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
        /// Gets or sets the total number of WaiterBartenders.
        /// </summary>
        public static int TotalRestaurants
        {
            get => totalRestaurants;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalWaiterBartenders cannot be negative.");
                totalRestaurants = value;
            }
        }
        
        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Restaurant> restaurants = new List<Restaurant>();
        
        
        /// <summary>
        /// Gets a read-only list of all restaurant items.
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
            restaurants = loadedRestaurants ?? new List<Restaurant>();
            TotalRestaurants = restaurants.Count;
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
        public List<Menu> Menus { get; set; }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        [XmlIgnore] // Prevent direct serialization of the collection
        public Dictionary<string, string> OpeningHours { get; set; } = new Dictionary<string, string>();

        
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        
        
        // -------------------------------
        // Methods to Add Orders, Payments, Feedbacks, Reservations
        // -------------------------------
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
        /// Initializes a new instance of the Restaurant class with mandatory and optional attributes.
        /// </summary>
        /// <param name="restaurantId">The unique identifier for the restaurant.</param>
        /// <param name="name">The name of the restaurant.</param>
        /// <param name="address">The address of the restaurant.</param>
        /// <param name="contactNumber">The contact number for the restaurant.</param>
        /// <param name="openingHours">The dictionary of opening hours, with day names as keys and opening times as values.</param>
        /// <param name="menus">The list of menus of the restaurant.</param>
        public Restaurant(int restaurantId, string name, string address, string contactNumber, List<Menu> menus, Dictionary<string, string> openingHours = null)
        {
            RestaurantId = restaurantId;
            Name = name;
            Address = address;
            ContactNumber = contactNumber;
            Menus = menus;
            
            // Initialize OpeningHours if provided
            if (openingHours != null)
            {
                OpeningHours = openingHours;
            }
            
            // Add to the restaurants extent and update total
            restaurants.Add(this);
            TotalRestaurants = restaurants.Count;
        }
        
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Restaurant(){}
        
        /// <summary>
        /// Determines whether the specified object is equal to the current Restaurant.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Restaurant other)
            {
                return RestaurantId == other.RestaurantId &&
                       Name == other.Name &&
                       Address == other.Address &&
                       ContactNumber == other.ContactNumber;
                // Excluding Menus and OpeningHours collections to simplify equality
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(RestaurantId, Name, Address, ContactNumber);
        }

        private List<Staff> _staff = new List<Staff>();

        public IReadOnlyList<Staff> GetStaff()
        {
            return _staff.AsReadOnly();
        }

        public void AddStaff(Staff staff)
        {
            if (staff == null)
            {
                throw new ArgumentException("Staff cant be null");
            }

            if (_staff.Contains(staff))
                return;
            
            _staff.Add(staff);
            if (staff.GetRestaurant() != this)
            {
                staff.AddRestaurant(this);
            }
        }

        public void RemoveStaff(Staff staff)
        {
            if (staff == null)
                throw new ArgumentException("Feedback cannot be null.");

            if (!_staff.Contains(staff))
                throw new KeyNotFoundException("The specified feedback is not associated with this customer.");

            _staff.Remove(staff);

            if (staff.GetRestaurant() == this)
            {
                staff.RemoveRest(this);
            }
        }

        public void ModifyStaff(Staff oldStaff, Staff newStaff)
        {
            if (oldStaff == null || newStaff == null)
                throw new ArgumentException("Staff members cannot be null.");

            if (!_staff.Contains(oldStaff))
                throw new InvalidOperationException("The specified old staff member is not associated with this restaurant.");

            RemoveStaff(oldStaff);

            AddStaff(newStaff);
        }

    }
}