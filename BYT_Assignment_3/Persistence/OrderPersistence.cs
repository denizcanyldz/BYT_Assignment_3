namespace BYT_Assignment_3;

public class OrderPersistence : Persistence<Order>
{
    public static void SaveOrders(string filePath)
    {
        Save(filePath, Order.Orders);
    }

    public static void LoadOrders(string filePath)
    {
        Order.Orders = Load(filePath);
    }
}
