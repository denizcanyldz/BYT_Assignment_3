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

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Ingredient class with mandatory and optional attributes.
        /// </summary>
        public Ingredient(int ingredientID, string name, double quantity, string? description = null)
        {
            IngredientID = ingredientID;
            Name = name;
            Quantity = quantity;
            Description = description;

            // Add to class extent
            ingredients.Add(this);
            TotalIngredients = ingredients.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Ingredient() { }
    }
}