namespace BYT_Assignment_3;

public class MenuItem
{
    public static List<MenuItem> MenuItems = new List<MenuItem>();

    public int ItemID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool IsAvailable { get; set; }
    public string Category { get; set; } // e.g., "Appetizer", "Main Course"
    public int PreparationTime { get; set; } // in minutes
    public int Calories { get; set; }
    public double DiscountPrice { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>(); // Association with Ingredient

    public MenuItem(int itemID, string name, string description, double price, bool isAvailable, string category, int preparationTime, int calories, double discountPrice)
    {
        if (string.IsNullOrEmpty(name) || price <= 0)
        {
            throw new ArgumentException("Invalid menu item details.");
        }

        ItemID = itemID;
        Name = name;
        Description = description;
        Price = price;
        IsAvailable = isAvailable;
        Category = category;
        PreparationTime = preparationTime;
        Calories = calories;
        DiscountPrice = discountPrice;

        MenuItems.Add(this);
    }

    public void UpdatePrice(double newPrice)
    {
        if (newPrice <= 0)
        {
            throw new ArgumentException("Price must be positive.");
        }
        Price = newPrice;
    }

    public void ApplyDiscount(double discount)
    {
        if (discount < 0 || discount > Price)
        {
            throw new ArgumentException("Invalid discount amount.");
        }
        DiscountPrice = Price - discount;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }
}
