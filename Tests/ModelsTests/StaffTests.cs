using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class StaffTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset static fields before each test to ensure test isolation
            Restaurant.SetAll(new List<Restaurant>());
            Restaurant.TotalRestaurants = 0;
            Staff.SetAll(new List<Staff>());
            Staff.TotalStaff = 0;
        }

        // Derived class for testing because Staff is abstract
        private class TestStaff : Staff
        {
            public TestStaff(int staffID, string name, string? contactNumber = null)
                : base(staffID, name, contactNumber) { }

            public TestStaff() { } // Parameterless constructor for serialization
        }

        [Test]
        public void Constructor_ShouldInitializeStaffCorrectly()
        {
            // Arrange & Act
            var staff = new TestStaff(1, "John Doe", "Morning");

            // Assert
            Assert.That(staff.StaffID, Is.EqualTo(1), "StaffID should be correctly assigned.");
            Assert.That(staff.Name, Is.EqualTo("John Doe"), "Name should be correctly assigned.");
            Assert.That(staff.ContactNumber, Is.EqualTo("Morning"), "ContactNumber should be correctly assigned.");
            Assert.That(Staff.TotalStaff, Is.EqualTo(1), "TotalStaff should be incremented correctly.");
        }

        [Test]
        public void Name_ShouldThrowException_WhenNullOrEmpty()
        {
            // Arrange, Act & Assert for Empty Name
            var exEmpty = Assert.Throws<ArgumentException>(() => new TestStaff(1, "", "Morning"));
            Assert.That(exEmpty.Message, Is.EqualTo("Name cannot be null or empty."), "Exception message should indicate that Name cannot be null or empty.");

            // Arrange, Act & Assert for Null Name (if allowed)
            // If Name is non-nullable and the constructor doesn't accept null, this test might be unnecessary.
            // Otherwise, uncomment the following lines:
            /*
            var exNull = Assert.Throws<ArgumentNullException>(() => new TestStaff(2, null, "Morning"));
            Assert.That(exNull.Message, Does.Contain("Name cannot be null."), "Exception message should indicate that Name cannot be null.");
            */
        }

        [Test]
        public void RemoveStaff_ShouldThrowException_WhenStaffNotFound()
        {
            // Arrange
            var staff1 = new TestStaff(1, "John Doe");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Staff.RemoveStaff(staff1));
            Assert.That(ex.Message, Is.EqualTo("Staff member not found."), "Exception message should indicate that Staff member was not found.");
        }

        [Test]
        public void SetAll_ShouldUpdateStaffListCorrectly()
        {
            // Arrange
            var staff1 = new TestStaff(1, "John Doe");
            var newStaffList = new List<Staff> { staff1 };

            // Act
            Staff.SetAll(newStaffList);

            // Assert
            var allStaff = Staff.GetAll();
            Assert.That(allStaff.Count, Is.EqualTo(1), "GetAll should return the correct number of staff members.");
            CollectionAssert.Contains(allStaff, staff1, "GetAll should contain the added staff member.");
            Assert.That(Staff.TotalStaff, Is.EqualTo(1), "TotalStaff should reflect the correct count after SetAll.");
        }

        [Test]
        public void SetAll_ShouldReplaceExistingStaffList()
        {
            // Arrange
            var staff1 = new TestStaff(1, "John Doe");
            var staff2 = new TestStaff(2, "Jane Smith");
            Staff.SetAll(new List<Staff> { staff1 });

            // Act
            Staff.SetAll(new List<Staff> { staff2 });

            // Assert
            var allStaff = Staff.GetAll();
            Assert.That(allStaff.Count, Is.EqualTo(1), "SetAll should replace the existing staff list.");
            CollectionAssert.DoesNotContain(allStaff, staff1, "SetAll should remove previous staff members.");
            CollectionAssert.Contains(allStaff, staff2, "SetAll should include new staff members.");
            Assert.That(Staff.TotalStaff, Is.EqualTo(1), "TotalStaff should reflect the updated count after SetAll.");
        }

        [Test]
        public void TotalStaff_ShouldThrowException_WhenSetToNegative()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Staff.TotalStaff = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalStaff cannot be negative."), "Exception message should indicate that TotalStaff cannot be negative.");
        }

        [Test]
        public void RemoveStaff_ShouldUpdateTotalStaffCount()
        {
            // Arrange
            var staff = new TestStaff(1, "John Doe");
            Assert.That(Staff.TotalStaff, Is.EqualTo(1), "TotalStaff should be incremented after adding a staff member.");

            // Act
            Staff.RemoveStaff(staff);

            // Assert
            Assert.That(Staff.TotalStaff, Is.EqualTo(0), "TotalStaff should be decremented after removing a staff member.");
        }

        [Test]
        public void GetAll_ShouldReturnAllStaffMembers()
        {
            // Arrange
            var staff1 = new TestStaff(1, "John Doe");
            var staff2 = new TestStaff(2, "Jane Smith");

            // Act
            var allStaff = Staff.GetAll();

            // Assert
            Assert.That(allStaff.Count, Is.EqualTo(2), "GetAll should return all added staff members.");
            CollectionAssert.Contains(allStaff, staff1, "GetAll should contain staff1.");
            CollectionAssert.Contains(allStaff, staff2, "GetAll should contain staff2.");
        }

        [Test]
        public void Name_ShouldThrowException_WhenNull()
        {
            // Arrange, Act & Assert
            // Depending on Staff implementation, this might throw ArgumentException or ArgumentNullException
            var ex = Assert.Throws<ArgumentException>(() => new TestStaff(3, null!));
            Assert.That(ex.Message, Is.EqualTo("Name cannot be null or empty."), "Exception message should indicate that Name cannot be null or empty.");
        }

        [Test]
        public void AssignSupervisor_ShouldAssociateSupervisorCorrectly()
        {
            // Arrange
            var restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            var supervisor = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            var subordinate = new TestStaff(2, "Elizabeth Bennett", "020-7946-0958");

            // Act
            subordinate.AssignSupervisor(supervisor);

            // Assert
            Assert.AreEqual(supervisor, subordinate.Supervisor, "Subordinate's Supervisor should be correctly assigned.");
            Assert.That(supervisor.Subordinates, Does.Contain(subordinate), "Supervisor's Subordinates should include the subordinate.");
        }

        [Test]
        public void AssignSupervisor_ShouldThrowException_WhenAssigningSelf()
        {
            // Arrange
            var restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            var staff = new TestStaff(1, "John Doe", "Morning");

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => staff.AssignSupervisor(staff));
            Assert.That(ex.Message, Is.EqualTo("Staff member cannot supervise themselves."), "Exception message should indicate that a Staff member cannot supervise themselves.");
        }

        [Test]
        public void AssignSupervisor_ShouldThrowException_WhenSupervisorIsNull()
        {
            // Arrange
            var subordinate = new TestStaff(2, "Jane Smith", "Evening");

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => subordinate.AssignSupervisor(null!));
            Assert.That(ex.Message, Does.Contain("Supervisor cannot be null."), "Exception message should indicate that Supervisor cannot be null.");
        }

        [Test]
        public void AssignSupervisor_ShouldPreventCircularSupervisoryRelationship()
        {
            // Arrange
            var restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            var staff1 = new TestStaff(1, "John Doe", "Morning");
            var staff2 = new TestStaff(2, "Jane Smith", "Evening");

            // Act
            staff1.AssignSupervisor(staff2);

            // Attempt to create a circular supervisory relationship
            var ex = Assert.Throws<InvalidOperationException>(() => staff2.AssignSupervisor(staff1));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Assigning this supervisor creates a circular supervisory relationship."), "Exception message should indicate that assigning this supervisor creates a circular supervisory relationship.");
        }

        [Test]
        public void AssignMultipleSubordinates_ToSupervisor_ShouldSucceed()
        {
            // Arrange
            var restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            var supervisor = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            var subordinate1 = new TestStaff(2, "Elizabeth Bennett", "020-7946-0958");
            var subordinate2 = new TestStaff(3, "Emma Woodhouse", "020-7946-0958");

            // Act
            subordinate1.AssignSupervisor(supervisor);
            subordinate2.AssignSupervisor(supervisor);

            // Assert
            Assert.AreEqual(supervisor, subordinate1.Supervisor, "Subordinate1's Supervisor should be correctly assigned.");
            Assert.AreEqual(supervisor, subordinate2.Supervisor, "Subordinate2's Supervisor should be correctly assigned.");
            Assert.That(supervisor.Subordinates, Does.Contain(subordinate1), "Supervisor's Subordinates should include subordinate1.");
            Assert.That(supervisor.Subordinates, Does.Contain(subordinate2), "Supervisor's Subordinates should include subordinate2.");
            Assert.AreEqual(2, supervisor.Subordinates.Count, "Supervisor should have exactly two subordinates.");
        }
    }
}
