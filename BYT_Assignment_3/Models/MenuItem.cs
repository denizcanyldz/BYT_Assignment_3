using System;
using System.Collections.Generic;
using System.Linq;
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
        public int MenuItemID
        {
            get => menuItemID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("MenuItemId must be greater than zero.");
                menuItemID = value;
            }
        }

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
                if (value < 0)
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
        private readonly List<Ingredient> ingredients = new List<Ingredient>();

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
            if (ingredient.ParentInventory == null)
                throw new InvalidOperationException("Ingredient must be assigned to an Inventory before being added to a MenuItem.");
    
            ingredients.Add(ingredient);
            ingredient.AddMenuItem(this); // Maintain bidirectional association
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
            ingredient.RemoveMenuItem(this); // Maintain bidirectional association
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
            foreach (var ingredient in ingredients.ToList())
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

        private Chef chef;

        /// <summary>
        /// Gets the Chef who prepares the MenuItem.
        /// </summary>
        [XmlIgnore] // Prevent circular reference during serialization
        public Chef Chef => chef;

        // For serialization purposes, store ChefID
        [XmlElement("ChefID")]
        public int ChefID
        {
            get => Chef.MenuItems.Contains(this) ? Chef.StaffID : 0;
            set
            {
                // This property is used during deserialization to associate MenuItem with Chef
                // The actual Chef object should be linked after all objects are deserialized
            }
        }

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Sets the Chef for the MenuItem, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="newChef">The Chef to associate with.</param>
        internal void SetChef(Chef newChef)
        {
            if (newChef == null)
                throw new ArgumentNullException(nameof(newChef), "Chef cannot be null.");

            if (this.chef != null && this.chef != newChef)
            {
                // Remove from previous chef's MenuItems
                this.chef.RemoveMenuItem(this);
            }

            this.chef = newChef;

            // Ensure bidirectional association
            if (!newChef.MenuItems.Contains(this))
            {
                newChef.AddMenuItem(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Chef, maintaining bidirectional consistency.
        /// </summary>
        internal void RemoveChef()
        {
            if (this.chef != null)
            {
                var oldChef = this.chef;
                this.chef = null;

                // Remove from chef's MenuItems
                if (oldChef.MenuItems.Contains(this))
                {
                    oldChef.RemoveMenuItem(this);
                }
            }
        }

        /// <summary>
        /// Sets the Menu for the MenuItem, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="newMenu">The Menu to associate with.</param>
        internal void SetMenu(Menu newMenu)
        {
            if (newMenu == null)
                throw new ArgumentNullException(nameof(newMenu), "Menu cannot be null.");

            if (this.menu != null && this.menu != newMenu)
            {
                // Remove from previous menu's MenuItems
                this.menu.RemoveMenuItem(this);
            }

            this.menu = newMenu;

            // Ensure bidirectional association
            if (!newMenu.MenuItems.Contains(this))
            {
                newMenu.AddMenuItem(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Menu, maintaining bidirectional consistency.
        /// </summary>
        internal void RemoveMenu()
        {
            if (this.menu != null)
            {
                var oldMenu = this.menu;
                this.menu = null;

                // Remove from menu's MenuItems
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
        /// <param name="menuItemID">The unique identifier for the menu item.</param>
        /// <param name="name">The name of the menu item.</param>
        /// <param name="basePrice">The base price of the menu item.</param>
        /// <param name="calories">The number of calories in the menu item.</param>
        /// <param name="discountPrice">The discount price of the menu item.</param>
        /// <param name="preparationTime">The preparation time in minutes.</param>
        /// <param name="isAvailable">Availability status.</param>
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
        
        public MenuItem(int menuItemID, string name, double basePrice, int calories, double discountPrice, int preparationTime, Menu menu, bool isAvailable = true)
        {
            MenuItemID = menuItemID;
            Name = name;
            BasePrice = basePrice;
            Calories = calories;
            DiscountPrice = discountPrice;
            PreparationTime = preparationTime;
            IsAvailable = isAvailable;
    
            SetMenu(menu);

            // Add to class extent and update total
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
                // Excluding Menu and Chef references for simplicity
            }
            return false;
        }
        
        // -------------------------------
        // RemoveFromExtent Method
        // -------------------------------
        /// <summary>
        /// Removes the MenuItem and all its associated Ingredients from the class extent.
        /// </summary>
        public void RemoveFromExtent()
        {
            // Remove all associated Ingredients
            foreach (var ingredient in ingredients.ToList()) // Use a copy to avoid modification during iteration
            {
                RemoveIngredient(ingredient);
                // ingredient.RemoveMenuItem(this) is already called within RemoveIngredient
            }

            // Remove associations with Chef and Menu
            RemoveChef();
            RemoveMenu();

            // Remove from class extent
            menuItems.Remove(this);
            TotalMenuItems = menuItems.Count;
        }

        /// <summary>
        /// Association with OrderItem
        /// </summary>
        public OrderItem? _orderItem { get; private set; }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentException("OrderItem cannot be null.");
            if (_orderItem == orderItem)
                return;

            _orderItem = orderItem;

            if (orderItem._menuItem != this)
            {
                orderItem.AddMenuItem(this);
            }
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentException("OrderItem cannot be null.");

            if (!(_orderItem == orderItem))
                throw new KeyNotFoundException("The specified OrderItem is not associated with this MenuItem.");

            _orderItem = null;

            if (orderItem._menuItem == this)
            {
                orderItem.RemoveMenuItem(this);
            }
        }

        public void ModifyOrderItem(OrderItem newOrderItem, OrderItem oldOrderItem)
        {
            if (newOrderItem == null || oldOrderItem == null)
                throw new ArgumentException("OrderItem cannot be null.");
            if (_orderItem != oldOrderItem)
                throw new ArgumentException("OrderItem not found.");

            _orderItem = newOrderItem;

            // Update reverse relationship
            if (oldOrderItem._menuItem == this)
            {
                oldOrderItem.RemoveMenuItem(this);
            }

            if (newOrderItem._menuItem != this)
            {
                newOrderItem.AddMenuItem(this);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MenuItemID, Name, IsAvailable, BasePrice, Calories, DiscountPrice, PreparationTime);
        }
    }
}
