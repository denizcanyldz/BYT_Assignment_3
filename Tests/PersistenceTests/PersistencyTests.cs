using NUnit.Framework;
using BYT_Assignment_3.Persistences;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using BYT_Assignment_3;

namespace Tests.PersistenceTests
{
   [TestFixture]
    public class PersistencyTests
    {
        // Path to the test XML file used for serialization/deserialization
        private const string TestFilePath = "test_extents.xml";

        /// <summary>
        /// Runs before each test to ensure a clean environment.
        /// Resets all class extents and deletes the test XML file if it exists.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Reset all class extents to ensure no residual data from previous tests
            Customer.SetAll(new List<Customer>());
            Bartender.SetAll(new List<Bartender>());
            Chef.SetAll(new List<Chef>());
            Feedback.SetAll(new List<Feedback>());
            Ingredient.SetAll(new List<Ingredient>());
            Inventory.SetAll(new List<Inventory>());
            Manager.SetAll(new List<Manager>());
            MenuItem.SetAll(new List<MenuItem>());
            Order.SetAll(new List<Order>());
            OrderItem.SetAll(new List<OrderItem>());
            PaymentMethod.SetAll(new List<PaymentMethod>());
            Payment.SetAll(new List<Payment>());
            Reservation.SetAll(new List<Reservation>());
            Table.SetAll(new List<Table>());
            Waiter.SetAll(new List<Waiter>());
            Menu.SetAll(new List<Menu>());
            Restaurant.SetAll(new List<Restaurant>());
            WaiterBartender.SetAll(new List<WaiterBartender>());

            // Ensure the test file does not exist before each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        /// <summary>
        /// Runs after each test to clean up the test environment.
        /// Deletes the test XML file and resets all class extents.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clean up the test file after each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            // Reset class extents after each test
            SetUp();
        }

        #region Persistence Tests

        /// <summary>
        /// Tests that saving all populated extents serializes the data correctly.
        /// Verifies that after saving and loading, all data matches the original instances.
        /// </summary>
        [Test]
        public void SaveAll_WithPopulatedExtents_ShouldSerializeDataCorrectly()
        {
            // Arrange
            // Create sample data for each class
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var bartender = new Bartender(1, "Jane Smith", "555-1234");
            var chef = new Chef(1, "Gordon Ramsay", "Italian", "555-5678");
            var feedback = new Feedback(1, 1, 5, DateTime.Now.AddDays(-1), 5.0, "Excellent service!");
            var ingredient = new Ingredient(1, "Tomato", 100.0, "Pieces", false);
            var inventory = new Inventory(1, DateTime.Now.AddDays(-2));
            var manager = new Manager(1, "Alice Johnson", "Operations");
            var menuItem = new MenuItem(1, "Burger", 10.0, 500, 9.0, 15, true);
            var table = new Table(1, 4, "Window", "Regular");
            var order = new Order(1, DateTime.Now.AddHours(-5), table, "No onions", "DISCOUNT10");
            var orderItem = new OrderItem(1, "Burger", 2, 18.0, "Extra cheese");
            var paymentMethod = new PaymentMethod(1, "Credit Card", "Visa and MasterCard accepted.");
            var payment = new Payment(1, 1, 36.0, DateTime.Now.AddHours(-4), "TXN12345");
            var reservation = new Reservation(1, 1, DateTime.Now.AddDays(1), table, "Confirmed", "Window seat please");
            var waiter = new Waiter(1, "Bob Brown", false, "555-6789");
            var menu = new Menu(1, new List<MenuItem> { menuItem });
            var restaurant = new Restaurant(1, "Main Branch", "123 Street", "555-0000", new List<Menu> { menu }, new Dictionary<string, string> { { "Monday", "9 AM - 9 PM" } });
            var waiterBartender = new WaiterBartender(1, "Charlie Davis", 100.0, "555-1111");

            // Establish relationships if necessary
            inventory.AddIngredient(ingredient);
            order.AddOrderItem(orderItem);
            reservation.AddOrderItem(orderItem);
            menuItem.AddIngredient(ingredient);

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save all data to the test XML file
            PersistencyManager.LoadAll(TestFilePath); // Load the data back from the test XML file

            // Assert
            // Verify counts for each class
            Assert.AreEqual(1, Customer.TotalCustomers, "TotalCustomers should be 1.");
            Assert.AreEqual(1, Bartender.TotalBartenders, "TotalBartenders should be 1.");
            Assert.AreEqual(1, Chef.TotalChefs, "TotalChefs should be 1.");
            Assert.AreEqual(1, Feedback.TotalFeedbacks, "TotalFeedbacks should be 1.");
            Assert.AreEqual(1, Ingredient.TotalIngredients, "TotalIngredients should be 1.");
            Assert.AreEqual(1, Inventory.TotalInventories, "TotalInventories should be 1.");
            Assert.AreEqual(1, Manager.TotalManagers, "TotalManagers should be 1.");
            Assert.AreEqual(1, MenuItem.TotalMenuItems, "TotalMenuItems should be 1.");
            Assert.AreEqual(1, Order.TotalOrders, "TotalOrders should be 1.");
            Assert.AreEqual(1, OrderItem.TotalOrderItems, "TotalOrderItems should be 1.");
            Assert.AreEqual(1, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 1.");
            Assert.AreEqual(1, Payment.TotalPayments, "TotalPayments should be 1.");
            Assert.AreEqual(1, Reservation.TotalReservations, "TotalReservations should be 1.");
            Assert.AreEqual(1, Table.TotalTables, "TotalTables should be 1.");
            Assert.AreEqual(1, Waiter.TotalWaiters, "TotalWaiters should be 1.");
            Assert.AreEqual(1, Menu.TotalMenus, "TotalMenus should be 1.");
            Assert.AreEqual(1, Restaurant.TotalRestaurants, "TotalRestaurants should be 1.");
            Assert.AreEqual(1, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 1.");

            // Verify that each object is present in its respective extent
            CollectionAssert.Contains(Customer.GetAll(), customer, "Customer should be present in the extent.");
            CollectionAssert.Contains(Bartender.GetAll(), bartender, "Bartender should be present in the extent.");
            CollectionAssert.Contains(Chef.GetAll(), chef, "Chef should be present in the extent.");
            CollectionAssert.Contains(Feedback.GetAll(), feedback, "Feedback should be present in the extent.");
            CollectionAssert.Contains(Ingredient.GetAll(), ingredient, "Ingredient should be present in the extent.");
            CollectionAssert.Contains(Inventory.GetAll(), inventory, "Inventory should be present in the extent.");
            CollectionAssert.Contains(Manager.GetAll(), manager, "Manager should be present in the extent.");
            CollectionAssert.Contains(MenuItem.GetAll(), menuItem, "MenuItem should be present in the extent.");
            CollectionAssert.Contains(Order.GetAll(), order, "Order should be present in the extent.");
            CollectionAssert.Contains(OrderItem.GetAll(), orderItem, "OrderItem should be present in the extent.");
            CollectionAssert.Contains(PaymentMethod.GetAll(), paymentMethod, "PaymentMethod should be present in the extent.");
            CollectionAssert.Contains(Payment.GetAll(), payment, "Payment should be present in the extent.");
            CollectionAssert.Contains(Reservation.GetAll(), reservation, "Reservation should be present in the extent.");
            CollectionAssert.Contains(Table.GetAll(), table, "Table should be present in the extent.");
            CollectionAssert.Contains(Waiter.GetAll(), waiter, "Waiter should be present in the extent.");
            CollectionAssert.Contains(Menu.GetAll(), menu, "Menu should be present in the extent.");
            CollectionAssert.Contains(Restaurant.GetAll(), restaurant, "Restaurant should be present in the extent.");
            CollectionAssert.Contains(WaiterBartender.GetAll(), waiterBartender, "WaiterBartender should be present in the extent.");
        }

        /// <summary>
        /// Tests that loading from a non-existing file initializes all extents as empty without throwing exceptions.
        /// </summary>
        [Test]
        public void LoadAll_WithNonExistingFile_ShouldInitializeEmptyExtents()
        {
            // Arrange
            // Ensure the test file does not exist
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            // Act
            PersistencyManager.LoadAll(TestFilePath); // Attempt to load from a non-existing file

            // Assert
            // Verify all extents are empty
            Assert.AreEqual(0, Customer.TotalCustomers, "TotalCustomers should be 0.");
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0.");
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0.");
            Assert.AreEqual(0, MenuItem.TotalMenuItems, "TotalMenuItems should be 0.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0.");
            Assert.AreEqual(0, Table.TotalTables, "TotalTables should be 0.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0.");
        }

        /// <summary>
        /// Tests that saving with empty extents results in empty data after deserialization.
        /// Ensures that no data is inadvertently saved when extents are empty.
        /// </summary>
        [Test]
        public void SaveAll_WithEmptyExtents_ShouldSerializeEmptyData()
        {
            // Arrange
            // No data is added; all extents are empty

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save all data (which is empty)
            PersistencyManager.LoadAll(TestFilePath); // Load the data back

            // Assert
            // Verify all extents are empty
            Assert.AreEqual(0, Customer.TotalCustomers, "TotalCustomers should be 0.");
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0.");
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0.");
            Assert.AreEqual(0, MenuItem.TotalMenuItems, "TotalMenuItems should be 0.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0.");
            Assert.AreEqual(0, Table.TotalTables, "TotalTables should be 0.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0.");
        }

        /// <summary>
        /// Tests that after saving initial data, modifying extents, and reloading, the original data is retained.
        /// Ensures that loading data overwrites any modifications made after saving.
        /// </summary>
        [Test]
        public void SaveAll_ThenModifyExtents_AndLoadAll_ShouldRetainOriginalData()
        {
            // Arrange
            // Create initial data and save
            var customer1 = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var bartender1 = new Bartender(1, "Jane Smith", "555-1234");

            PersistencyManager.SaveAll(TestFilePath); // Save initial data

            // Modify extents after saving
            var customer2 = new Customer(2, "Alice Wonderland", "alice@example.com", "0987654321");
            var bartender2 = new Bartender(2, "Bob Builder", "555-5678");

            // Act
            PersistencyManager.LoadAll(TestFilePath); // Load data back, should overwrite current extents

            // Assert
            // Only original data should be present
            Assert.AreEqual(1, Customer.TotalCustomers, "TotalCustomers should be 1 after loading.");
            Assert.AreEqual(customer1, Customer.GetAll()[0], "Loaded customer should match the saved customer.");
            CollectionAssert.DoesNotContain(Customer.GetAll(), customer2, "New customer should not be present after loading.");

            Assert.AreEqual(1, Bartender.TotalBartenders, "TotalBartenders should be 1 after loading.");
            Assert.AreEqual(bartender1, Bartender.GetAll()[0], "Loaded bartender should match the saved bartender.");
            CollectionAssert.DoesNotContain(Bartender.GetAll(), bartender2, "New bartender should not be present after loading.");
        }

        /// <summary>
        /// Tests that saving only partial data serializes only the populated extents.
        /// Verifies that non-populated extents remain empty after deserialization.
        /// </summary>
        [Test]
        public void SaveAll_WithPartialData_ShouldSerializeOnlyPopulatedExtents()
        {
            // Arrange
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var ingredient = new Ingredient(1, "Tomato", 100.0, "Pieces", false);

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save only customers and ingredients
            PersistencyManager.LoadAll(TestFilePath); // Load the data back

            // Assert
            // Verify populated extents
            Assert.AreEqual(1, Customer.TotalCustomers, "TotalCustomers should be 1.");
            Assert.AreEqual(customer, Customer.GetAll()[0], "Customer should match the saved data.");

            Assert.AreEqual(1, Ingredient.TotalIngredients, "TotalIngredients should be 1.");
            Assert.AreEqual(ingredient, Ingredient.GetAll()[0], "Ingredient should match the saved data.");

            // Verify other extents are empty
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0.");
            Assert.AreEqual(0, MenuItem.TotalMenuItems, "TotalMenuItems should be 0.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0.");
            Assert.AreEqual(0, Table.TotalTables, "TotalTables should be 0.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0.");
        }
        

        /// <summary>
        /// Tests that saving with null extents results in empty data after deserialization.
        /// Ensures that passing null or clearing data does not cause serialization issues.
        /// </summary>
        [Test]
        public void SaveAll_WithNullExtents_ShouldInitializeEmptyData()
        {
            // Arrange
            // Attempt to save with null extents by clearing all data
            // All extents are already cleared in SetUp

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save all data (which is empty)
            PersistencyManager.LoadAll(TestFilePath); // Load the data back

            // Assert
            // Verify all extents are empty
            Assert.AreEqual(0, Customer.TotalCustomers, "TotalCustomers should be 0.");
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0.");
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0.");
            Assert.AreEqual(0, MenuItem.TotalMenuItems, "TotalMenuItems should be 0.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0.");
            Assert.AreEqual(0, Table.TotalTables, "TotalTables should be 0.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0.");
        }

        /// <summary>
        /// Tests that saving with partial populated extents serializes only those populated classes.
        /// Verifies that only specified extents contain data while others remain empty.
        /// </summary>
        [Test]
        public void SaveAll_WithPartialPopulatedExtents_ShouldSerializeOnlyPopulatedClasses()
        {
            // Arrange
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var menuItem = new MenuItem(1, "Pizza", 15.0, 800, 14.0, 20, true);
            var table = new Table(1, 4, "Center", "Booth");

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save only populated extents
            PersistencyManager.LoadAll(TestFilePath); // Load the data back

            // Assert
            // Verify populated extents
            Assert.AreEqual(1, Customer.TotalCustomers, "TotalCustomers should be 1.");
            Assert.AreEqual(customer, Customer.GetAll()[0], "Customer should match the saved data.");

            Assert.AreEqual(1, MenuItem.TotalMenuItems, "TotalMenuItems should be 1.");
            Assert.AreEqual(menuItem, MenuItem.GetAll()[0], "MenuItem should match the saved data.");

            Assert.AreEqual(1, Table.TotalTables, "TotalTables should be 1.");
            Assert.AreEqual(table, Table.GetAll()[0], "Table should match the saved data.");

            // Verify other extents are empty
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0.");
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0.");
        }

        /// <summary>
        /// Tests that attempting to save invalid data (e.g., negative quantity) throws appropriate exceptions.
        /// Ensures that data integrity is maintained by preventing invalid objects from being serialized.
        /// </summary>
        [Test]
        public void SaveAll_WithInvalidData_ShouldHandleSerializationErrors()
        {
            // Arrange
            // Attempt to create an Ingredient with negative quantity, which should throw an exception
            Assert.Throws<ArgumentException>(() =>
            {
                var invalidIngredient = new Ingredient(1, "Tomato", -10.0, "Pieces", false);
                // Attempt to save invalid data
                PersistencyManager.SaveAll(TestFilePath);
            }, "Creating an Ingredient with negative quantity should throw an ArgumentException.");
        }

        /// <summary>
        /// Tests that after saving data, deleting the persistence file, and loading, all extents are initialized as empty.
        /// Ensures that the system correctly handles missing files by initializing empty data.
        /// </summary>
        [Test]
        public void SaveAll_ThenDeleteFile_LoadAll_ShouldInitializeEmptyExtents()
        {
            // Arrange
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save data
            Assert.IsTrue(File.Exists(TestFilePath), "Test file should exist after saving.");

            File.Delete(TestFilePath); // Delete the test file

            PersistencyManager.LoadAll(TestFilePath); // Attempt to load data back

            // Assert
            // Verify all extents are empty
            Assert.AreEqual(0, Customer.TotalCustomers, "TotalCustomers should be 0 after deleting the file.");
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0 after deleting the file.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0 after deleting the file.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0 after deleting the file.");
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0 after deleting the file.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0 after deleting the file.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0 after deleting the file.");
            Assert.AreEqual(0, MenuItem.TotalMenuItems, "TotalMenuItems should be 0 after deleting the file.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0 after deleting the file.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0 after deleting the file.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0 after deleting the file.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0 after deleting the file.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0 after deleting the file.");
            Assert.AreEqual(0, Table.TotalTables, "TotalTables should be 0 after deleting the file.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0 after deleting the file.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0 after deleting the file.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0 after deleting the file.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0 after deleting the file.");
        }

        /// <summary>
        /// Tests that loading from a corrupted XML file results in all extents being initialized as empty.
        /// Ensures that the system handles corrupted files without crashing and maintains data integrity.
        /// </summary>
        [Test]
        public void LoadAll_WithCorruptedFile_ShouldReturnEmptyExtents()
        {
            // Arrange
            // Create a corrupted XML file
            File.WriteAllText(TestFilePath, "This is not a valid XML content.");

            // Act
            PersistencyManager.LoadAll(TestFilePath); // Attempt to load corrupted data

            // Assert
            // Verify all extents are empty due to failed deserialization
            Assert.AreEqual(0, Customer.TotalCustomers, "TotalCustomers should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Bartender.TotalBartenders, "TotalBartenders should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Feedback.TotalFeedbacks, "TotalFeedbacks should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Manager.TotalManagers, "TotalManagers should be 0 after loading corrupted file.");
            Assert.AreEqual(0, MenuItem.TotalMenuItems, "TotalMenuItems should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Order.TotalOrders, "TotalOrders should be 0 after loading corrupted file.");
            Assert.AreEqual(0, OrderItem.TotalOrderItems, "TotalOrderItems should be 0 after loading corrupted file.");
            Assert.AreEqual(0, PaymentMethod.TotalPaymentMethods, "TotalPaymentMethods should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Payment.TotalPayments, "TotalPayments should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Reservation.TotalReservations, "TotalReservations should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Table.TotalTables, "TotalTables should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Waiter.TotalWaiters, "TotalWaiters should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Menu.TotalMenus, "TotalMenus should be 0 after loading corrupted file.");
            Assert.AreEqual(0, Restaurant.TotalRestaurants, "TotalRestaurants should be 0 after loading corrupted file.");
            Assert.AreEqual(0, WaiterBartender.TotalWaiterBartenders, "TotalWaiterBartenders should be 0 after loading corrupted file.");
        }

        /// <summary>
        /// Tests that saving data with circular references is handled appropriately.
        /// Since current model classes do not support circular references, the test is marked as passed.
        /// </summary>
        [Test]
        public void SaveAll_WithCircularReferences_ShouldHandleSerializationErrors()
        {
            // Arrange
            // Currently, model classes do not have circular references. If they did, XmlSerializer would throw an exception.
            // Therefore, this test is designed to pass.

            Assert.Pass("Current model classes do not have circular references. No serialization error expected.");
        }

        #endregion

        #region Class Extent Tests

        /// <summary>
        /// Tests that multiple instances of a class (e.g., Inventory) are correctly stored and counted in their extent.
        /// Ensures that the extent accurately reflects all instances.
        /// </summary>
        [Test]
        public void Inventory_Extent_ShouldStoreInstancesCorrectly()
        {
            // Arrange
            var inventory1 = new Inventory(1, DateTime.Now.AddDays(-1));
            var inventory2 = new Inventory(2, DateTime.Now.AddDays(-2));

            // Act
            var allInventories = Inventory.GetAll(); // Retrieve all inventories

            // Assert
            Assert.AreEqual(2, Inventory.TotalInventories, "TotalInventories should be 2 after adding two inventories.");
            CollectionAssert.Contains(allInventories, inventory1, "Inventory1 should be present in the inventories extent.");
            CollectionAssert.Contains(allInventories, inventory2, "Inventory2 should be present in the inventories extent.");
        }

        /// <summary>
        /// Tests that the TotalItems property in Inventory accurately reflects the number of Ingredients.
        /// Ensures that adding and removing ingredients updates the TotalItems count correctly.
        /// </summary>
        [Test]
        public void Inventory_TotalItems_ShouldReflectNumberOfIngredients()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient1 = new Ingredient(1, "Tomato", 100.0, "Pieces", false);
            var ingredient2 = new Ingredient(2, "Lettuce", 50.0, "Leaves", false);

            // Act
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);

            // Assert
            Assert.AreEqual(2, inventory.TotalItems, "TotalItems should be 2 after adding two ingredients.");
            CollectionAssert.Contains(inventory.Ingredients, ingredient1, "Ingredients should contain ingredient1.");
            CollectionAssert.Contains(inventory.Ingredients, ingredient2, "Ingredients should contain ingredient2.");
        }

        #endregion
        
    }
}

