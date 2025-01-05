using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using BYT_Assignment_3.Models;

namespace BYT_Assignment_3.Persistences
{
    [XmlRoot("Extents")]
    internal class Extents
    {
        [XmlArray("Customers")]
        [XmlArrayItem("Customer")]
        internal List<Customer> Customers { get; set; } = new List<Customer>();

        [XmlArray("Bartenders")]
        [XmlArrayItem("Bartender")]
        internal List<Bartender> Bartenders { get; set; } = new List<Bartender>();

        [XmlArray("Chefs")]
        [XmlArrayItem("Chef")]
        internal List<Chef> Chefs { get; set; } = new List<Chef>();

        [XmlArray("Feedbacks")]
        [XmlArrayItem("Feedback")]
        internal List<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        [XmlArray("Ingredients")]
        [XmlArrayItem("Ingredient")]
        internal List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        [XmlArray("Inventories")]
        [XmlArrayItem("Inventory")]
        internal List<Inventory> Inventories { get; set; } = new List<Inventory>();

        [XmlArray("Managers")]
        [XmlArrayItem("Manager")]
        internal List<Manager> Managers { get; set; } = new List<Manager>();

        [XmlArray("MenuItems")]
        [XmlArrayItem("MenuItem")]
        internal List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        [XmlArray("Orders")]
        [XmlArrayItem("Order")]
        internal List<Order> Orders { get; set; } = new List<Order>();

        [XmlArray("OrderItems")]
        [XmlArrayItem("OrderItem")]
        internal List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [XmlArray("PaymentMethods")]
        [XmlArrayItem("PaymentMethod")]
        internal List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

        [XmlArray("Payments")]
        [XmlArrayItem("Payment")]
        internal List<Payment> Payments { get; set; } = new List<Payment>();

        [XmlArray("Reservations")]
        [XmlArrayItem("Reservation")]
        internal List<Reservation> Reservations { get; set; } = new List<Reservation>();

        [XmlArray("Tables")]
        [XmlArrayItem("Table")]
        internal List<Table> Tables { get; set; } = new List<Table>();

        [XmlArray("Waiters")]
        [XmlArrayItem("Waiter")]
        internal List<Waiter> Waiters { get; set; } = new List<Waiter>();

        [XmlArray("Menus")]
        [XmlArrayItem("Menu")]
        internal List<Menu> Menus { get; set; } = new List<Menu>();

        [XmlArray("Restaurants")]
        [XmlArrayItem("Restaurant")]
        internal List<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        [XmlArray("WaiterBartenders")]
        [XmlArrayItem("WaiterBartender")]
        internal List<WaiterBartender> WaiterBartenders { get; set; } = new List<WaiterBartender>();
    }
}
