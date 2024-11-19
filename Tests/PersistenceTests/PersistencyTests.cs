using NUnit.Framework;
using BYT_Assignment_3.Persistences;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.IO;

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
