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
        }

        [Test]
        public void Order_CreatesObjectCorrectly()
        {
            var order = new Order(1, DateTime.Now, "Special instructions", "DISCOUNT10");
            Assert.That(order.OrderID, Is.EqualTo(1));
            Assert.That(order.OrderDate.Date, Is.EqualTo(DateTime.Now.Date));
            Assert.That(order.Notes, Is.EqualTo("Special instructions"));
            Assert.That(order.DiscountCode, Is.EqualTo("DISCOUNT10"));
        }

        [Test]
        public void Order_ThrowsExceptionForFutureOrderDate()
        {
            Assert.Throws<ArgumentException>(() => new Order(2, DateTime.Now.AddDays(1)));
        }

        [Test]
        public void Order_IsCorrectlySavedInExtent()
        {
            var order = new Order(3, DateTime.Now);
            Assert.That(Order.GetAll(), Contains.Item(order));
        }
    }
}