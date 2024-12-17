using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Ingredient
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalIngredients = 0;

        /// <summary>
        /// Gets the total number of ingredients.
        /// </summary>
        public static int TotalIngredients
        {
            get => totalIngredients;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("TotalIngredients cannot be negative.");
                totalIngredients = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Ingredient> ingredients = new List<Ingredient>();

        /// <summary>
        /// Gets a read-only list of all ingredients.
        /// </summary>
        public static IReadOnlyList<Ingredient> GetAll()
        {
            return ingredients.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire ingredient list (used during deserialization).
        /// Validates each ingredient entry before adding.
        /// </summary>
        /// <param name="loadedIngredients">List of ingredients to load.</param>
        public static void SetAll(List<Ingredient> loadedIngredients)
        {
            ingredients = loadedIngredients ?? new List<Ingredient>();
            TotalIngredients = 0; // Reset count before re-adding validated ingredients

            foreach (var ingredient in ingredients.ToList()) // Use a copy to allow safe removal
            {
                try
                {
                    Console.WriteLine($"Processing Ingredient: {ingredient.Name} (IngredientID: {ingredient.IngredientID})");
                    ValidateIngredient(ingredient);
                    TotalIngredients++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding Ingredient (IngredientID: {ingredient.IngredientID}): {ex.Message}");
                    ingredients.Remove(ingredient); // Remove invalid ingredient
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the average quantity of all ingredients.
        /// </summary>
        public static double AverageQuantity
        {
            get
            {
                if (ingredients.Count == 0)
                    return 0.0;

                return ingredients.Average(ing => ing.Quantity);
            }
        }

        // -------------------------------
        // Mandatory Attributes (Immutable)
        // -------------------------------
        [XmlElement("IngredientID")]
        private int ingredientID;
        public int IngredientID
        {
            get => ingredientID;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("IngredientID must be positive.");
                ingredientID = value;
            }
        }
        private string name;

        [XmlElement("Name")]
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

        private double quantity;

        [XmlElement("Quantity")]
        public double Quantity
        {
            get => quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Quantity cannot be negative.");
                quantity = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string unit;

        /// <summary>
        /// Gets or sets the unit of measurement for the ingredient.
        /// </summary>
        [XmlElement("Unit")]
        public string Unit
        {
            get => unit;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Unit cannot be null or empty.");
                unit = value;
            }
        }

        private bool isPerishable;

        /// <summary>
        /// Gets or sets whether the ingredient is perishable.
        /// </summary>
        [XmlElement("IsPerishable")]
        public bool IsPerishable
        {
            get => isPerishable;
            set
            {
                isPerishable = value;
            }
        }
        
        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<MenuItem> menuItems = new List<MenuItem>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<MenuItem> MenuItems => menuItems.AsReadOnly();

        /// <summary>
        /// Adds a menu item to the ingredient.
        /// </summary>
        internal void AddMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem), "MenuItem cannot be null.");
            if (!menuItems.Contains(menuItem))
            {
                menuItems.Add(menuItem);
            }
        }

        /// <summary>
        /// Removes a menu item from the ingredient.
        /// </summary>
        internal void RemoveMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem), "MenuItem cannot be null.");
            if (menuItems.Contains(menuItem))
            {
                menuItems.Remove(menuItem);
            }
        }
        
        
        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Ingredient class with mandatory and optional attributes.
        /// </summary>
        public Ingredient(int ingredientID, string name, double quantity, string unit, bool isPerishable)
        {
            IngredientID = ingredientID; // Setter includes validation
            Name = name;
            Quantity = quantity;
            Unit = unit;
            IsPerishable = isPerishable;

            // Add to class extent
            ingredients.Add(this);
            TotalIngredients = ingredients.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Ingredient() { }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Ingredient other)
            {
                return IngredientID == other.IngredientID &&
                       Name == other.Name &&
                       Quantity == other.Quantity &&
                       Unit == other.Unit &&
                       IsPerishable == other.IsPerishable;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IngredientID, Name, Quantity, Unit, IsPerishable);
        }

        // -------------------------------
        // Private Validation Method
        // -------------------------------
        /// <summary>
        /// Validates the properties of an Ingredient instance.
        /// </summary>
        /// <param name="ingredient">The Ingredient instance to validate.</param>
        internal static void ValidateIngredient(Ingredient ingredient)
        {
            if (ingredient.IngredientID <= 0)
                throw new ArgumentException("IngredientID must be positive.");

            if (string.IsNullOrWhiteSpace(ingredient.Name))
                throw new ArgumentException("Name cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(ingredient.Unit))
                throw new ArgumentException("Unit cannot be null or empty.");

            if (ingredient.Quantity < 0)
                throw new ArgumentException("Quantity cannot be negative.");
        }
    }
}
