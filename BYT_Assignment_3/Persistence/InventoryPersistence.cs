namespace BYT_Assignment_3;

public class InventoryPersistence : Persistence<Inventory>
{
    public static void SaveInventories(string filePath)
    {
        Save(filePath, Inventory.Inventories);
    }

    public static void LoadInventories(string filePath)
    {
        Inventory.Inventories = Load(filePath);
    }
}
