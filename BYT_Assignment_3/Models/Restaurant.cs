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
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public List<Menu> Menus { get; set; }
        
        // -------------------------------
        // Optional Attributes
        // -------------------------------
        public Dictionary<string, string> OpeningHours { get; set; } = new Dictionary<string, string>();

        
        
        
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
    }
}