using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class StaffReflexiveAssociationTests
    {
        private Staff manager;
        private Staff employee;

        [SetUp]
        public void Setup()
        {
            // Reset state before each test
            Staff.SetAll(new List<Staff>());

            manager = new TestStaff(1, "John Manager", "123456789");
            employee = new TestStaff(2, "Jane Employee", "987654321");
        }

        [Test]
        public void SetSupervisor_AssociationIsLinkedBothWays()
        {
            // Act
            employee.SetSupervisor(manager);

            // Assert
            Assert.That(employee.Supervisor, Is.EqualTo(manager), "Employee's supervisor should be set to the manager.");
            Assert.That(manager.Supervisees, Contains.Item(employee), "Manager's supervisee list should include the employee.");
        }

        [Test]
        public void AddSupervisee_AssociationIsLinkedBothWays()
        {
            // Act
            manager.AddSupervisee(employee);

            // Assert
            Assert.That(manager.Supervisees, Contains.Item(employee), "Manager's supervisee list should include the employee.");
            Assert.That(employee.Supervisor, Is.EqualTo(manager), "Employee's supervisor should be set to the manager.");
        }

        [Test]
        public void RemoveSupervisor_AssociationIsUnlinkedBothWays()
        {
            // Arrange
            employee.SetSupervisor(manager);

            // Act
            employee.RemoveSupervisor();

            // Assert
            Assert.That(employee.Supervisor, Is.Null, "Employee's supervisor should be null after removal.");
            Assert.That(manager.Supervisees, Does.Not.Contain(employee), "Manager's supervisee list should not include the employee.");
        }

        [Test]
        public void RemoveSupervisee_AssociationIsUnlinkedBothWays()
        {
            // Arrange
            manager.AddSupervisee(employee);

            // Act
            manager.RemoveSupervisee(employee);

            // Assert
            Assert.That(manager.Supervisees, Does.Not.Contain(employee), "Manager's supervisee list should not include the employee.");
            Assert.That(employee.Supervisor, Is.Null, "Employee's supervisor should be null after removal.");
        }

        [Test]
        public void SetSupervisor_SameObject_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => manager.SetSupervisor(manager),
                "A staff member cannot supervise themselves.");
        }

        [Test]
        public void AddSupervisee_SameObject_ShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => manager.AddSupervisee(manager),
                "A staff member cannot supervise themselves.");
        }

        [Test]
        public void UpdateSupervisor_ReplacesExistingSupervisorProperly()
        {
            // Arrange
            var newManager = new TestStaff(3, "Alice NewManager", "555555555");

            // Act
            employee.SetSupervisor(manager);
            employee.SetSupervisor(newManager);

            // Assert
            Assert.That(employee.Supervisor, Is.EqualTo(newManager), "Employee's supervisor should be updated to the new manager.");
            Assert.That(manager.Supervisees, Does.Not.Contain(employee), "Old manager's supervisee list should not include the employee.");
            Assert.That(newManager.Supervisees, Contains.Item(employee), "New manager's supervisee list should include the employee.");
        }

        [Test]
        public void AddSupervisee_NullShouldThrowException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => manager.AddSupervisee(null),
                "Adding a null supervisee should throw an exception.");
        }

        [Test]
        public void RemoveNonexistentSupervisee_ShouldNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => manager.RemoveSupervisee(employee),
                "Removing a non-existent supervisee should not throw an exception.");
        }
    }

    /// <summary>
    /// Test implementation of the abstract Staff class for testing
    /// </summary>
    public class TestStaff : Staff
    {
        public TestStaff(int staffID, string name, string contactNumber = null)
            : base(staffID, name, contactNumber)
        {
        }
    }
}
