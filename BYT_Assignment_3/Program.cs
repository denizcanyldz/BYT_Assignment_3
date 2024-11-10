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

           

            // Save all data before exiting
            PersistencyManager.SaveAll();
            Console.WriteLine("All data saved successfully.");

            // Prevent console from closing immediately
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
