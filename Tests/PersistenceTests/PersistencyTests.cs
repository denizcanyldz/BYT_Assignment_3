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
        private Extents _originalExtents;

        [SetUp]
        public void Setup()
        {
            
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            
            _originalExtents = new Extents
            {
                Customers = new List<Customer> { new Customer { CustomerID = 1, Name = "John Doe" } },
                Chefs = new List<Chef> { new Chef { StaffID = 1, Name = "test asdf" } },
                Orders = new List<Order> { new Order { OrderID = 1, Notes = "some notes" } },
                Bartenders = new List<Bartender> { new Bartender { StaffID = 1, Name = "Thomas" } }
            };
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
            
            Persistence.SaveAll(TestFilePath, _originalExtents);

            
            var loadedExtents = Persistence.LoadAll(TestFilePath);

            
            Assert.Multiple(() =>
            {
                Assert.That(loadedExtents.Customers, Has.Count.EqualTo(_originalExtents.Customers.Count));
                Assert.That(loadedExtents.Customers[0].Name, Is.EqualTo(_originalExtents.Customers[0].Name));
                Assert.That(loadedExtents.Customers[0].CustomerID, Is.EqualTo(_originalExtents.Customers[0].CustomerID));
                Assert.That(loadedExtents.Chefs, Has.Count.EqualTo(_originalExtents.Chefs.Count));
                Assert.That(loadedExtents.Orders, Has.Count.EqualTo(_originalExtents.Orders.Count));
            });
        }

        [Test]
        public void Load_NonExistentFile_ShouldReturnEmptyExtents()
        {
            
            var loadedExtents = Persistence.LoadAll("non_existent_file.xml");

            
            Assert.Multiple(() =>
            {
                Assert.That(loadedExtents, Is.Not.Null);
                Assert.That(loadedExtents.Customers, Is.Empty);
                Assert.That(loadedExtents.Chefs, Is.Empty);
                Assert.That(loadedExtents.Orders, Is.Empty);
                Assert.That(loadedExtents.Bartenders, Is.Empty);
            });
        }

        [Test]
        public void Save_WithInvalidPath_ShouldThrowDirectoryNotFoundException()
        {
            
            var invalidPath = Path.Combine("C:", "invalid_directory", "test.xml");

            
            var exception = Assert.Throws<DirectoryNotFoundException>(() =>
                Persistence.SaveAll(invalidPath, new Extents()));

            Assert.That(exception.Message, Does.Contain("Could not find a part of the path"));
        }

        [Test]
        public void SaveAndLoad_WithNullValues_ShouldHandleGracefully()
        {
            
            var extentsWithNull = new Extents
            {
                Customers = null,
                Orders = new List<Order> { new Order { OrderID = 1 } }
            };

            
            Persistence.SaveAll(TestFilePath, extentsWithNull);
            var loadedExtents = Persistence.LoadAll(TestFilePath);

            
            Assert.Multiple(() =>
            {
                Assert.That(loadedExtents.Customers, Is.Not.Null);
                Assert.That(loadedExtents.Customers, Is.Empty);
                Assert.That(loadedExtents.Orders, Has.Count.EqualTo(1));
            });
        }
        [Test]
        public void Load_CorruptedFile_ShouldReturnEmptyExtents()
        {
            
            File.WriteAllText(TestFilePath, "corrupted xml content");

            
            var loadedExtents = Persistence.LoadAll(TestFilePath);

            
            Assert.Multiple(() =>
            {
                Assert.That(loadedExtents, Is.Not.Null);
                Assert.That(loadedExtents.Customers, Is.Empty);
                Assert.That(loadedExtents.Orders, Is.Empty);
            });
        }


    }
}
