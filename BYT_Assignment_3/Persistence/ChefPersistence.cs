using BYT_Assignment_3.Models;

public class ChefPersistence : Persistence<Chef>
{
    public static void SaveChefs(string filePath)
    {
        Save(filePath, Chef.Chefs);

    }
    public static void LoadChefs(string filePath)
    {
        Chef.Chefs = Load(filePath);
    }
}
