namespace BYT_Assignment_3;

public class IngredientPersistence : Persistence<Ingredient>
{
    public static void SaveIngredients(string filePath)
    {
        Save(filePath, Ingredient.Ingredients);
    }

    public static void LoadIngredients(string filePath)
    {
        Ingredient.Ingredients = Load(filePath);
    }
}
