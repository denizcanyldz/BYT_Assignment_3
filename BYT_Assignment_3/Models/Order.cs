namespace BYT_Assignment_3;

public class Order
{
    public static List<Order> Orders = new List<Order>();

    public int OrderID { get; set; }
    public Table Table { get; set; } // Association with Table
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public double TotalAmount => OrderItems.Sum(item => item.TotalPrice); // Derived attribute
    public string Status { get; set; }
    public DateTime Timestamp { get; set; }

    public Order(int orderID, Table table, string status)
    {
        OrderID = orderID;
        Table = table;
        Status = status;
        Timestamp = DateTime.Now;

        Orders.Add(this);
    }

    public void AddOrderItem(OrderItem item)
    {
        OrderItems.Add(item);
    }

    public void RemoveOrderItem(OrderItem item)
    {
        OrderItems.Remove(item);
    }
}
