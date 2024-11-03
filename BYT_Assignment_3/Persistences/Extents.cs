using BYT_Assignment_3.Models;

namespace BYT_Assignment_3;

[Serializable]
public class Extents
{
    public List<Customer> Customers { get; set; } = new List<Customer>();
    public List<Bartender> Bartenders { get; set; } = new List<Bartender>();
    public List<Chef> Chefs { get; set; } = new List<Chef>();
    public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public List<Inventory> Inventories { get; set; } = new List<Inventory>();
    public List<Manager> Managers { get; set; } = new List<Manager>();
    public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    public List<Order> Orders { get; set; } = new List<Order>();
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    public List<Table> Tables { get; set; } = new List<Table>();
    public List<Waiter> Waiters { get; set; } = new List<Waiter>();
}