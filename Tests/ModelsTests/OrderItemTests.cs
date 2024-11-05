using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class OrderItemTests
    {
        [SetUp]
        public void SetUp()
        {
            OrderItem.SetAll(new List<OrderItem>());
        }

        [Test]
        public void OrderItem_CreatesObjectCorrectly()
        {
            var orderItem = new OrderItem(1, "Burger", 2, 5.0, "No onions");
            Assert.That(orderItem.OrderItemID, Is.EqualTo(1));
            Assert.That(orderItem.ItemName, Is.EqualTo("Burger"));
            Assert.That(orderItem.Quantity, Is.EqualTo(2));
            Assert.That(orderItem.Price, Is.EqualTo(5.0));
            Assert.That(orderItem.SpecialInstructions, Is.EqualTo("No onions"));
        }

        [Test]
        public void OrderItem_ThrowsExceptionForNegativeTotalOrderItems()
        {
            Assert.Throws<ArgumentException>(() => OrderItem.TotalOrderItems = -1);
        }

        [Test]
        public void OrderItem_ThrowsExceptionForZeroOrNegativeQuantity()
        {
            Assert.Throws<ArgumentException>(() => new OrderItem(2, "Burger", 0, 5.0));
            Assert.Throws<ArgumentException>(() => new OrderItem(2, "Burger", -1, 5.0));
        }

        [Test]
        public void OrderItem_IsCorrectlySavedInExtent()
        {
            var orderItem = new OrderItem(3, "Pizza", 1, 10.0);
            Assert.That(OrderItem.GetAll(), Contains.Item(orderItem));
        }
    }
}