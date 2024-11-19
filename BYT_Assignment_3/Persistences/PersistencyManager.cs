using System;
using System.Collections.Generic;
using BYT_Assignment_3.Models;

namespace BYT_Assignment_3.Persistences
{
  public static class PersistencyManager
    {
        private const string DefaultFilePath = "extents.xml"; // Compile-time constant

        /// <summary>
        /// Saves all class extents to a specified XML file.
        /// </summary>
        /// <param name="filePath">Path to the XML file. Defaults to "extents.xml".</param>
        public static void SaveAll(string filePath = DefaultFilePath)
        {
            try
            {
                Extents extents = new Extents
                {
                    Customers = new List<Customer>(Customer.GetAll()),
                    Bartenders = new List<Bartender>(Bartender.GetAll()),
                    Chefs = new List<Chef>(Chef.GetAll()),
                    Feedbacks = new List<Feedback>(Feedback.GetAll()),
                    Ingredients = new List<Ingredient>(Ingredient.GetAll()),
                    Inventories = new List<Inventory>(Inventory.GetAll()),
                    Managers = new List<Manager>(Manager.GetAll()),
                    MenuItems = new List<MenuItem>(MenuItem.GetAll()),
                    Orders = new List<Order>(Order.GetAll()),
                    OrderItems = new List<OrderItem>(OrderItem.GetAll()),
                    PaymentMethods = new List<PaymentMethod>(PaymentMethod.GetAll()),
                    Payments = new List<Payment>(Payment.GetAll()),
                    Reservations = new List<Reservation>(Reservation.GetAll()),
                    Tables = new List<Table>(Table.GetAll()),
                    Waiters = new List<Waiter>(Waiter.GetAll()),
                    Menus = new List<Menu>(Menu.GetAll()),
                    Restaurants = new List<Restaurant>(Restaurant.GetAll()),
                    WaiterBartenders = new List<WaiterBartender>(WaiterBartender.GetAll()),
                };
                

                Persistence.SaveAll(filePath, extents);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving all data: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads all class extents from a specified XML file.
        /// </summary>
        /// <param name="filePath">Path to the XML file. Defaults to "extents.xml".</param>
        public static void LoadAll(string filePath = DefaultFilePath)
        {
            try
            {
                Extents extents = Persistence.LoadAll(filePath);

                Customer.SetAll(extents.Customers);
                Bartender.SetAll(extents.Bartenders);
                Chef.SetAll(extents.Chefs);
                Feedback.SetAll(extents.Feedbacks);
                Ingredient.SetAll(extents.Ingredients);
                Inventory.SetAll(extents.Inventories);
                Manager.SetAll(extents.Managers);
                MenuItem.SetAll(extents.MenuItems);
                Order.SetAll(extents.Orders);
                OrderItem.SetAll(extents.OrderItems);
                PaymentMethod.SetAll(extents.PaymentMethods);
                Payment.SetAll(extents.Payments);
                Reservation.SetAll(extents.Reservations);
                Table.SetAll(extents.Tables);
                Waiter.SetAll(extents.Waiters);
                Menu.SetAll(extents.Menus);
                Restaurant.SetAll(extents.Restaurants);
                WaiterBartender.SetAll(extents.WaiterBartenders);

                // Aggregate all staff members from derived classes
                List<Staff> allStaff = new List<Staff>();
                allStaff.AddRange(Bartender.GetAll());
                allStaff.AddRange(Chef.GetAll());
                allStaff.AddRange(Waiter.GetAll());
                allStaff.AddRange(WaiterBartender.GetAll());

                // Update the Staff class's extent
                Staff.SetAll(allStaff);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading all data: {ex.Message}");
                throw new InvalidOperationException("Failed to load all data.", ex);
            }
        }
    }
}
