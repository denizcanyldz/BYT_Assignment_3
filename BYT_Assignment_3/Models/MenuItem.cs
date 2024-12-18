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
        

        private int menuItemID;
        public int MenuItemID{
            get=> menuItemID;
            set {
                if(value <= 0)
                    throw new ArgumentException("MenuItemId must be greater that zero.");
                menuItemID = value;
            }
        }

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
        // Additional Attributes
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

        private int calories;

        /// <summary>
        /// Gets or sets the number of calories in the menu item.
        /// </summary>
        public int Calories
        {
            get => calories;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Calories cannot be negative.");
                calories = value;
            }
        }

        private double discountPrice;

        /// <summary>
        /// Gets or sets the discount price of the menu item.
        /// </summary>
        public double DiscountPrice
        {
            get => discountPrice;
            set
            {
                if (value < 0)
                    throw new ArgumentException("DiscountPrice cannot be negative.");
                discountPrice = value;
            }
        }

        private int preparationTime;

        /// <summary>
        /// Gets or sets the preparation time of the menu item in minutes.
        /// </summary>
        public int PreparationTime
        {
            get => preparationTime;
            set
            {
                if (value < 0)
                    throw new ArgumentException("PreparationTime cannot be negative.");
                preparationTime = value;
            }
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public double TotalPriceAfterTax => Math.Round(BasePrice * TaxPercentage, 2);

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
            if (ingredient == null)
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
            if (ingredients.Contains(ingredient))
                throw new ArgumentException("Ingredient already exists in the menu item.");

            ingredients.Add(ingredient);
            ingredient.AddMenuItem(this); // Update reverse connection
        }

        /// <summary>
        /// Removes an ingredient from the menu item.
        /// </summary>
        public void RemoveIngredient(Ingredient ingredient)
        {
            if (ingredient == null)
                throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
            if (!ingredients.Contains(ingredient))
                throw new ArgumentException("Ingredient not found in the menu item.");

            ingredients.Remove(ingredient);
            ingredient.RemoveMenuItem(this); // Update reverse connection
        }
        
        /// <summary>
        /// Updates an existing ingredient with a new ingredient in the menu item.
        /// </summary>
        /// <param name="existingIngredient">The ingredient to be replaced.</param>
        /// <param name="newIngredient">The new ingredient to replace with.</param>
        public void UpdateIngredient(Ingredient existingIngredient, Ingredient newIngredient)
        {
            if (existingIngredient == null)
                throw new ArgumentNullException(nameof(existingIngredient), "Existing ingredient cannot be null.");
            if (newIngredient == null)
                throw new ArgumentNullException(nameof(newIngredient), "New ingredient cannot be null.");
            if (!ingredients.Contains(existingIngredient))
                throw new ArgumentException("Existing ingredient not found in the menu item.");
            if (ingredients.Contains(newIngredient))
                throw new ArgumentException("New ingredient already exists in the menu item.");

            // Remove the existing ingredient
            RemoveIngredient(existingIngredient);

            // Add the new ingredient
            AddIngredient(newIngredient);
        }
        
        /// <summary>
        /// Sets the entire ingredients list, replacing any existing associations.
        /// </summary>
        /// <param name="newIngredients">The new list of ingredients.</param>
        public void SetAllIngredients(List<Ingredient> newIngredients)
        {
            // Remove existing associations
            foreach (var ingredient in new List<Ingredient>(ingredients))
            {
                RemoveIngredient(ingredient);
            }

            // Add new associations
            if (newIngredients != null)
            {
                foreach (var ingredient in newIngredients)
                {
                    AddIngredient(ingredient);
                }
            }
        }

        // -------------------------------
        // Association Attributes
        // -------------------------------
        private Menu menu;

        /// <summary>
        /// Gets the Menu associated with the MenuItem.
        /// </summary>
        public Menu Menu => menu;

        // -------------------------------
        // Association Methods
        // -------------------------------
        internal void SetMenu(Menu menu)
        {
            if (menu == null)
                throw new ArgumentException("Menu cannot be null.");
            
            if(this.menu != null && this.menu != menu)
            {
                // Remove from previous menu
                this.menu.RemoveMenuItem(this);
            }
            
            this.menu = menu;
            
            // Ensure reverse connection
            if (!menu.MenuItems.Contains(this))
            {
                menu.AddMenuItem(this);
            }
        }

        internal void RemoveMenu()
        {
            if (menu != null)
            {
                var oldMenu = menu;
                menu = null;
                
                // Remove reverse connection
                if (oldMenu.MenuItems.Contains(this))
                {
                    oldMenu.RemoveMenuItem(this);
                }
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the MenuItem class with mandatory and optional attributes.
        /// </summary>
        public MenuItem(int menuItemID, string name, double basePrice, int calories, double discountPrice, int preparationTime, bool isAvailable = true)
        {
            MenuItemID = menuItemID;
            Name = name;
            BasePrice = basePrice;
            Calories = calories;
            DiscountPrice = discountPrice;
            PreparationTime = preparationTime;
            IsAvailable = isAvailable;

            // Add to class extent
            menuItems.Add(this);
            TotalMenuItems = menuItems.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public MenuItem() { }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is MenuItem other)
            {
                return MenuItemID == other.MenuItemID &&
                       Name == other.Name &&
                       IsAvailable == other.IsAvailable &&
                       BasePrice == other.BasePrice &&
                       Calories == other.Calories &&
                       DiscountPrice == other.DiscountPrice &&
                       PreparationTime == other.PreparationTime;
                // Excluding Menu reference for simplicity
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MenuItemID, Name, IsAvailable, BasePrice, Calories, DiscountPrice, PreparationTime);
        }
    }
}
