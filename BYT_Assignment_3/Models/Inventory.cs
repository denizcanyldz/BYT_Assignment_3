using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Inventory
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalInventories = 0;

        /// <summary>
        /// Gets or sets the total number of inventories.
        /// </summary>
        public static int TotalInventories
        {
            get => totalInventories;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalInventories cannot be negative.");
                totalInventories = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Inventory> inventories = new List<Inventory>();

        /// <summary>
        /// Gets a read-only list of all inventories.
        /// </summary>
        public static IReadOnlyList<Inventory> GetAll()
        {
            return inventories.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire inventory list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Inventory> loadedInventories)
        {
            inventories = loadedInventories ?? new List<Inventory>();
            TotalInventories = inventories.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int InventoryID { get; private set; }
        

        private DateTime lastRestockDate;

        public DateTime LastRestockDate
        {
            get => lastRestockDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("LastRestockDate cannot be in the future.");
                lastRestockDate = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<Ingredient> ingredients = new List<Ingredient>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<Ingredient> Ingredients => ingredients.AsReadOnly();

        /// <summary>
        /// Adds an ingredient to the inventory.
        /// </summary>
        public void AddIngredient(Ingredient ingredient)
        {
            if(ingredient == null)
                throw new ArgumentException("Ingredient cannot be null.");
            ingredients.Add(ingredient);
        }

        /// <summary>
        /// Removes an ingredient from the inventory.
        /// </summary>
        public void RemoveIngredient(Ingredient ingredient)
        {
            if(ingredient == null || !ingredients.Contains(ingredient))
                throw new ArgumentException("Ingredient not found.");
            ingredients.Remove(ingredient);
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public int TotalItems => ingredients.Count;

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Inventory class with mandatory attributes.
        /// </summary>
        public Inventory(int inventoryID, DateTime lastRestockDate)
        {
            InventoryID = inventoryID;
            LastRestockDate = lastRestockDate;

            // Add to class extent
            inventories.Add(this);
            TotalInventories = inventories.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Inventory()
        {
           
        }
    }
}