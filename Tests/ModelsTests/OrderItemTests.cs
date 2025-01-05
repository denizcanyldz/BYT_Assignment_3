using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class OrderItemTests
    {
        private Table testTable;

        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test to ensure test isolation
            OrderItem.SetAll(new List<OrderItem>());
            Order.SetAll(new List<Order>()); // Reset Orders as well

            // Initialize a common Table instance for use in tests
            testTable = new Table(1, 2);
        }

        [Test]
        public void OrderItem_CreatesObjectCorrectly()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            
            // Act
            var orderItem = order.CreateOrderItem(1, "Burger", 2, 5.0, "No onions");

            // Assert
            Assert.That(orderItem.OrderItemID, Is.EqualTo(1), "OrderItemID should be correctly assigned.");
            Assert.That(orderItem.ItemName, Is.EqualTo("Burger"), "ItemName should be correctly assigned.");
            Assert.That(orderItem.Quantity, Is.EqualTo(2), "Quantity should be correctly assigned.");
            Assert.That(orderItem.Price, Is.EqualTo(5.0), "Price should be correctly assigned.");
            Assert.That(orderItem.SpecialInstructions, Is.EqualTo("No onions"), "SpecialInstructions should be correctly assigned.");
            Assert.That(orderItem.ParentOrder, Is.EqualTo(order), "ParentOrder should be correctly set.");
            Assert.That(OrderItem.TotalOrderItems, Is.EqualTo(1), "TotalOrderItems should be incremented correctly.");
        }

        [Test]
        public void OrderItem_ThrowsExceptionForZeroOrNegativeQuantity()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);

            // Act & Assert for Zero Quantity
            var exZero = Assert.Throws<ArgumentException>(() => order.CreateOrderItem(2, "Burger", 0, 5.0));
            Assert.That(exZero.Message, Is.EqualTo("Quantity must be greater than zero."), "Should throw exception for zero quantity.");

            // Act & Assert for Negative Quantity
            var exNegative = Assert.Throws<ArgumentException>(() => order.CreateOrderItem(3, "Burger", -1, 5.0));
            Assert.That(exNegative.Message, Is.EqualTo("Quantity must be greater than zero."), "Should throw exception for negative quantity.");
        }

        [Test]
        public void OrderItem_IsCorrectlySavedInExtent()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            var orderItem = order.CreateOrderItem(4, "Pizza", 1, 10.0);

            // Act & Assert
            Assert.That(OrderItem.GetAll(), Contains.Item(orderItem), "OrderItem should be present in the class extent.");
            Assert.That(OrderItem.TotalOrderItems, Is.EqualTo(1), "TotalOrderItems should reflect the correct count.");
        }

        [Test]
        public void OrderItem_TotalPrice_CalculatesCorrectly()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            var orderItem = order.CreateOrderItem(5, "Salad", 3, 4.0);

            // Act
            double expectedTotalPrice = 3 * 4.0;

            // Assert
            Assert.That(orderItem.TotalPrice, Is.EqualTo(expectedTotalPrice), "TotalPrice should be correctly calculated based on Quantity and Price.");
        }

        [Test]
        public void OrderItem_AssociatesWithOnlyOneOrder()
        {
            // Arrange
            var order1 = new Order(1, DateTime.Now, testTable);
            var order2 = new Order(2, DateTime.Now, testTable);
            var orderItem = order1.CreateOrderItem(6, "Soda", 2, 1.5);

            // Act & Assert
            // Attempting to associate the same OrderItem with another Order should throw an exception
            var ex = Assert.Throws<ArgumentException>(() => order2.AddOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem already belongs to another Order."), "Should not allow OrderItem to belong to multiple Orders.");
        }

        [Test]
        public void RemovingOrderItem_UpdatesExtentAndOrder()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            var orderItem = order.CreateOrderItem(7, "Ice Cream", 1, 3.0);
            Assert.That(OrderItem.TotalOrderItems, Is.EqualTo(1), "TotalOrderItems should be incremented after adding an OrderItem.");

            // Act
            order.RemoveOrderItem(orderItem);

            // Assert
            Assert.That(OrderItem.GetAll(), Does.Not.Contains(orderItem), "OrderItem should be removed from the class extent.");
            Assert.That(order.OrderItems, Does.Not.Contains(orderItem), "Order's OrderItems should not contain the removed OrderItem.");
            Assert.That(orderItem.ParentOrder, Is.Null, "OrderItem's ParentOrder should be null after removal.");
            Assert.That(OrderItem.TotalOrderItems, Is.EqualTo(0), "TotalOrderItems should be decremented after removing an OrderItem.");
        }

        [Test]
        public void CreatingOrderItem_WithParentOrderIsOptional()
        {
            // Arrange & Act
            var orderItem = new OrderItem(8, "Tea", 1, 2.0);

            // Assert
            Assert.That(orderItem.ParentOrder, Is.Null, "OrderItem should have no ParentOrder when created independently.");
            Assert.That(OrderItem.GetAll(), Contains.Item(orderItem), "OrderItem should be present in the class extent even if not associated with an Order.");
            Assert.That(OrderItem.TotalOrderItems, Is.EqualTo(1), "TotalOrderItems should be incremented correctly.");
        }

        [Test]
        public void AddingDuplicateOrderItem_ShouldThrowException()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            var orderItem = order.CreateOrderItem(9, "Coffee", 1, 2.5);
        
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.AddOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem already exists in the order."), "Should not allow adding duplicate OrderItem to an Order.");
        }


        [Test]
        public void Order_TotalAmount_CalculatesCorrectly()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            order.CreateOrderItem(10, "Steak", 2, 15.0); // Total: 30.0
            order.CreateOrderItem(11, "Wine", 1, 20.0);  // Total: 20.0

            // Act
            double expectedTotalAmount = 30.0 + 20.0;

            // Assert
            Assert.That(order.TotalAmount, Is.EqualTo(expectedTotalAmount), "TotalAmount should correctly sum up all OrderItems' TotalPrice.");
        }

        [Test]
        public void OrderItem_CannotBelongToMultipleOrders()
        {
            // Arrange
            var order1 = new Order(1, DateTime.Now, testTable);
            var order2 = new Order(2, DateTime.Now, testTable);
            var orderItem = order1.CreateOrderItem(12, "Juice", 1, 3.0);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order2.AddOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem already belongs to another Order."), "Should not allow an OrderItem to belong to multiple Orders.");
        }

        [Test]
        public void RemovingOrderItem_ShouldUpdateBothOrderAndOrderItem()
        {
            // Arrange
            var order = new Order(1, DateTime.Now, testTable);
            var orderItem = order.CreateOrderItem(13, "Cake", 1, 5.0);

            // Act
            order.RemoveOrderItem(orderItem);

            // Assert
            Assert.That(order.OrderItems, Does.Not.Contain(orderItem), "Order's OrderItems should not contain the removed OrderItem.");
            Assert.That(orderItem.ParentOrder, Is.Null, "OrderItem's ParentOrder should be null after removal.");
            Assert.That(OrderItem.GetAll(), Does.Not.Contain(orderItem), "OrderItem should be removed from the class extent.");
            Assert.That(OrderItem.TotalOrderItems, Is.EqualTo(0), "TotalOrderItems should be decremented after removal.");
        }
    }
}
