using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.AssociationTests.StaffReflexive
{
    [TestFixture]
    public class StaffReflexiveAssociationTests
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

        [Test]
        public void TestAssignSupervisor()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff supervisor = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff subordinate = new Waiter(2, "Elizabeth Bennett", "020-7946-0958", restaurant);

            // Act
            subordinate.AssignSupervisor(supervisor);

            // Assert
            Assert.AreEqual(supervisor, subordinate.Supervisor, "Subordinate's Supervisor should be correctly assigned.");
            Assert.That(supervisor.Subordinates, Does.Contain(subordinate), "Supervisor's Subordinates should include the subordinate.");
        }

        [Test]
        public void TestRemoveSupervisor()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff supervisor = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff subordinate = new Waiter(2, "Elizabeth Bennett", "020-7946-0958", restaurant);
            subordinate.AssignSupervisor(supervisor);

            // Act
            subordinate.RemoveSupervisor();

            // Assert
            Assert.IsNull(subordinate.Supervisor, "Subordinate's Supervisor should be null after removal.");
            Assert.That(supervisor.Subordinates, Does.Not.Contain(subordinate), "Supervisor's Subordinates should not contain the subordinate after removal.");
        }

        [Test]
        public void TestChangeSupervisor()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff supervisor1 = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff supervisor2 = new Manager(2, "Jane Austen", "020-7946-0958", restaurant);
            Staff subordinate = new Waiter(3, "Elizabeth Bennett", "020-7946-0958", restaurant);
            subordinate.AssignSupervisor(supervisor1);

            // Act
            subordinate.AssignSupervisor(supervisor2);

            // Assert
            Assert.AreEqual(supervisor2, subordinate.Supervisor, "Subordinate's Supervisor should be updated to supervisor2.");
            Assert.That(supervisor2.Subordinates, Does.Contain(subordinate), "Supervisor2's Subordinates should include the subordinate.");
            Assert.That(supervisor1.Subordinates, Does.Not.Contain(subordinate), "Supervisor1's Subordinates should not contain the subordinate after reassignment.");
        }

        [Test]
        public void TestAssignSelfAsSupervisor_ShouldThrowException()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff staff = new Waiter(1, "Charles Brown", "020-7946-0958", restaurant);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => staff.AssignSupervisor(staff));
            Assert.AreEqual("Staff member cannot supervise themselves.", ex.Message, "Exception message should indicate that a Staff member cannot supervise themselves.");
        }

        [Test]
        public void TestAssignNullSupervisor_ShouldThrowException()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff subordinate = new Waiter(2, "Elizabeth Bennett", "020-7946-0958", restaurant);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => subordinate.AssignSupervisor(null));
            Assert.That(ex.Message, Does.Contain("Supervisor cannot be null."), "Exception message should indicate that Supervisor cannot be null.");
        }

        [Test]
        public void TestNoInfiniteRecursion_InMutualSupervisoryAssignment_ShouldThrowException()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff staff1 = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff staff2 = new Waiter(2, "Elizabeth Bennett", "020-7946-0958", restaurant);

            // Act
            staff1.AssignSupervisor(staff2);

            // Attempt to create a circular supervisory relationship
            var ex = Assert.Throws<InvalidOperationException>(() => staff2.AssignSupervisor(staff1));

            // Assert
            Assert.AreEqual("Assigning this supervisor creates a circular supervisory relationship.", ex.Message, "Exception message should indicate that assigning this supervisor creates a circular supervisory relationship.");
        }

        // Removed the following tests as they are no longer applicable:
        // - TestAssignSupervisorWithExistingSubordinate_ShouldThrowException
        // - TestAssignSupervisorAlreadyHasSupervisor_ShouldThrowException
    }
}
