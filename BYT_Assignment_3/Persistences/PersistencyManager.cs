using System;
using System.Collections.Generic;
using BYT_Assignment_3.Models;

namespace BYT_Assignment_3.Persistences
{
    public static class PersistencyManager
    {
        private const string FilePath = "extents.xml";

        /// <summary>
        /// Saves all class extents to a single XML file.
        /// </summary>
        public static void SaveAll()
        {
            try
            {
                Extents extents = new Extents
                {
                    Customers = new List<Customer>(Customer.GetAll()),
                    Chefs = new List<Chef>(Chef.GetAll()),
                    Bartenders = new List<Bartender>(Bartender.GetAll()),
                    Waiters = new List<Waiter>(Waiter.GetAll()),
                    Managers = new List<Manager>(Manager.GetAll()),
                    Feedbacks = new List<Feedback>(Feedback.GetAll()),
                    Ingredients = new List<Ingredient>(Ingredient.GetAll()),
                    Inventories = new List<Inventory>(Inventory.GetAll()),
                    MenuItems = new List<MenuItem>(MenuItem.GetAll()),
                    Orders = new List<Order>(Order.GetAll()),
                    OrderItems = new List<OrderItem>(OrderItem.GetAll()),
                    PaymentMethods = new List<PaymentMethod>(PaymentMethod.GetAll()),
                    Payments = new List<Payment>(Payment.GetAll()),
                    Reservations = new List<Reservation>(Reservation.GetAll()),
                    Tables = new List<Table>(Table.GetAll())
                };

                Persistence.SaveAll(FilePath, extents);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving all data: {ex.Message}");
                // Optionally, implement logging or additional error handling
            }
        }

        /// <summary>
        /// Loads all class extents from a single XML file.
        /// </summary>
        public static void LoadAll()
        {
            try
            {
                Extents extents = Persistence.LoadAll(FilePath);

                Customer.SetAll(extents.Customers);
                Chef.SetAll(extents.Chefs);
                Bartender.SetAll(extents.Bartenders);
                Waiter.SetAll(extents.Waiters);
                Manager.SetAll(extents.Managers);
                Feedback.SetAll(extents.Feedbacks);
                Ingredient.SetAll(extents.Ingredients);
                Inventory.SetAll(extents.Inventories);
                MenuItem.SetAll(extents.MenuItems);
                Order.SetAll(extents.Orders);
                OrderItem.SetAll(extents.OrderItems);
                PaymentMethod.SetAll(extents.PaymentMethods);
                Payment.SetAll(extents.Payments);
                Reservation.SetAll(extents.Reservations);
                Table.SetAll(extents.Tables);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading all data: {ex.Message}");
            }
        }
    }
}
