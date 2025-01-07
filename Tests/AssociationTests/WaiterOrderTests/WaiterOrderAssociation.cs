using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using BYT_Assignment_3.Models;

namespace Tests.AssociationTests
{
    [TestFixture]
    public class WaiterOrderAssociationTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test to ensure test isolation
            Waiter.SetAll(new List<Waiter>());
            Order.SetAll(new List<Order>());
            Staff.SetAll(new List<Staff>());
            Staff.TotalStaff = 0;
        }

        [Test]
        public void CreateAssociation_AddOrderToWaiter_SetsBothSidesCorrectly()
        {
            // Arrange
            var waiter = new Waiter(1, "John Doe");
            var table = new Table(1, 4);
            var order = new Order(10, DateTime.Now, table);

            // Act
            waiter.AddOrder(order);

            // Assert
            Assert.Contains(order, waiter.Orders.ToList(),
                "Waiter should have the newly added order in its Orders list.");
            Assert.AreEqual(waiter, order.Waiter,
                "Order's Waiter property should be set to the Waiter who added the Order.");
        }

        [Test]
        public void CreateAssociation_AddOrderThrows_WhenOrderIsNull()
        {
            // Arrange
            var waiter = new Waiter(1, "John Doe");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => waiter.AddOrder(null!),
                "Adding a null Order should throw an ArgumentNullException.");
        }

        [Test]
        public void DeleteAssociation_RemoveOrderFromWaiter_SetsBothSidesCorrectly()
        {
            // Arrange
            var waiter = new Waiter(1, "John Doe");
            var table = new Table(1, 4);
            var order = new Order(111, DateTime.Now, table);
            waiter.AddOrder(order);

            // Act
            waiter.RemoveOrder(order);

            // Assert
            Assert.IsFalse(waiter.Orders.Contains(order),
                "Waiter's Orders should no longer contain the removed Order.");
            Assert.IsNull(order.Waiter,
                "Order's Waiter reference should be null after removal.");
        }

        [Test]
        public void DeleteAssociation_RemoveOrderThrows_WhenOrderNotFound()
        {
            // Arrange
            var waiter = new Waiter(1, "John Doe");
            var table = new Table(1, 4);
            var order = new Order(10, DateTime.Now, table);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => waiter.RemoveOrder(order),
                "Removing an Order that doesn't belong to this Waiter should throw an ArgumentException.");
        }

        [Test]
        public void UpdateAssociation_UpdateOrder_SetsBothSidesCorrectly()
        {
            // Arrange
            var waiter = new Waiter(1, "John Doe");
            var table = new Table(1, 4);
            var oldOrder = new Order(10, DateTime.Now, table);
            var newOrder = new Order(20, DateTime.Now, table);
            waiter.AddOrder(oldOrder);

            // Act
            waiter.UpdateOrder(oldOrder, newOrder);

            // Assert
            Assert.IsFalse(waiter.Orders.Contains(oldOrder),
                "Old order should be removed from the Waiter's Orders list.");
            Assert.IsNull(oldOrder.Waiter,
                "Old order's Waiter reference should be null after removal.");
            Assert.Contains(newOrder, waiter.Orders.ToList(),
                "New order should be added to the Waiter's Orders list.");
            Assert.AreEqual(waiter, newOrder.Waiter,
                "New order's Waiter reference should be set to the correct Waiter.");
        }

        [Test]
        public void UpdateAssociation_Throws_WhenOldOrderNotFound()
        {
            // Arrange
            var waiter = new Waiter(1, "John Doe");
            var table = new Table(1, 4);
            var oldOrder = new Order(10, DateTime.Now, table);
            var newOrder = new Order(20, DateTime.Now, table);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => waiter.UpdateOrder(oldOrder, newOrder),
                "Updating from an oldOrder that doesn't belong to this Waiter should throw an exception.");
        }

        [Test]
        public void CreateAssociation_Throws_WhenOrderAlreadyHasDifferentWaiter()
        {
            // Arrange
            var waiterA = new Waiter(1, "John Doe");
            var waiterB = new Waiter(2, "Jane Doe");
            var table = new Table(1, 4);
            var order = new Order(10, DateTime.Now, table);

            // Assign order to waiterA
            waiterA.AddOrder(order);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () => waiterB.AddOrder(order),
                "Order already belongs to another Waiter, so adding it to a different Waiter should throw an exception.");
        }
    }
}
