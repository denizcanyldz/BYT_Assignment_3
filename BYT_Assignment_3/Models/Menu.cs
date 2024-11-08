namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Menu
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalMenus = 0;
        
        /// <summary>
        /// Gets or sets the total number of WaiterBartenders.
        /// </summary>
        public static int TotalMenus
        {
            get => totalMenus;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalWaiterBartenders cannot be negative.");
                totalMenus = value;
            }
        }
        
        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Menu> menus = new List<Menu>();
        
        
        /// <summary>
        /// Gets a read-only list of all menu items.
        /// </summary>
        public static IReadOnlyList<Menu> GetAll()
        {
            return menus.AsReadOnly();
        }
        
        /// <summary>
        /// Sets the entire menu list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Menu> loadedMenus)
        {
            menus = loadedMenus ?? new List<Menu>();
            TotalMenus = menus.Count;
            Staff.TotalStaff = Staff.GetAll().Count;
        }
        
        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int MenuId { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        
        
        // -------------------------------
        // Optional Attributes
        // -------------------------------
        
        
        
        
        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Menu class with mandatory and optional attributes.
        /// </summary>
        /// <param name="menuId">The unique identifier for the menu.</param>
        /// <param name="menuItems">The list of menu items that the menu contains.</param>
        public  Menu(int menuId, List<MenuItem> menuItems)
        {
            MenuId = menuId;
            MenuItems = menuItems;
            
            // Add to the menus extent and update total
            menus.Add(this);
            TotalMenus = menus.Count;
        }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Menu(){}
    }
}