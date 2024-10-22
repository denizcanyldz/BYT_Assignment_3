namespace BYT_Assignment_3;

public class OrderItemPersistence : Persistence<OrderItem>
{
    public static void SaveOrderItems(string filePath)
    {
        Save(filePath, OrderItem.OrderItems);
    }

    public static void LoadOrderItems(string filePath)
    {
        OrderItem.OrderItems = Load(filePath);
    }
}
