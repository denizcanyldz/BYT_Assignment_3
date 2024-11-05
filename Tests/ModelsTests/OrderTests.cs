using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class OrderTests
    {
        [SetUp]
        public void SetUp()
        {
            
            Order.SetAll(new List<Order>());
            Order.TotalOrders = 0;
        }

        [Test]
        public void Constructor_ShouldInitializeOrderCorrectly()
        {
            var orderDate = DateTime.Now;
            var order = new Order(1, orderDate, "Initial notes", "DISCOUNT01");

            Assert.That(order.OrderID, Is.EqualTo(1));
            Assert.That(order.OrderDate, Is.EqualTo(orderDate));
            Assert.That(order.Notes, Is.EqualTo("Initial notes"));
            Assert.That(order.DiscountCode, Is.EqualTo("DISCOUNT01"));
            Assert.That(Order.TotalOrders, Is.EqualTo(1));
        }

        [Test]
        public void OrderDate_ShouldThrowException_WhenInFuture()
        {
            var futureDate = DateTime.Now.AddDays(1);
            var ex = Assert.Throws<ArgumentException>(() => new Order(1, futureDate));
            Assert.That(ex.Message, Is.EqualTo("OrderDate cannot be in the future."));
        }

        [Test]
        public void Notes_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longNotes = new string('a', 501);
            var ex = Assert.Throws<ArgumentException>(() => new Order(1, DateTime.Now, longNotes));
            Assert.That(ex.Message, Is.EqualTo("Notes length cannot exceed 500 characters."));
        }

        [Test]
        public void DiscountCode_ShouldThrowException_WhenInvalidFormat()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Order(1, DateTime.Now, null, "INVALIDCODE123"));
            Assert.That(ex.Message, Is.EqualTo("Invalid DiscountCode format."));
        }

        [Test]
        public void AddOrderItem_ShouldAddItemToOrder()
        {
            var order = new Order(1, DateTime.Now);
            var orderItem = new OrderItem(1, "Item 1", 2, 10.0);
            order.AddOrderItem(orderItem);

            Assert.That(order.OrderItems.Count, Is.EqualTo(1));
            Assert.That(order.OrderItems[0], Is.EqualTo(orderItem));
        }

        [Test]
        public void AddOrderItem_ShouldThrowException_WhenItemIsNull()
        {
            var order = new Order(1, DateTime.Now);
            var ex = Assert.Throws<ArgumentException>(() => order.AddOrderItem(null));
            Assert.That(ex.Message, Is.EqualTo("OrderItem cannot be null."));
        }

        [Test]
        public void RemoveOrderItem_ShouldRemoveItemFromOrder()
        {
            var order = new Order(1, DateTime.Now);
            var orderItem = new OrderItem(1, "Item 1", 2, 10.0);
            order.AddOrderItem(orderItem);

            order.RemoveOrderItem(orderItem);
            Assert.That(order.OrderItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveOrderItem_ShouldThrowException_WhenItemNotInOrder()
        {
            var order = new Order(1, DateTime.Now);
            var orderItem = new OrderItem(1, "Item 1", 2, 10.0);

            var ex = Assert.Throws<ArgumentException>(() => order.RemoveOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem not found."));
        }

        [Test]
        public void TotalPrice_ShouldCalculateCorrectly()
        {
            var order = new Order(1, DateTime.Now);
            var item1 = new OrderItem(1, "Item 1", 2, 10.0); // 2 * 10.0 = 20.0
            var item2 = new OrderItem(2, "Item 2", 1, 15.0); // 1 * 15.0 = 15.0
            order.AddOrderItem(item1);
            order.AddOrderItem(item2);

            Assert.That(order.TotalPrice, Is.EqualTo(35.0));
        }

        [Test]
        public void GetAll_ShouldReturnAllOrders()
        {
            var order1 = new Order(1, DateTime.Now);
            var order2 = new Order(2, DateTime.Now);

            var allOrders = Order.GetAll();
            Assert.That(allOrders.Count, Is.EqualTo(2));
            Assert.Contains(order1, (System.Collections.ICollection)allOrders);
            Assert.Contains(order2, (System.Collections.ICollection)allOrders);
        }

        [Test]
        public void SetAll_ShouldUpdateOrdersListCorrectly()
        {
            var order1 = new Order(1, DateTime.Now);
            var order2 = new Order(2, DateTime.Now);

            var newOrders = new List<Order> { order1 };
            Order.SetAll(newOrders);

            var allOrders = Order.GetAll();
            Assert.That(allOrders.Count, Is.EqualTo(1));
            Assert.Contains(order1, (System.Collections.ICollection)allOrders);
            Assert.That(Order.TotalOrders, Is.EqualTo(1)    );
        }
    }
}
