namespace BYT_Assignment_3;

public class OrderItem
{
    public static List<OrderItem> OrderItems = new List<OrderItem>();

    public int OrderItemID { get; set; }
    public MenuItem MenuItem { get; set; } // Association with MenuItem
    public int Quantity { get; set; }
    public string Notes { get; set; }
    public double UnitPrice => MenuItem.Price;
    public double TotalPrice => UnitPrice * Quantity;

    public OrderItem(int orderItemID, MenuItem menuItem, int quantity, string notes)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be at least 1.");
        }

        OrderItemID = orderItemID;
        MenuItem = menuItem;
        Quantity = quantity;
        Notes = notes;

        OrderItems.Add(this);
    }
}
