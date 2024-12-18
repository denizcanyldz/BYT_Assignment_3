using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{ 
     [TestFixture]
    public class ManagerTests 
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the extent and total count before each test
            Manager.SetAll(new List<Manager>());
            // Manager.TotalManagers = 0; // Removed due to private setter
        }

        [TearDown]
        public void TearDown()
        {
            // Reset the extent and total count after each test to ensure test isolation
            Manager.SetAll(new List<Manager>());
            // Manager.TotalManagers = 0; // Removed due to private setter
        }

        [Test]
        public void Manager_CreatesObjectCorrectly()
        {
            // Arrange & Act
            var manager = new Manager(
                staffID: 1,
                name: "Sam Smith",
                department: "HR" // Using named parameter to correctly assign department
            );

            // Since ManagerID is aligned with StaffID, no need to set it separately

            // Assert
            Assert.That(manager.StaffID, Is.EqualTo(1), "StaffID should be set correctly.");
            Assert.That(manager.Name, Is.EqualTo("Sam Smith"), "Name should be set correctly.");
            Assert.That(manager.Department, Is.EqualTo("HR"), "Department should be set correctly.");
            // If ManagerID is aligned with StaffID, you can assert it as well
            // Assert.That(manager.ManagerID, Is.EqualTo(manager.StaffID), "ManagerID should align with StaffID.");
        }

        [Test]
        public void Manager_ThrowsExceptionForNegativeTotalManagers()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Manager.TotalManagers = -1);
            Assert.AreEqual("TotalManagers cannot be negative.", ex.Message, "Exception message should indicate that TotalManagers cannot be negative.");
        }

        [Test]
        public void Manager_IsCorrectlySavedInExtent()
        {
            // Arrange
            var manager = new Manager(
                staffID: 2,
                name: "Jamie Lee"
                // department is optional and defaults to null
            );

            // Act
            var allManagers = Manager.GetAll();

            // Assert
            Assert.That(allManagers, Contains.Item(manager), "Manager should be correctly saved in the extent.");
        }
    }
}