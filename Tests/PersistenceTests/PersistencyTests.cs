using NUnit.Framework;
using BYT_Assignment_3.Persistences;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using BYT_Assignment_3;

namespace Tests.PersistenceTests
{
    [TestFixture]
    public class PersistencyTests
    {
        private const string TestFilePath = "test_extents.xml";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        [Test]
        public void SaveAndLoad_AllData_ShouldPersistCorrectly()
        {
            var originalExtents = new Extents
            {
                Customers = new List<Customer> { new Customer { CustomerID = 1, Name = "John Doe" } },
                Chefs = new List<Chef> { new Chef { StaffID = 1, Name = "test asdf" } },
                Orders = new List<Order> { new Order { OrderID = 1, Notes="some notes"} },
                Bartenders = new List<Bartender> { new Bartender { StaffID = 1, Name = "Thomes"} }
            };

            Persistence.SaveAll(TestFilePath, originalExtents);
            var loadedExtents = Persistence.LoadAll(TestFilePath);

            Assert.That(loadedExtents.Customers.Count, Is.EqualTo(originalExtents.Customers.Count));
            Assert.That(loadedExtents.Customers[0].Name, Is.EqualTo(originalExtents.Customers[0].Name));
            Assert.That(loadedExtents.Chefs.Count, Is.EqualTo(originalExtents.Chefs.Count));
            Assert.That(loadedExtents.Orders.Count, Is.EqualTo(originalExtents.Orders.Count));
        }

        [Test]
        public void Load_NonExistentFile_ShouldReturnEmptyExtents()
        {
            var loadedExtents = Persistence.LoadAll("non_existent_file.xml");

            Assert.IsNotNull(loadedExtents);
            Assert.IsEmpty(loadedExtents.Customers);
            Assert.IsEmpty(loadedExtents.Chefs);
            
        }

        [Test]
        public void Save_WithInvalidPath_ShouldThrowDirectoryNotFoundException()
        {

            var extents = new Extents();

            var exception = Assert.Throws<DirectoryNotFoundException>(() => Persistence.SaveAll("C:/invalid_path/test.xml", extents));
            Assert.That(exception.Message, Does.Contain("Could not find a part of the path"));
        }

        [Test]
        public void SaveAndLoad_WithComplexData_ShouldPersistAllFields()
        {
            var originalExtents = new Extents
            {
                Customers = new List<Customer>
                {
                    new Customer { CustomerID = 1, Name = "Alice", PhoneNumber = "1234567890", Email = "alice@example.com" }
                },
                Feedbacks = new List<Feedback>
                {
                    new Feedback { FeedbackID = 1, CustomerID = 1, Rating = 5, Comments = "Excellent!" }
                }
            };

            Persistence.SaveAll(TestFilePath, originalExtents);
            var loadedExtents = Persistence.LoadAll(TestFilePath);

            Assert.That(loadedExtents.Customers[0].Email, Is.EqualTo(originalExtents.Customers[0].Email));
            Assert.That(loadedExtents.Feedbacks[0].Rating, Is.EqualTo(originalExtents.Feedbacks[0].Rating));
            Assert.That(loadedExtents.Feedbacks[0].Comments, Is.EqualTo(originalExtents.Feedbacks[0].Comments));
        }
    }
}

