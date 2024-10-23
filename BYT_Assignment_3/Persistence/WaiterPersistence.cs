using BYT_Assignment_3.Models;

public class WaiterPersistence : Persistence<Waiter>
{
    public static void SaveWaiters(string filePath)
    {
        Save(filePath, Waiter.Waiters);

    }
    public static void LoadCustomers(string filePath)
    {
        Waiter.Waiters = Load(filePath);
    }
}