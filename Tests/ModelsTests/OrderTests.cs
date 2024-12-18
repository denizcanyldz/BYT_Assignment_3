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
        }

        [Test]
        public void Order_CreatesObjectCorrectly_WithMandatoryAttributes()
        {
            // Arrange
            var table = new Table(1, "A1", 4);
            var orderDate = DateTime.Now.AddHours(-2);
            var order = new Order(1, orderDate, table, "No onions", "DISC10CODE");

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
            var table = new Table(2, 2, "B2");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Order(-1, DateTime.Now.AddHours(-1), table));
            Assert.That(ex.Message, Is.EqualTo("OrderID must be greater than zero."), "Exception message should indicate invalid OrderID.");
        }

        [Test]
        public void Order_ThrowsExceptionForFutureOrderDate()
        {
            // Arrange
            var table = new Table(3, 6,"C3");
            var futureDate = DateTime.Now.AddHours(1);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Order(2, futureDate, table));
            Assert.That(ex.Message, Is.EqualTo("OrderDate cannot be in the future."), "Exception message should indicate invalid OrderDate.");
        }

        [Test]
        public void Order_AddsOrderItem_Correctly()
        {
            // Arrange
            var table = new Table(4, "D4", 3);
            var order = new Order(3, DateTime.Now.AddHours(-3), table);
            var orderItem = order.CreateOrderItem(1, "Burger", 2, 5.99, "Extra cheese");

            // Act & Assert
            Assert.Contains(orderItem, (System.Collections.ICollection)order.OrderItems, "OrderItem should be added to the Order.");
            Assert.AreEqual(1, order.OrderItems.Count, "Order should have one OrderItem.");
            Assert.AreEqual(11.98, order.TotalAmount, "TotalAmount should be calculated correctly.");
        }

        [Test]
        public void Order_RemovesOrderItem_Correctly()
        {
            // Arrange
            var table = new Table(5, "E5", 5);
            var order = new Order(4, DateTime.Now.AddHours(-4), table);
            var orderItem1 = order.CreateOrderItem(2, "Pizza", 1, 8.99, "No olives");
            var orderItem2 = order.CreateOrderItem(3, "Salad", 3, 4.50, null);

            // Act
            order.RemoveOrderItem(orderItem1);

            // Assert
            Assert.IsFalse(order.OrderItems.Contains(orderItem1), "OrderItem1 should be removed from the Order.");
            Assert.Contains(orderItem2, (System.Collections.ICollection)order.OrderItems, "OrderItem2 should remain in the Order.");
            Assert.AreEqual(1, order.OrderItems.Count, "Order should have one OrderItem after removal.");
            Assert.AreEqual(13.50, order.TotalAmount, "TotalAmount should be updated correctly.");
        }

        [Test]
        public void Order_AddDuplicateOrderItem_ShouldThrowException()
        {
            // Arrange
            var table = new Table(6, "F6", 2);
            var order = new Order(5, DateTime.Now.AddHours(-5), table);
            var orderItem = order.CreateOrderItem(4, "Steak", 1, 15.99, "Medium rare");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.AddOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem already exists in the order."), "Exception message should indicate duplicate OrderItem.");
        }

        [Test]
        public void Order_RemoveNonExistentOrderItem_ShouldThrowException()
        {
            // Arrange
            var table = new Table(7, "G7", 4);
            var order = new Order(6, DateTime.Now.AddHours(-6), table);
            var orderItem = new OrderItem(5, "Soup", 1, 3.99, null); // Not added to the order

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.RemoveOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem not found in the order."), "Exception message should indicate non-existent OrderItem.");
        }

        [Test]
        public void Order_TotalAmount_CalculatesCorrectly()
        {
            // Arrange
            var table = new Table(8, "H8", 2);
            var order = new Order(7, DateTime.Now.AddHours(-7), table);
            order.CreateOrderItem(6, "Pasta", 2, 7.50, "Gluten-free");
            order.CreateOrderItem(7, "Wine", 1, 12.00, "Red");
            order.CreateOrderItem(8, "Dessert", 3, 4.00, null);

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
            var table = new Table(9, "I9", 6);
            var order = new Order(8, DateTime.Now.AddHours(-8), table);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.DiscountCode = "INVALID!@#");
            Assert.That(ex.Message, Is.EqualTo("Invalid DiscountCode format."), "Exception message should indicate invalid DiscountCode.");
        }

        [Test]
        public void Order_SetValidDiscountCode_ShouldSetSuccessfully()
        {
            // Arrange
            var table = new Table(10, "J10", 4);
            var order = new Order(9, DateTime.Now.AddHours(-9), table);

            // Act
            order.DiscountCode = "DISC123456";

            // Assert
            Assert.AreEqual("DISC123456", order.DiscountCode, "DiscountCode should be set correctly.");
        }

        [Test]
        public void Order_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var table1 = new Table(11, "K11", 2);
            var table2 = new Table(12, "L12", 3);
            var orderDate1 = new DateTime(2023, 10, 10, 12, 0, 0);
            var orderDate2 = new DateTime(2023, 10, 11, 13, 30, 0);

            var order1 = new Order(10, orderDate1, table1, "Fast delivery", "SAVE10CODE");
            var order2 = new Order(10, orderDate1, table1, "Fast delivery", "SAVE10CODE");
            var order3 = new Order(11, orderDate2, table2, null, null);

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
            var table = new Table(13, "M13", 5);
            var orderDate = DateTime.Now.AddHours(-10);
            var order = new Order(12, orderDate, table);

            // Act & Assert
            Assert.IsNull(order.Notes, "Notes should be null when not provided.");
            Assert.IsNull(order.DiscountCode, "DiscountCode should be null when not provided.");
        }
    }
}