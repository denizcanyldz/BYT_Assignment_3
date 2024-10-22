namespace BYT_Assignment_3;

public class Ingredient
{
    public static List<Ingredient> Ingredients = new List<Ingredient>();

    public int IngredientID { get; set; }
    public string Name { get; set; }
    public int QuantityInStock { get; set; }
    public string Unit { get; set; } 
    public bool IsPerishable { get; set; }

    public Ingredient(int ingredientID, string name, int quantityInStock, string unit, bool isPerishable)
    {
        if (string.IsNullOrEmpty(name) || quantityInStock < 0)
        {
            throw new ArgumentException("Invalid ingredient details.");
        }

        IngredientID = ingredientID;
        Name = name;
        QuantityInStock = quantityInStock;
        Unit = unit;
        IsPerishable = isPerishable;

        Ingredients.Add(this);
    }

    public void AddStock(int amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive.");
        }
        QuantityInStock += amount;
    }

    public void RemoveStock(int amount)
    {
        if (amount <= 0 || amount > QuantityInStock)
        {
            throw new InvalidOperationException("Insufficient stock or invalid amount.");
        }
        QuantityInStock -= amount;
    }

    public int CheckStock()
    {
        return QuantityInStock;
    }
}
