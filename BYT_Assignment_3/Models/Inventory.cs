namespace BYT_Assignment_3;

public class Inventory
{
    public static List<Inventory> Inventories = new List<Inventory>();

    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>(); // Association with Ingredient
    public DateTime LastUpdated { get; set; }

    public Inventory()
    {
        LastUpdated = DateTime.Now;

        Inventories.Add(this);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient == null)
        {
            throw new ArgumentException("Ingredient cannot be null.");
        }

        if (!Ingredients.Contains(ingredient))
        {
            Ingredients.Add(ingredient);
            LastUpdated = DateTime.Now;
        }
    }

    public void UpdateStock(Ingredient ingredient, int newQuantity)
    {
        if (ingredient == null || newQuantity < 0)
        {
            throw new ArgumentException("Invalid ingredient or quantity.");
        }

        if (Ingredients.Contains(ingredient))
        {
            ingredient.QuantityInStock = newQuantity;
            LastUpdated = DateTime.Now;
        }
        else
        {
            throw new ArgumentException("Ingredient not found in inventory.");
        }
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        if (ingredient == null || !Ingredients.Contains(ingredient))
        {
            throw new ArgumentException("Ingredient not found in inventory.");
        }

        Ingredients.Remove(ingredient);
        LastUpdated = DateTime.Now;
    }
}
