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
        /// Gets or sets the total number of ingredients.
        /// </summary>
        public static int TotalIngredients
        {
            get => totalIngredients;
            set
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
        /// </summary>
        public static void SetAll(List<Ingredient> loadedIngredients)
        {
            ingredients = loadedIngredients ?? new List<Ingredient>();
            TotalIngredients = ingredients.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int IngredientID { get; set; }

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

        private double quantity;

        public double Quantity
        {
            get => quantity;
            set
            {
                if(value < 0)
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
        public string Unit
        {
            get => unit;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Unit cannot be null or empty.");
                unit = value;
            }
        }

        private bool isPerishable;

        /// <summary>
        /// Gets or sets whether the ingredient is perishable.
        /// </summary>
        public bool IsPerishable
        {
            get => isPerishable;
            set
            {
                isPerishable = value;
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
            IngredientID = ingredientID;
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
        
        /// <summary>
        /// Determines whether the specified object is equal to the current Ingredient.
        /// </summary>
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

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(IngredientID, Name, Quantity, Unit, IsPerishable);
        }
    }
}