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
       
        
        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private readonly List<MenuItem> menuItems = new List<MenuItem>();
        
        /// <summary>
        /// Gets a read-only list of menu items in the menu.
        /// </summary>
        public IReadOnlyList<MenuItem> MenuItems => menuItems.AsReadOnly();
        
        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Adds a MenuItem to the Menu.
        /// </summary>
        public void AddMenuItem(MenuItem menuItem)
        {
            if(menuItem == null)
                throw new ArgumentException("MenuItem cannot be null.");
            
            if(!menuItems.Contains(menuItem))
            {
                menuItems.Add(menuItem);
                menuItem.SetMenu(this);
            }
        }
        
        /// <summary>
        /// Removes a MenuItem from the Menu.
        /// </summary>
        public void RemoveMenuItem(MenuItem menuItem)
        {
            if(menuItem == null)
                throw new ArgumentException("MenuItem cannot be null.");
            
            if(menuItems.Contains(menuItem))
            {
                menuItems.Remove(menuItem);
                menuItem.RemoveMenu();
            }
        }
        
        /// <summary>
        /// Updates a MenuItem in the Menu.
        /// </summary>
        public void UpdateMenuItem(MenuItem oldItem, MenuItem newItem)
        {
            if(oldItem == null || newItem == null)
                throw new ArgumentException("MenuItem cannot be null.");
            
            if(!menuItems.Contains(oldItem))
                throw new ArgumentException("Old MenuItem not found in the Menu.");
            
            // Remove old item
            RemoveMenuItem(oldItem);
            
            // Add new item
            AddMenuItem(newItem);
        }
        
        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Menu class with mandatory attributes.
        /// </summary>
        public Menu(int menuId, List<MenuItem> menuItems)
        {
            MenuId = menuId;

            if (menuItems == null)
                throw new ArgumentException("MenuItems list cannot be null.");

            foreach (var item in menuItems)
            {
                AddMenuItem(item);
            }

            // Add to class extent and update total
            menus.Add(this);
            TotalMenus = menus.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Menu()
        {
            // Initialize the menuItems list
        }

        
         
        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Menu other)
            {
                return MenuId == other.MenuId;
                // Excluding menuItems for simplicity
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MenuId);
        }
    }
}
