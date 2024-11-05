using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class CustomerTests
    {
        [SetUp]
        public void SetUp()
        {
            Customer.SetAll(new List<Customer>());
        }

        [Test]
        public void Customer_CreatesObjectCorrectly()
        {
            var customer = new Customer(1, "Michael", "michael@example.com", "1234567890");
            Assert.That(customer.CustomerID, Is.EqualTo(1));
            Assert.That(customer.Name, Is.EqualTo("Michael"));
            Assert.That(customer.Email, Is.EqualTo("michael@example.com"));
            Assert.That(customer.PhoneNumber, Is.EqualTo("1234567890"));
        }

        [Test]
        public void Customer_ThrowsExceptionForEmptyName()
        {
            Assert.Throws<ArgumentException>(() => new Customer(1, ""));
        }

        [Test]
        public void Customer_ThrowsExceptionForInvalidEmail()
        {
            Assert.Throws<ArgumentException>(() => new Customer(1, "Michael", "invalidemail"));
        }

        [Test]
        public void Customer_IsCorrectlySavedInExtent()
        {
            var customer = new Customer(2, "Sarah");
            Assert.That(Customer.GetAll(), Contains.Item(customer));
        }
    }
}