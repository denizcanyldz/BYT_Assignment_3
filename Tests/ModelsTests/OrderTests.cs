using System;
using System.Collections.Generic;
using NUnit.Framework;
using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class OrderTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset all orders before each test to ensure test isolation
            Order.SetAll(new List<Order>());
            Table.SetAll(new List<Table>()); // Reset tables if necessary
            Customer.SetAll(new List<Customer>()); // Reset customers if necessary
        }

        [Test]
        public void Order_CreatesObjectCorrectly_WithMandatoryAttributes()
        {
            // Arrange
            var table = new Table(
                tableNumber: 1,
                maxSeats: 4,
                location: "A1"
                // seatingArrangement is optional and defaults to null
            );

            var orderDate = DateTime.Now.AddHours(-2);
            var order = new Order(
                orderID: 1,
                orderDate: orderDate,
                table: table,
                notes: "No onions",
                discountCode: "DISC10CODE"
            );

            // Act & Assert
            Assert.AreEqual(1, order.OrderID, "OrderID should be set correctly.");
            Assert.AreEqual(orderDate, order.OrderDate, "OrderDate should be set correctly.");
            Assert.AreEqual(table, order.Table, "Table should be associated correctly.");
            Assert.AreEqual("No onions", order.Notes, "Notes should be set correctly.");
            Assert.AreEqual("DISC10CODE", order.DiscountCode, "DiscountCode should be set correctly.");
            Assert.AreEqual(0, order.OrderItems.Count, "Order should initially have no OrderItems.");
            Assert.AreEqual(1, Order.TotalOrders, "TotalOrders should be incremented.");
        }

        [Test]
        public void Order_ThrowsExceptionForInvalidOrderID()
        {
            // Arrange
            var table = new Table(
                tableNumber: 2,
                maxSeats: 2,
                location: "B2"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Order(
                orderID: -1,
                orderDate: DateTime.Now.AddHours(-1),
                table: table
                // notes and discountCode are optional
            ));

            Assert.That(ex.Message, Is.EqualTo("OrderID must be greater than zero."), "Exception message should indicate invalid OrderID.");
        }

        [Test]
        public void Order_ThrowsExceptionForFutureOrderDate()
        {
            // Arrange
            var table = new Table(
                tableNumber: 3,
                maxSeats: 6,
                location: "C3"
            );

            var futureDate = DateTime.Now.AddHours(1);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Order(
                orderID: 2,
                orderDate: futureDate,
                table: table
            ));

            Assert.That(ex.Message, Is.EqualTo("OrderDate cannot be in the future."), "Exception message should indicate invalid OrderDate.");
        }

        [Test]
        public void Order_AddsOrderItem_Correctly()
        {
            // Arrange
            var table = new Table(
                tableNumber: 4,
                maxSeats: 3,
                location: "D4"
            );

            var order = new Order(
                orderID: 3,
                orderDate: DateTime.Now.AddHours(-3),
                table: table
            );

            var orderItem = order.CreateOrderItem(
                orderItemID: 1,
                itemName: "Burger",
                quantity: 2,
                price: 5.99,
                specialInstructions: "Extra cheese"
            );

            // Act & Assert
            Assert.Contains(orderItem, (System.Collections.ICollection)order.OrderItems, "OrderItem should be added to the Order.");
            Assert.AreEqual(1, order.OrderItems.Count, "Order should have one OrderItem.");
            Assert.AreEqual(11.98, order.TotalAmount, "TotalAmount should be calculated correctly.");
        }

        [Test]
        public void Order_RemovesOrderItem_Correctly()
        {
            // Arrange
            var table = new Table(
                tableNumber: 5,
                maxSeats: 5,
                location: "E5"
            );

            var order = new Order(
                orderID: 4,
                orderDate: DateTime.Now.AddHours(-4),
                table: table
            );

            var orderItem1 = order.CreateOrderItem(
                orderItemID: 2,
                itemName: "Pizza",
                quantity: 1,
                price: 8.99,
                specialInstructions: "No olives"
            );

            var orderItem2 = order.CreateOrderItem(
                orderItemID: 3,
                itemName: "Salad",
                quantity: 3,
                price: 4.50,
                specialInstructions: null
            );

            // Act
            order.RemoveOrderItem(orderItem1);

            // Assert
            Assert.IsFalse(order.OrderItems.Contains(orderItem1), "OrderItem1 should be removed from the Order.");
            Assert.Contains(orderItem2, (System.Collections.ICollection)order.OrderItems, "OrderItem2 should remain in the Order.");
            Assert.AreEqual(1, order.OrderItems.Count, "Order should have one OrderItem after removal.");
            Assert.AreEqual(13.50, order.TotalAmount, "TotalAmount should be updated correctly.");
        }
        

        [Test]
        public void Order_RemoveNonExistentOrderItem_ShouldThrowException()
        {
            // Arrange
            var table = new Table(
                tableNumber: 7,
                maxSeats: 4,
                location: "G7"
            );

            var order = new Order(
                orderID: 6,
                orderDate: DateTime.Now.AddHours(-6),
                table: table
            );

            var orderItem = new OrderItem(
                orderItemID: 5,
                itemName: "Soup",
                quantity: 1,
                price: 3.99,
                specialInstructions: null
            ); // Not added to the order

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.RemoveOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem not found in the order."), "Exception message should indicate non-existent OrderItem.");
        }

        [Test]
        public void Order_TotalAmount_CalculatesCorrectly()
        {
            // Arrange
            var table = new Table(
                tableNumber: 8,
                maxSeats: 2,
                location: "H8"
            );

            var order = new Order(
                orderID: 7,
                orderDate: DateTime.Now.AddHours(-7),
                table: table
            );

            order.CreateOrderItem(
                orderItemID: 6,
                itemName: "Pasta",
                quantity: 2,
                price: 7.50,
                specialInstructions: "Gluten-free"
            );

            order.CreateOrderItem(
                orderItemID: 7,
                itemName: "Wine",
                quantity: 1,
                price: 12.00,
                specialInstructions: "Red"
            );

            order.CreateOrderItem(
                orderItemID: 8,
                itemName: "Dessert",
                quantity: 3,
                price: 4.00,
                specialInstructions: null
            );

            var expectedTotal = (2 * 7.50) + (1 * 12.00) + (3 * 4.00); // 15 + 12 + 12 = 39

            // Act
            var actualTotal = order.TotalAmount;

            // Assert
            Assert.AreEqual(expectedTotal, actualTotal, "TotalAmount should be calculated correctly.");
        }

        [Test]
        public void Order_SetInvalidDiscountCode_ShouldThrowException()
        {
            // Arrange
            var table = new Table(
                tableNumber: 9,
                maxSeats: 6,
                location: "I9"
            );

            var order = new Order(
                orderID: 8,
                orderDate: DateTime.Now.AddHours(-8),
                table: table
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.DiscountCode = "INVALID!@#");
            Assert.That(ex.Message, Is.EqualTo("Invalid DiscountCode format."), "Exception message should indicate invalid DiscountCode.");
        }

        [Test]
        public void Order_SetValidDiscountCode_ShouldSetSuccessfully()
        {
            // Arrange
            var table = new Table(
                tableNumber: 10,
                maxSeats: 4,
                location: "J10"
            );

            var order = new Order(
                orderID: 9,
                orderDate: DateTime.Now.AddHours(-9),
                table: table
            );

            // Act
            order.DiscountCode = "DISC123456";

            // Assert
            Assert.AreEqual("DISC123456", order.DiscountCode, "DiscountCode should be set correctly.");
        }

        [Test]
        public void Order_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var table1 = new Table(
                tableNumber: 11,
                maxSeats: 2,
                location: "K11"
            );

            var table2 = new Table(
                tableNumber: 12,
                maxSeats: 3,
                location: "L12"
            );

            var orderDate1 = new DateTime(2023, 10, 10, 12, 0, 0);
            var orderDate2 = new DateTime(2023, 10, 11, 13, 30, 0);

            var order1 = new Order(
                orderID: 10,
                orderDate: orderDate1,
                table: table1,
                notes: "Fast delivery",
                discountCode: "SAVE10CODE"
            );

            var order2 = new Order(
                orderID: 10,
                orderDate: orderDate1,
                table: table1,
                notes: "Fast delivery",
                discountCode: "SAVE10CODE"
            );

            var order3 = new Order(
                orderID: 11,
                orderDate: orderDate2,
                table: table2,
                notes: null,
                discountCode: null
            );

            // Act & Assert
            Assert.IsTrue(order1.Equals(order2), "Orders with identical properties should be equal.");
            Assert.AreEqual(order1.GetHashCode(), order2.GetHashCode(), "HashCodes of identical orders should be equal.");

            Assert.IsFalse(order1.Equals(order3), "Orders with different properties should not be equal.");
            Assert.AreNotEqual(order1.GetHashCode(), order3.GetHashCode(), "HashCodes of different orders should not be equal.");
        }

        [Test]
        public void Order_CreatesObjectWithoutOptionalAttributes()
        {
            // Arrange
            var table = new Table(
                tableNumber: 13,
                maxSeats: 5,
                location: "M13"
            );

            var orderDate = DateTime.Now.AddHours(-10);
            var order = new Order(
                orderID: 12,
                orderDate: orderDate,
                table: table
                // notes and discountCode are optional
            );

            // Act & Assert
            Assert.IsNull(order.Notes, "Notes should be null when not provided.");
            Assert.IsNull(order.DiscountCode, "DiscountCode should be null when not provided.");
        }
    }
}
