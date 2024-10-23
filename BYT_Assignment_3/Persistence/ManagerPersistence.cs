using BYT_Assignment_3.Models;

public class ManagerPersistence : Persistence<Manager>
{
    public static void SaveCustomers(string filePath)
    {
        Save(filePath, Manager.Managers);

    }
    public static void LoadCustomers(string filePath)
    {
        Manager.Managers = Load(filePath);
    }
}