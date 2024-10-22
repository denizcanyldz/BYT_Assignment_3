namespace BYT_Assignment_3;

public class MenuItemPersistence : Persistence<MenuItem>
{
    public static void SaveMenuItems(string filePath)
    {
        Save(filePath, MenuItem.MenuItems);
    }

    public static void LoadMenuItems(string filePath)
    {
        MenuItem.MenuItems = Load(filePath);
    }
}
