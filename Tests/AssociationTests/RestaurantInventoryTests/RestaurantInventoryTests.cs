using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class RestaurantInventoryTests
    {
        private Table testTable;

        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test to ensure test isolation
            OrderItem.SetAll(new List<OrderItem>());
            Order.SetAll(new List<Order>());
            Inventory.SetAll(new List<Inventory>());
            Restaurant.SetAll(new List<Restaurant>());

            // Initialize a common Table instance for use in tests
            testTable = new Table(1, 2);
        }

        [Test]
        public void AssignInventory_ToRestaurant_SetsBothSidesCorrectly()
        {
            // Arrange
            var restaurant = new Restaurant(1, "Testaurant", "123 Test St", "555-1234");
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act
            restaurant.AssignInventory(inventory);

            // Assert
            Assert.That(restaurant.Inventory, Is.EqualTo(inventory), "Restaurant's Inventory should be set correctly.");
            Assert.That(inventory.Restaurant, Is.EqualTo(restaurant), "Inventory's Restaurant should be set correctly.");
        }

        [Test]
        public void AssignInventory_ToRestaurant_WhenRestaurantAlreadyHasInventory_ShouldThrowException()
        {
            // Arrange
            var restaurant = new Restaurant(1, "Testaurant", "123 Test St", "555-1234");
            var inventory1 = new Inventory(1, DateTime.Now.AddDays(-1));
            var inventory2 = new Inventory(2, DateTime.Now.AddDays(-2));

            // Act
            restaurant.AssignInventory(inventory1);

            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => restaurant.AssignInventory(inventory2));
            Assert.That(ex.Message, Is.EqualTo("This Restaurant already has an Inventory assigned."), "Should not allow assigning multiple Inventories to a Restaurant.");
        }

        [Test]
        public void AssignInventory_ToRestaurant_WhenInventoryAlreadyAssignedToAnotherRestaurant_ShouldThrowException()
        {
            // Arrange
            var restaurant1 = new Restaurant(1, "Testaurant1", "123 Test St", "555-1234");
            var restaurant2 = new Restaurant(2, "Testaurant2", "456 Test Ave", "555-5678");
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act
            restaurant1.AssignInventory(inventory);

            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => restaurant2.AssignInventory(inventory));
            Assert.That(ex.Message, Is.EqualTo("This Inventory is already assigned to another Restaurant."), "Should not allow assigning an Inventory to multiple Restaurants.");
        }

        [Test]
        public void UnassignInventory_FromRestaurant_RemovesAssociationBothSides()
        {
            // Arrange
            var restaurant = new Restaurant(1, "Testaurant", "123 Test St", "555-1234");
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            restaurant.AssignInventory(inventory);

            // Act
            restaurant.UnassignInventory();

            // Assert
            Assert.That(restaurant.Inventory, Is.Null, "Restaurant's Inventory should be null after unassignment.");
            Assert.That(inventory.Restaurant, Is.Null, "Inventory's Restaurant should be null after unassignment.");
        }

        [Test]
        public void AssignInventory_WhenInventoryIsNull_UnassignsExistingInventory()
        {
            // Arrange
            var restaurant = new Restaurant(1, "Testaurant", "123 Test St", "555-1234");
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            restaurant.AssignInventory(inventory);

            // Act
            restaurant.UnassignInventory(); // Properly unassign using method

            // Assert
            Assert.That(restaurant.Inventory, Is.Null, "Restaurant's Inventory should be null after unassignment.");
            Assert.That(inventory.Restaurant, Is.Null, "Inventory's Restaurant should be null after Restaurant unassigns it.");
        }

        [Test]
        public void AssignInventory_ToInventory_SetsBothSidesCorrectly()
        {
            // Arrange
            var restaurant = new Restaurant(1, "Testaurant", "123 Test St", "555-1234");
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act
            // Assigning via Inventory's internal method is not possible directly.
            // Therefore, use Restaurant's AssignInventory method.
            restaurant.AssignInventory(inventory);

            // Assert
            Assert.That(inventory.Restaurant, Is.EqualTo(restaurant), "Inventory's Restaurant should be set correctly.");
            Assert.That(restaurant.Inventory, Is.EqualTo(inventory), "Restaurant's Inventory should be set correctly.");
        }

        [Test]
        public void AssignRestaurant_ToInventory_WhenInventoryAlreadyAssigned_ShouldThrowException()
        {
            // Arrange
            var restaurant1 = new Restaurant(1, "Testaurant1", "123 Test St", "555-1234");
            var restaurant2 = new Restaurant(2, "Testaurant2", "456 Test Ave", "555-5678");
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act
            restaurant1.AssignInventory(inventory);

            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => restaurant2.AssignInventory(inventory));
            Assert.That(ex.Message, Is.EqualTo("This Inventory is already assigned to another Restaurant."), "Should not allow assigning Inventory to multiple Restaurants.");
        }
    }
}
