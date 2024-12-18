using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Inventory
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalInventories = 0;

        /// <summary>
        /// Gets the total number of inventories.
        /// </summary>
        public static int TotalInventories
        {
            get => totalInventories;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("TotalInventories cannot be negative.");
                totalInventories = value;
            }
        }

        private static readonly object inventoryLock = new object();

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Inventory> inventories = new List<Inventory>();

        /// <summary>
        /// Gets a read-only list of all inventories.
        /// </summary>
        public static IReadOnlyList<Inventory> GetAll()
        {
            lock (inventoryLock)
            {
                return inventories.AsReadOnly();
            }
        }

        /// <summary>
        /// Sets the entire inventory list (used during deserialization).
        /// Validates each inventory entry before adding.
        /// </summary>
        /// <param name="loadedInventories">List of inventories to load.</param>
        public static void SetAll(List<Inventory> loadedInventories)
        {
            lock (inventoryLock)
            {
                inventories = loadedInventories ?? new List<Inventory>();
                TotalInventories = 0; // Reset count before re-adding validated inventories

                foreach (var inventory in inventories.ToList()) // Use a copy to allow safe removal
                {
                    try
                    {
                        Console.WriteLine($"Processing Inventory: ID {inventory.InventoryID}");
                        ValidateInventory(inventory);
                        TotalInventories++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding Inventory (ID: {inventory.InventoryID}): {ex.Message}");
                        inventories.Remove(inventory); // Remove invalid inventory
                        throw;
                    }
                }
            }
        }

        // -------------------------------
        // Mandatory Attributes
        // -------------------------------
        private int inventoryID;

        [XmlElement("InventoryID")]
        public int InventoryID
        {
            get => inventoryID;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("InventoryID must be positive.");
                inventoryID = value;
            }
        }

        private DateTime lastRestockDate;

        [XmlElement("LastRestockDate")]
        public DateTime LastRestockDate
        {
            get => lastRestockDate;
            private set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("LastRestockDate cannot be in the future.");
                lastRestockDate = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private readonly List<Ingredient> ingredients = new List<Ingredient>();

        [XmlArray("Ingredients")]
        [XmlArrayItem("Ingredient")]
        public List<Ingredient> Ingredients
        {
            get => ingredients;
            private set
            {
                if (value == null)
                    throw new ArgumentException("Ingredients list cannot be null.");
                ingredients.Clear();
                foreach (var ingredient in value)
                {
                    AddIngredient(ingredient);
                }
            }
        }

        /// <summary>
        /// Adds an ingredient to the inventory.
        /// </summary>
        /// <param name="ingredient">The ingredient to add.</param>
        public void AddIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");

            if (!ingredients.Contains(ingredient))
            {
                ingredients.Add(ingredient);
            }
            else
            {
                throw new ArgumentException("Ingredient already exists in the inventory.");
            }
        }

        /// <summary>
        /// Removes an ingredient from the inventory.
        /// </summary>
        /// <param name="ingredient">The ingredient to remove.</param>
        public void RemoveIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");

            if (!ingredients.Remove(ingredient))
            {
                throw new ArgumentException("Ingredient not found in the inventory.");
            }
        }
        
        /// <summary>
        /// Updates an existing Ingredient in the Inventory.
        /// </summary>
        public void UpdateIngredient(Ingredient oldIngredient, Ingredient newIngredient)
        {
            if (oldIngredient == null || newIngredient == null)
                throw new ArgumentNullException("Ingredient cannot be null.");
            if (!ingredients.Contains(oldIngredient))
                throw new ArgumentException("Old Ingredient not found in the Inventory.");
            if (ingredients.Contains(newIngredient))
                throw new ArgumentException("New Ingredient already exists in the Inventory.");

            // Remove old ingredient
            RemoveIngredient(oldIngredient);

            // Add new ingredient
            AddIngredient(newIngredient);
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        /// <summary>
        /// Gets the total number of items in the inventory.
        /// </summary>
        public int TotalItems => ingredients.Count;

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Inventory class with mandatory attributes.
        /// </summary>
        public Inventory(int inventoryID, DateTime lastRestockDate)
        {
            InventoryID = inventoryID; // Setter includes validation
            LastRestockDate = lastRestockDate;

            // Add to class extent
            lock (inventoryLock)
            {
                if (inventories.Any(inv => inv.InventoryID == inventoryID))
                    throw new ArgumentException($"An Inventory with ID {inventoryID} already exists.");

                inventories.Add(this);
                TotalInventories = inventories.Count;
            }
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Inventory()
        {
            // Required for XML serialization
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Inventory other)
            {
                return InventoryID == other.InventoryID &&
                       LastRestockDate == other.LastRestockDate;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InventoryID, LastRestockDate);
        }

        // -------------------------------
        // Private Validation Method
        // -------------------------------
        /// <summary>
        /// Validates the properties of an Inventory instance.
        /// </summary>
        /// <param name="inventory">The Inventory instance to validate.</param>
        private static void ValidateInventory(Inventory inventory)
        {
            if (inventory.InventoryID <= 0)
                throw new ArgumentException("InventoryID must be positive.");

            if (inventory.LastRestockDate > DateTime.Now)
                throw new ArgumentException("LastRestockDate cannot be in the future.");

            // Validate each Ingredient in the Ingredients list
            foreach (var ingredient in inventory.ingredients)
            {
                Ingredient.ValidateIngredient(ingredient);
            }
        }
    }
}
