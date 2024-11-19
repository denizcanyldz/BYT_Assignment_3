using System.Xml.Serialization;
using BYT_Assignment_3.Models;

namespace BYT_Assignment_3
{
    [XmlRoot("Extents")]
    public class Extents
    {
        [XmlArray("Customers")]
        [XmlArrayItem("Customer")]
        public List<Customer> Customers { get; set; } = new List<Customer>();

        [XmlArray("Bartenders")]
        [XmlArrayItem("Bartender")]
        public List<Bartender> Bartenders { get; set; } = new List<Bartender>();

        [XmlArray("Chefs")]
        [XmlArrayItem("Chef")]
        public List<Chef> Chefs { get; set; } = new List<Chef>();

        [XmlArray("Feedbacks")]
        [XmlArrayItem("Feedback")]
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        [XmlArray("Ingredients")]
        [XmlArrayItem("Ingredient")]
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        [XmlArray("Inventories")]
        [XmlArrayItem("Inventory")]
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();

        [XmlArray("Managers")]
        [XmlArrayItem("Manager")]
        public List<Manager> Managers { get; set; } = new List<Manager>();

        [XmlArray("MenuItems")]
        [XmlArrayItem("MenuItem")]
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        public List<Order> Orders { get; set; } = new List<Order>();

        [XmlArray("OrderItems")]
        [XmlArrayItem("OrderItem")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [XmlArray("PaymentMethods")]
        [XmlArrayItem("PaymentMethod")]
        public List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

        [XmlArray("Payments")]
        [XmlArrayItem("Payment")]
        public List<Payment> Payments { get; set; } = new List<Payment>();

        [XmlArray("Reservations")]
        [XmlArrayItem("Reservation")]
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        [XmlArray("Tables")]
        [XmlArrayItem("Table")]
        public List<Table> Tables { get; set; } = new List<Table>();

        [XmlArray("Waiters")]
        [XmlArrayItem("Waiter")]
        public List<Waiter> Waiters { get; set; } = new List<Waiter>();

        [XmlArray("Menus")]
        [XmlArrayItem("Menu")]
        public List<Menu> Menus { get; set; } = new List<Menu>();

        [XmlArray("Restaurants")]
        [XmlArrayItem("Restaurant")]
        public List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        [XmlArray("WaiterBartenders")]
        [XmlArrayItem("WaiterBartender")]
        public List<WaiterBartender> WaiterBartenders { get; set; } = new List<WaiterBartender>();
    }
}