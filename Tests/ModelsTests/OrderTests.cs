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
            OrderItem.SetAll(new List<OrderItem>());
        }

        [Test]
        public void Order_CreatesObjectCorrectly()
        {
            var table = new Table(1, 4, "Main Hall", "Round");
            var order = new Order(1, DateTime.Now, table);
            Assert.That(order.OrderID, Is.EqualTo(1));
            Assert.That(order.Table, Is.EqualTo(table));
            Assert.That(order.OrderDate.Date, Is.EqualTo(DateTime.Now.Date));
        }

        [Test]
        public void Order_TotalOrdersCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Order.TotalOrders = -1);
        }

        [Test]
        public void Order_OrderDateCannotBeInFuture()
        {
            var table = new Table(2, 6, "VIP Section", "Square");
            Assert.Throws<ArgumentException>(() => new Order(2, DateTime.Now.AddDays(1), table));
        }

        [Test]
        public void Order_IsCorrectlySavedInExtent()
        {
            var table = new Table(3, 2, "Outdoor", "Small Round");
            var order = new Order(3, DateTime.Now, table);
            Assert.That(Order.GetAll(), Contains.Item(order));
        }

        [Test]
        public void Order_AddOrderItemAddsCorrectly()
        {
            var table = new Table(4, 4, "Patio", "Rectangle");
            var order = new Order(4, DateTime.Now, table);
            var orderItem = new OrderItem(1, "Burger", 2, 10.0, "No onions"); // ItemName, Quantity, Price, SpecialInstructions
            order.AddOrderItem(orderItem);
            Assert.That(order.OrderItems, Contains.Item(orderItem));
        }

        [Test]
        public void Order_RemoveOrderItemRemovesCorrectly()
        {
            var table = new Table(5, 6, "Garden", "Hexagon");
            var order = new Order(5, DateTime.Now, table);
            var orderItem = new OrderItem(2, "Salad", 1, 8.0, "Dressing on the side");
            order.AddOrderItem(orderItem);
            order.RemoveOrderItem(orderItem);
            Assert.That(order.OrderItems, Does.Not.Contain(orderItem));
        }

        [Test]
        public void Order_TotalAmountCalculatesCorrectly()
        {
            var table = new Table(6, 6, "Rooftop", "Oval");
            var order = new Order(6, DateTime.Now, table);
            order.AddOrderItem(new OrderItem(3, "Pizza", 2, 5.0)); // Quantity 2, Price 5.0
            order.AddOrderItem(new OrderItem(4, "Pasta", 1, 10.0)); // Quantity 1, Price 10.0
            Assert.That(order.TotalAmount, Is.EqualTo(20.0));
        }
    }
}