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
        /// Gets or sets the total number of menus.
        /// </summary>
        public static int TotalMenus
        {
            get => totalMenus;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalMenus cannot be negative.");
                totalMenus = value;
            }
        }
        
        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Menu> menus = new List<Menu>();
        
        
        /// <summary>
        /// Gets a read-only list of all menus.
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
            if(loadedMenus == null)
                throw new ArgumentNullException(nameof(loadedMenus), "Loaded menus list cannot be null.");
            menus = loadedMenus ?? new List<Menu>();
            TotalMenus = menus.Count;
            Staff.TotalStaff = Staff.GetAll().Count;
        }
        
        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        private int menuId;
        public int MenuId{
            get => menuId;
            set {
                if(value <= 0)
                    throw new ArgumentException("MenuId must be greater that zero.");
                menuId = value;
            }
        }
        private List<MenuItem> menuItems;
        public List<MenuItem> MenuItems{
            get => menuItems;
            set{
                if(value == null || value.Count == 0)
                    throw new ArgumentException("MenuItems cannot be null or empty.");
                menuItems = value;
            }
        }
        
        
        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Menu class with mandatory and optional attributes.
        /// </summary>
        /// <param name="menuId">The unique identifier for the menu.</param>
        /// <param name="menuItems">The list of menu items that the menu contains.</param>
        public Menu(int menuId, List<MenuItem> menuItems)
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
        public Menu()
        {
            MenuItems = new List<MenuItem>();
            
            // Add to the menus extent and update total
            menus.Add(this);
            TotalMenus = menus.Count;
        }
        
        /// <summary>
        /// Determines whether the specified object is equal to the current Menu.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Menu other)
            {
                return MenuId == other.MenuId &&
                       // Excluding MenuItems collection to simplify equality
                       // Alternatively, implement sequence equality if needed
                       true;
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(MenuId);
        }
    }
}
