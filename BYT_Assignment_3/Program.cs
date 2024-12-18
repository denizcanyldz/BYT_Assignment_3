using System;
using System.Xml.Serialization;
using System.Linq;
using BYT_Assignment_3.Models;
using BYT_Assignment_3.Persistences;

namespace BYT_Assignment_3
{
  class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "BYT Assignment 3 - Restaurant Management System";

            // Load existing data
            PersistencyManager.LoadAll();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("      Welcome to the Restaurant Manager        ");
                Console.WriteLine("==============================================");
                Console.WriteLine("1. Manage Customers");
                Console.WriteLine("2. Manage Bartenders");
                Console.WriteLine("3. Manage Chefs");
                Console.WriteLine("4. Manage Waiters");
                Console.WriteLine("5. Manage WaiterBartenders");
                Console.WriteLine("6. Manage Orders");
                Console.WriteLine("7. Manage Tables");
                Console.WriteLine("8. Save Data");
                Console.WriteLine("9. Load Data");
                Console.WriteLine("0. Exit");
                Console.WriteLine("==============================================");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        ManageCustomers();
                        break;
                    case "2":
                        ManageBartenders();
                        break;
                    case "3":
                        ManageChefs();
                        break;
                    case "4":
                        ManageWaiters();
                        break;
                    case "5":
                        ManageWaiterBartenders();
                        break;
                    case "6":
                        ManageOrders();
                        break;
                    case "7":
                        ManageTables();
                        break;
                    case "8":
                        PersistencyManager.SaveAll();
                        break;
                    case "9":
                        PersistencyManager.LoadAll();
                        break;
                    case "0":
                        PersistencyManager.SaveAll();
                        Console.WriteLine("Data saved successfully. Exiting application...");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        #region Customer Management

        static void ManageCustomers()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage Customers -----");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. List Customers");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddCustomer();
                        break;
                    case "2":
                        ListCustomers();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddCustomer()
        {
            try
            {
                Console.WriteLine("----- Add New Customer -----");
                int customerID = PromptForInt("Enter Customer ID: ");
                string name = PromptForString("Enter Name: ");
                string email = PromptForString("Enter Email: ");
                string phoneNumber = PromptForString("Enter Phone Number: ");

                var customer = new Customer(customerID, name, email, phoneNumber);
                Console.WriteLine("Customer added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding customer: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void ListCustomers()
        {
            Console.WriteLine("----- List of Customers -----");
            var customers = Customer.GetAll();
            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerID}, Name: {customer.Name}, Email: {customer.Email}, Phone: {customer.PhoneNumber}");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region Bartender Management

        static void ManageBartenders()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage Bartenders -----");
                Console.WriteLine("1. Add Bartender");
                Console.WriteLine("2. List Bartenders");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddBartender();
                        break;
                    case "2":
                        ListBartenders();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddBartender()
        {
            try
            {
                Console.WriteLine("----- Add New Bartender -----");
                int staffID = PromptForInt("Enter Staff ID: ");
                string name = PromptForString("Enter Name: ");
                string contactNumber = PromptForString("Enter Contact Number: ");

                var bartender = new Bartender(staffID, name, contactNumber);
                Console.WriteLine("Bartender added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding bartender: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void ListBartenders()
        {
            Console.WriteLine("----- List of Bartenders -----");
            var bartenders = Bartender.GetAll();
            if (bartenders.Count == 0)
            {
                Console.WriteLine("No bartenders found.");
            }
            else
            {
                foreach (var bartender in bartenders)
                {
                    Console.WriteLine($"ID: {bartender.StaffID}, Name: {bartender.Name}, Contact: {bartender.ContactNumber}");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region Chef Management

        static void ManageChefs()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage Chefs -----");
                Console.WriteLine("1. Add Chef");
                Console.WriteLine("2. List Chefs");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddChef();
                        break;
                    case "2":
                        ListChefs();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddChef()
        {
            try
            {
                Console.WriteLine("----- Add New Chef -----");
                int staffID = PromptForInt("Enter Staff ID: ");
                string name = PromptForString("Enter Name: ");
 // Collect multiple specialties
                var specialties = new List<string>();
                Console.WriteLine("Enter Specialties (type 'done' when finished):");
                while (true)
                {
                    string specialtyInput = PromptForString("Enter Specialty: ");
                    if (specialtyInput.Equals("done", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    try
                    {
                        // Attempt to add the specialty using the Chef class's validation
                        // Temporarily create a Chef object to use the AddSpecialty method
                        // Alternatively, implement a separate validation method
                        var tempChef = new Chef(); // Using parameterless constructor
                        tempChef.AddSpecialty(specialtyInput);
                        specialties.Add(specialtyInput);
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Invalid specialty: {ex.Message}");
                    }
                }

                if (specialties.Count == 0)
                {
                    Console.WriteLine("At least one specialty is required to add a chef.");
                    return;
                }                string contactNumber = PromptForString("Enter Contact Number: ");

                var chef = new Chef(staffID, name, specialties, contactNumber);
                Console.WriteLine("Chef added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding chef: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void ListChefs()
        {
            Console.WriteLine("----- List of Chefs -----");
            var chefs = Chef.GetAll();
            if (chefs.Count == 0)
            {
                Console.WriteLine("No chefs found.");
            }
            else
            {
                foreach (var chef in chefs)
                {
                    string specialties = chef.Specialties.Count > 0
                        ? string.Join(", ", chef.Specialties)
                        : "None";
                    Console.WriteLine($"ID: {chef.StaffID}, Name: {chef.Name}, Specialties: {specialties}, Contact: {chef.ContactNumber}");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region Waiter Management

        static void ManageWaiters()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage Waiters -----");
                Console.WriteLine("1. Add Waiter");
                Console.WriteLine("2. List Waiters");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddWaiter();
                        break;
                    case "2":
                        ListWaiters();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddWaiter()
        {
            try
            {
                Console.WriteLine("----- Add New Waiter -----");
                int staffID = PromptForInt("Enter Staff ID: ");
                string name = PromptForString("Enter Name: ");
                bool tipsCollected = PromptForBool("Are tips collected? (y/n): ");
                string contactNumber = PromptForString("Enter Contact Number: ");

                var waiter = new Waiter(staffID, name, contactNumber);
                Console.WriteLine("Waiter added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding waiter: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void ListWaiters()
        {
            Console.WriteLine("----- List of Waiters -----");
            var waiters = Waiter.GetAll();
            if (waiters.Count == 0)
            {
                Console.WriteLine("No waiters found.");
            }
            else
            {
                foreach (var waiter in waiters)
                {
                    Console.WriteLine($"ID: {waiter.StaffID}, Name: {waiter.Name}, Tips Collected: {waiter.TipsCollected}, Contact: {waiter.ContactNumber}");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region WaiterBartender Management

        static void ManageWaiterBartenders()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage WaiterBartenders -----");
                Console.WriteLine("1. Add WaiterBartender");
                Console.WriteLine("2. List WaiterBartenders");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddWaiterBartender();
                        break;
                    case "2":
                        ListWaiterBartenders();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddWaiterBartender()
        {
            try
            {
                Console.WriteLine("----- Add New WaiterBartender -----");
                int staffID = PromptForInt("Enter Staff ID: ");
                string name = PromptForString("Enter Name: ");
                double? bonus = PromptForNullableDouble("Enter Bonus (or leave blank): ");
                string contactNumber = PromptForString("Enter Contact Number: ");

                var waiterBartender = new WaiterBartender(staffID, name, bonus, contactNumber);
                Console.WriteLine("WaiterBartender added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding WaiterBartender: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void ListWaiterBartenders()
        {
            Console.WriteLine("----- List of WaiterBartenders -----");
            var waiterBartenders = WaiterBartender.GetAll();
            if (waiterBartenders.Count == 0)
            {
                Console.WriteLine("No WaiterBartenders found.");
            }
            else
            {
                foreach (var wb in waiterBartenders)
                {
                    Console.WriteLine($"ID: {wb.StaffID}, Name: {wb.Name}, Bonus: {(wb.Bonus.HasValue ? wb.Bonus.Value.ToString("C") : "None")}, Contact: {wb.ContactNumber}");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region Order Management

        static void ManageOrders()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage Orders -----");
                Console.WriteLine("1. Add Order");
                Console.WriteLine("2. List Orders");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddOrder();
                        break;
                    case "2":
                        ListOrders();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddOrder()
        {
            try
            {
                Console.WriteLine("----- Add New Order -----");
                int orderID = PromptForInt("Enter Order ID: ");
                DateTime orderDate = PromptForDateTime("Enter Order Date and Time (yyyy-MM-dd HH:mm): ");
                int tableNumber = PromptForInt("Enter Table Number: ");
                string notes = PromptForString("Enter Notes (optional, press Enter to skip): ", allowEmpty: true);
                string discountCode = PromptForString("Enter Discount Code (optional, press Enter to skip): ", allowEmpty: true);

                // Find the table
                var table = Table.GetAll().FirstOrDefault(t => t.TableNumber == tableNumber);
                if (table == null)
                {
                    Console.WriteLine($"Table number {tableNumber} does not exist. Please add the table first.");
                    PromptToContinue();
                    return;
                }

                var order = new Order(orderID, orderDate, table, notes, discountCode);
                Console.WriteLine("Order added successfully!");

                // Optionally, add OrderItems to the Order
                bool addMoreItems = true;
                while (addMoreItems)
                {
                    Console.Write("Do you want to add an OrderItem? (y/n): ");
                    string response = Console.ReadLine().ToLower();
                    if (response == "y")
                    {
                        AddOrderItemToOrder(order);
                    }
                    else
                    {
                        addMoreItems = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void AddOrderItemToOrder(Order order)
        {
            try
            {
                Console.WriteLine("----- Add OrderItem -----");
                int orderItemID = PromptForInt("Enter OrderItem ID: ");
                string itemName = PromptForString("Enter Item Name: ");
                int quantity = PromptForInt("Enter Quantity: ");
                double price = PromptForDouble("Enter Price per Unit: ");
                string specialInstructions = PromptForString("Enter Special Instructions (optional, press Enter to skip): ", allowEmpty: true);

                var orderItem = new OrderItem(orderItemID, itemName, quantity, price, specialInstructions);
                order.AddOrderItem(orderItem);
                Console.WriteLine("OrderItem added to the order successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding OrderItem: {ex.Message}");
            }
        }

        static void ListOrders()
        {
            Console.WriteLine("----- List of Orders -----");
            var orders = Order.GetAll();
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found.");
            }
            else
            {
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order ID: {order.OrderID}, Date: {order.OrderDate}, Table: {order.Table.TableNumber}, Total Amount: {order.TotalAmount:C}");
                    Console.WriteLine($"Notes: {order.Notes}, Discount Code: {order.DiscountCode}");
                    Console.WriteLine("Order Items:");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"\tItem ID: {item.OrderItemID}, Name: {item.ItemName}, Quantity: {item.Quantity}, Price: {item.Price:C}, Special Instructions: {item.SpecialInstructions}");
                    }
                    Console.WriteLine("------------------------------------");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region Table Management

        static void ManageTables()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("----- Manage Tables -----");
                Console.WriteLine("1. Add Table");
                Console.WriteLine("2. List Tables");
                Console.WriteLine("0. Back to Main Menu");
                Console.Write("Select an option: ");

                string input = Console.ReadLine();
                Console.WriteLine();

                switch (input)
                {
                    case "1":
                        AddTable();
                        break;
                    case "2":
                        ListTables();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AddTable()
        {
            try
            {
                Console.WriteLine("----- Add New Table -----");
                int tableNumber = PromptForInt("Enter Table Number: ");
                int maxSeats = PromptForInt("Enter Maximum Seats: ");
                string location = PromptForString("Enter Location (optional, press Enter to skip): ", allowEmpty: true);
                string seatingArrangement = PromptForString("Enter Seating Arrangement (optional, press Enter to skip): ", allowEmpty: true);

                var table = new Table(tableNumber, maxSeats, location, seatingArrangement);
                Console.WriteLine("Table added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding table: {ex.Message}");
            }
            finally
            {
                PromptToContinue();
            }
        }

        static void ListTables()
        {
            Console.WriteLine("----- List of Tables -----");
            var tables = Table.GetAll();
            if (tables.Count == 0)
            {
                Console.WriteLine("No tables found.");
            }
            else
            {
                foreach (var table in tables)
                {
                    Console.WriteLine($"Table Number: {table.TableNumber}, Max Seats: {table.MaxSeats}, Location: {table.Location ?? "N/A"}, Seating: {table.SeatingArrangement ?? "N/A"}, Occupied: {table.IsOccupied}");
                }
            }
            PromptToContinue();
        }

        #endregion

        #region Helper Methods

        static int PromptForInt(string message)
        {
            int value;
            bool valid = false;
            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                valid = int.TryParse(input, out value);
                if (!valid)
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            } while (!valid);
            return value;
        }

        static double PromptForDouble(string message)
        {
            double value;
            bool valid = false;
            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                valid = double.TryParse(input, out value);
                if (!valid)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            } while (!valid);
            return value;
        }

        static double? PromptForNullableDouble(string message)
        {
            double value;
            Console.Write(message);
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return null;
            bool valid = double.TryParse(input, out value);
            if (!valid)
            {
                Console.WriteLine("Invalid input. Bonus set to null.");
                return null;
            }
            return value;
        }

        static string PromptForString(string message, bool allowEmpty = false)
        {
            string input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (!allowEmpty && string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                }
            } while (!allowEmpty && string.IsNullOrWhiteSpace(input));
            return input;
        }

        static bool PromptForBool(string message)
        {
            string input;
            do
            {
                Console.Write(message);
                input = Console.ReadLine().ToLower();
                if (input == "y" || input == "yes")
                    return true;
                else if (input == "n" || input == "no")
                    return false;
                else
                    Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
            } while (true);
        }

        static DateTime PromptForDateTime(string message)
        {
            DateTime dateTime;
            bool valid = false;
            do
            {
                Console.Write(message);
                string input = Console.ReadLine();
                valid = DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out dateTime);
                if (!valid)
                {
                    Console.WriteLine("Invalid format. Please enter in 'yyyy-MM-dd HH:mm' format.");
                }
            } while (!valid);
            return dateTime;
        }

        static void PromptToContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        #endregion
    }
}
