using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
     [TestFixture]
    public class CustomerTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the class extent before each test
            Customer.SetAll(new List<Customer>());
        }

        [Test]
        public void Customer_CreatesObjectCorrectly()
        {
            // Arrange & Act
            var customer = new Customer(1, "Michael", "1234567890");

            // Assert
            Assert.That(customer.CustomerID, Is.EqualTo(1), "CustomerID should be set correctly.");
            Assert.That(customer.Name, Is.EqualTo("Michael"), "Name should be set correctly.");
            Assert.That(customer.ContactNumber, Is.EqualTo("1234567890"), "ContactNumber should be set correctly.");
        }

        [Test]
        public void Customer_ThrowsExceptionForEmptyName()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Customer(2, ""));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be null or empty."));
        }

        [Test]
        public void Customer_ThrowsExceptionForInvalidContactNumber()
        {
            // Arrange
            var longContactNumber = new string('1', 51); // 51 characters

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Customer(3, "Sarah", longContactNumber));
            Assert.That(ex.Message, Is.EqualTo("ContactNumber length cannot exceed 50 characters."));
        }

        [Test]
        public void Customer_IsCorrectlySavedInExtent()
        {
            // Arrange
            var customer1 = new Customer(4, "Sarah");
            var customer2 = new Customer(5, "John Doe", "0987654321");

            // Act
            var allCustomers = Customer.GetAll();

            // Assert
            Assert.That(allCustomers, Contains.Item(customer1), "Customer1 should be in the extent.");
            Assert.That(allCustomers, Contains.Item(customer2), "Customer2 should be in the extent.");
            Assert.That(allCustomers.Count, Is.EqualTo(2), "Extent should contain exactly 2 customers.");
        }
    }
}