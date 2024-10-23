using BYT_Assignment_3.Models;

public class BartenderPersistence : Persistence<Bartender>
{
    public static void SaveBartenders(string filePath)
    {
        Save(filePath, Bartender.Bartenders);

    }
    public static void LoadBartenders(string filePath)
    {
        Bartender.Bartenders = Load(filePath);
    }
}