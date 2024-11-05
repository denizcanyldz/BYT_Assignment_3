using System;
using BYT_Assignment_3.Models;
using BYT_Assignment_3.Persistences;

namespace BYT_Assignment_3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load all data at startup
            PersistencyManager.LoadAll();

            // Application logic here
            // Example: Creating a new customer
            try
            {
                var customer = new Customer(1, "John Doe", "john.doe@example.com", "5551234567");
                Console.WriteLine("Customer created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating customer: {ex.Message}");
            }

            // Example: Creating a new table
            try
            {
                var table = new Table(101, 4, "Near window", "Round");
                Console.WriteLine("Table created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table: {ex.Message}");
            }

            // Example: Creating a new bartender
            try
            {
                var bartender = new Bartender(1, "Alice Smith", "B123456", "Evening Shift");
                Console.WriteLine("Bartender created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating bartender: {ex.Message}");
            }

            // Example: Creating a new chef
            try
            {
                var chef = new Chef(2, "Gordon Ramsay", "Italian Cuisine", "Morning Shift");
                Console.WriteLine("Chef created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating chef: {ex.Message}");
            }

            // Example: Creating a new waiter
            try
            {
                var waiter = new Waiter(3, "James Bond", "VIP Section", "Afternoon Shift");
                Console.WriteLine("Waiter created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating waiter: {ex.Message}");
            }

            // Example: Creating a new feedback
            try
            {
                var feedback = new Feedback(1, 1, 5, "Excellent service!");
                Console.WriteLine("Feedback created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating feedback: {ex.Message}");
            }

            // Example: Creating a new ingredient
            try
            {
                var ingredient = new Ingredient(1, "Tomato Sauce", 2.5, "Organic tomato sauce.");
                Console.WriteLine("Ingredient created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating ingredient: {ex.Message}");
            }

            // Example: Creating a new inventory
            try
            {
                var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
                var ingredient1 = new Ingredient(2, "Cheese", 5.0, "Mozzarella cheese.");
                inventory.AddIngredient(ingredient1);
                Console.WriteLine("Inventory created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating inventory: {ex.Message}");
            }

            // Example: Creating a new manager
            try
            {
                var manager = new Manager(1, "Samantha Carter", "Operations");
                Console.WriteLine("Manager created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating manager: {ex.Message}");
            }

            // Example: Creating a new menu item
            try
            {
                var menuItem = new MenuItem(1, "Spaghetti Bolognese", 100,"Classic Italian pasta dish.", true);
                var ingredient2 = new Ingredient(3, "Spaghetti", 1.0, "Durum wheat spaghetti.");
                menuItem.AddIngredient(ingredient2);
                Console.WriteLine("Menu item created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating menu item: {ex.Message}");
            }

            // Example: Creating a new order
            try
            {
                var order = new Order(1, DateTime.Now, "No onions", "DISC10");
                var orderItem1 = new OrderItem(1, "Spaghetti Bolognese", 2, 12.99, "Extra cheese");
                var orderItem2 = new OrderItem(2, "Caesar Salad", 1, 6.99);
                order.AddOrderItem(orderItem1);
                order.AddOrderItem(orderItem2);
                Console.WriteLine("Order created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
            }

            // Example: Creating a new payment method
            try
            {
                var paymentMethod = new PaymentMethod(1, "Credit Card", "Visa, MasterCard accepted.");
                Console.WriteLine("Payment method created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating payment method: {ex.Message}");
            }

            // Example: Creating a new payment
            try
            {
                var payment = new Payment(1, 1, 32.97, "TXN1234567890");
                Console.WriteLine("Payment created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating payment: {ex.Message}");
            }

            // Example: Creating a new reservation
            try
            {
                var reservation = new Reservation(1, 1, DateTime.Now.AddDays(1), "Window seat");
                var orderItem3 = new OrderItem(3, "Lemonade", 2, 3.99);
                reservation.AddOrderItem(orderItem3);
                Console.WriteLine("Reservation created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating reservation: {ex.Message}");
            }

            // Save all data before exiting
            PersistencyManager.SaveAll();
            Console.WriteLine("All data saved successfully.");

            // Prevent console from closing immediately
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
