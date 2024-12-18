using BYT_Assignment_3.Models;

namespace Tests.AssociationTests.StaffReflexive;

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
            Assert.AreEqual(subordinate, supervisor.Subordinate, "Supervisor's Subordinate should be correctly assigned.");
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
            Assert.IsNull(supervisor.Subordinate, "Supervisor's Subordinate should be null after removal.");
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
            Assert.AreEqual(subordinate, supervisor2.Subordinate, "Supervisor2's Subordinate should be the subordinate.");
            Assert.IsNull(supervisor1.Subordinate, "Supervisor1's Subordinate should be null after subordinate is reassigned.");
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
            var ex = Assert.Throws<ArgumentException>(() => subordinate.AssignSupervisor(null));
            Assert.AreEqual("Supervisor cannot be null.", ex.Message, "Exception message should indicate that Supervisor cannot be null.");
        }

        [Test]
        public void TestAssignSupervisorWithExistingSubordinate_ShouldThrowException()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff supervisor = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff subordinate1 = new Waiter(2, "Elizabeth Bennett", "020-7946-0958", restaurant);
            Staff subordinate2 = new Waiter(3, "Emma Woodhouse", "020-7946-0958", restaurant);
            subordinate1.AssignSupervisor(supervisor);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => subordinate2.AssignSupervisor(supervisor));
            Assert.AreEqual("This Staff member already has a subordinate.", ex.Message, "Exception message should indicate that the Supervisor already has a different subordinate.");
        }

        [Test]
        public void TestAssignSupervisorAlreadyHasSupervisor_ShouldThrowException()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff supervisor1 = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff supervisor2 = new Manager(2, "Jane Austen", "020-7946-0958", restaurant);
            Staff subordinate = new Waiter(3, "Elizabeth Bennett", "020-7946-0958", restaurant);
            subordinate.AssignSupervisor(supervisor1);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => subordinate.AssignSupervisor(supervisor2));
            Assert.AreEqual("The new supervisor already has a different supervisor.", ex.Message, "Exception message should indicate that the new Supervisor already has a different supervisor.");
        }

        [Test]
        public void TestNoInfiniteRecursion_InMutualSupervisoryAssignment()
        {
            // Arrange
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", new List<Menu>(), null);
            Staff staff1 = new Manager(1, "Charles Dickens", "020-7946-0958", restaurant);
            Staff staff2 = new Waiter(2, "Elizabeth Bennett", "020-7946-0958", restaurant);

            // Act
            staff1.AssignSupervisor(staff2);
            staff2.AssignSupervisor(staff1);

            // Assert
            Assert.AreEqual(staff2, staff1.Supervisor, "Staff1's Supervisor should be staff2.");
            Assert.AreEqual(staff1, staff2.Supervisor, "Staff2's Supervisor should be staff1.");
            Assert.AreEqual(staff1, staff2.Subordinate, "Staff2's Subordinate should be staff1.");
            Assert.AreEqual(staff2, staff1.Subordinate, "Staff1's Subordinate should be staff2.");
        }
    }