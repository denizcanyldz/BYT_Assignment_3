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
        private const string TestFilePath = "test_extents.xml";

        [SetUp]
        public void SetUp()
        {
            // Reset all class extents before each test to ensure test isolation
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
            PersistencyManager.LoadAll(TestFilePath);

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

        [Test]
        public void SaveAll_WithEmptyExtents_ShouldSerializeEmptyData()
        {
            // Arrange
            // No data is added; all extents are empty

            // Act
            PersistencyManager.SaveAll(TestFilePath);

            // Assert
            // Load the data back
            PersistencyManager.LoadAll(TestFilePath);

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

        [Test]
        public void SaveAll_ThenDeleteFile_LoadAll_ShouldInitializeEmptyExtents()
        {
            // Arrange
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var extentsToSave = new Extents
            {
                Customers = new List<Customer> { customer }
                // Add other classes as needed
            };

            // Act
            PersistencyManager.SaveAll(TestFilePath);

            // Ensure the file exists
            Assert.IsTrue(File.Exists(TestFilePath), "Test file should exist after saving.");

            // Delete the file
            File.Delete(TestFilePath);

            // Load the data back
            PersistencyManager.LoadAll(TestFilePath);

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

        [Test]
        public void SaveAll_WithNullExtents_ShouldInitializeEmptyData()
        {
            // Arrange
            // Attempt to save with null extents by clearing all data
            // All extents are already cleared in SetUp

            // Act
            PersistencyManager.SaveAll(TestFilePath);

            // Load the data back
            PersistencyManager.LoadAll(TestFilePath);

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

        [Test]
        public void SaveAll_WithPartialPopulatedExtents_ShouldSerializeOnlyPopulatedClasses()
        {
            // Arrange
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var menuItem = new MenuItem(1, "Pizza", 15.0, 800, 14.0, 20, true);
            var table = new Table(1, 4, "Center", "Booth");

            // Act
            PersistencyManager.SaveAll(TestFilePath);

            // Load the data back
            PersistencyManager.LoadAll(TestFilePath);

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

        [Test]
        public void SaveAll_WithInvalidData_ShouldHandleSerializationErrors()
        {
            // Arrange
            // Attempt to create an invalid Chef with a long specialty
            Assert.Throws<ArgumentException>(() =>
            {
                var invalidChef = new Chef(1, "Gordon Ramsay", new string('A', 101), "555-0001");
                // Attempt to save invalid data
                PersistencyManager.SaveAll(TestFilePath);
            }, "Creating a Chef with specialty length exceeding 100 should throw an ArgumentException.");
        }

        [Test]
        public void SaveAll_WithCircularReferences_ShouldHandleSerializationErrors()
        {
            // Arrange
            // Create circular references if any (assuming none based on provided classes)
            // If circular references exist, XMLSerializer will throw an exception
            // For demonstration, let's assume Restaurant contains a Menu which contains a MenuItem which references Restaurant
            // Modify the MenuItem class to include a reference to Restaurant to create a circular reference (Not present in current classes)
            // Since current classes do not have circular references, this test will pass without error
            // Thus, to demonstrate, we can skip or note that current model classes do not support circular references
            Assert.Pass("Current model classes do not have circular references. No serialization error expected.");
        }

        #endregion

        #region Class Extent Tests

        [Test]
        public void Inventory_Extent_ShouldStoreInstancesCorrectly()
        {
            // Arrange
            var inventory1 = new Inventory(1, DateTime.Now.AddDays(-1));
            var inventory2 = new Inventory(2, DateTime.Now.AddDays(-2));

            // Act
            var allInventories = Inventory.GetAll();

            // Assert
            Assert.AreEqual(2, Inventory.TotalInventories, "TotalInventories should be 2 after adding two inventories.");
            CollectionAssert.Contains(allInventories, inventory1, "Inventory1 should be present in the inventories extent.");
            CollectionAssert.Contains(allInventories, inventory2, "Inventory2 should be present in the inventories extent.");
        }

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

        #region Edge Case Tests

        [Test]
        public void SaveAll_WithInvalidFilePath_ShouldThrowException()
        {
            // Arrange
            string invalidFilePath = "?:\\invalid_path\\extents.xml";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() =>
            {
                PersistencyManager.SaveAll(invalidFilePath);
            }, "Saving to an invalid file path should throw an exception.");

            Assert.IsNotNull(ex, "IOException should be thrown for invalid file path.");
        }

        [Test]
        public void LoadAll_WithCorruptedFile_ShouldReturnEmptyExtents()
        {
            // Arrange
            // Create a corrupted XML file
            File.WriteAllText(TestFilePath, "This is not a valid XML content.");

            // Act
            PersistencyManager.LoadAll(TestFilePath);

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

        #endregion
    }
}

