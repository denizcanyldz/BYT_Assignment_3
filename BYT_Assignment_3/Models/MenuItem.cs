using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class MenuItem
    {
        // -------------------------------
        // Static Attributes
        // -------------------------------
        private static int totalMenuItems = 0;
        public static double TaxPercentage { get; set; } = 1.2; // Represents a 20% tax

        /// <summary>
        /// Gets or sets the total number of menu items.
        /// </summary>
        public static int TotalMenuItems
        {
            get => totalMenuItems;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalMenuItems cannot be negative.");
                totalMenuItems = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<MenuItem> menuItems = new List<MenuItem>();

        /// <summary>
        /// Gets a read-only list of all menu items.
        /// </summary>
        public static IReadOnlyList<MenuItem> GetAll()
        {
            return menuItems.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire menu item list (used during deserialization).
        /// </summary>
        public static void SetAll(List<MenuItem> loadedMenuItems)
        {
            menuItems = loadedMenuItems ?? new List<MenuItem>();
            TotalMenuItems = menuItems.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int MenuItemID { get; private set; }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty.");
                name = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? description;

        public string? Description
        {
            get => description;
            set
            {
                if(!string.IsNullOrEmpty(value) && value.Length > 300)
                    throw new ArgumentException("Description length cannot exceed 300 characters.");
                description = value;
            }
        }

        private bool isAvailable;

        public bool IsAvailable
        {
            get => isAvailable;
            set
            {
                isAvailable = value;
            }
        }

        // -------------------------------
        // Additional Attribute
        // -------------------------------
        private double basePrice;

        /// <summary>
        /// Gets or sets the base price of the menu item.
        /// </summary>
        public double BasePrice
        {
            get => basePrice;
            set
            {
                if(value < 0)
                    throw new ArgumentException("BasePrice cannot be negative.");
                basePrice = value;
            }
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        [XmlIgnore] // Prevent serialization as it's derived
        public double PriceAfterTax => Math.Round(basePrice * TaxPercentage, 2);

        public int TotalIngredients => ingredients.Count;

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<Ingredient> ingredients = new List<Ingredient>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<Ingredient> Ingredients => ingredients.AsReadOnly();

        /// <summary>
        /// Adds an ingredient to the menu item.
        /// </summary>
        public void AddIngredient(Ingredient ingredient)
        {
            if(ingredient == null)
                throw new ArgumentException("Ingredient cannot be null.");
            ingredients.Add(ingredient);
        }

        /// <summary>
        /// Removes an ingredient from the menu item.
        /// </summary>
        public void RemoveIngredient(Ingredient ingredient)
        {
            if(ingredient == null || !ingredients.Contains(ingredient))
                throw new ArgumentException("Ingredient not found.");
            ingredients.Remove(ingredient);
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the MenuItem class with mandatory and optional attributes.
        /// </summary>
        public MenuItem(int menuItemID, string name, double basePrice, string? description = null, bool isAvailable = true)
        {
            MenuItemID = menuItemID;
            Name = name;
            BasePrice = basePrice;
            Description = description;
            IsAvailable = isAvailable;

            // Add to class extent
            menuItems.Add(this);
            TotalMenuItems = menuItems.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public MenuItem() { }
    }
}
