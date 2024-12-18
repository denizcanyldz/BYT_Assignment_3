using BYT_Assignment_3.Models;

namespace Tests.AssociationTests.StaffRestaurant;

 public class StaffRestaurantAssociationTests
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

        [TearDown]
        public void TearDown()
        {
            // Reset static fields after each test
            Restaurant.SetAll(new List<Restaurant>());
            Restaurant.TotalRestaurants = 0;
            Staff.SetAll(new List<Staff>());
            Staff.TotalStaff = 0;
        }

        [Test]
        public void TestAddStaff()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);
            Staff staff = new Waiter(staffID: 1, name: "Henry Higgins", contactNumber: "020-7946-0958");

            // Act
            restaurant.AddStaff(staff);

            // Assert
            Assert.IsTrue(restaurant.StaffMembers.Contains(staff), "Restaurant should contain the added Staff member.");
            Assert.AreEqual(restaurant, staff.Restaurant, "Staff member's Restaurant reference should point to the correct Restaurant.");
        }

        [Test]
        public void TestRemoveStaff()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);
            Staff staff = new Waiter(staffID: 1, name: "Henry Higgins", contactNumber: "020-7946-0958");
            restaurant.AddStaff(staff);

            // Act
            restaurant.RemoveStaff(staff);

            // Assert
            Assert.IsFalse(restaurant.StaffMembers.Contains(staff), "Restaurant should not contain the removed Staff member.");
            Assert.IsNull(staff.Restaurant, "Staff member's Restaurant reference should be null after removal.");
        }

        [Test]
        public void TestUpdateStaff()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);
            Staff oldStaff = new Waiter(staffID: 1, name: "Henry Higgins", contactNumber: "020-7946-0958");
            Staff newStaff = new Manager(staffID: 2, name: "Emma Woodhouse", contactNumber: "020-7946-0958");
            restaurant.AddStaff(oldStaff);

            // Act
            restaurant.UpdateStaff(oldStaff, newStaff);

            // Assert
            Assert.IsFalse(restaurant.StaffMembers.Contains(oldStaff), "Restaurant should not contain the old Staff member after update.");
            Assert.IsTrue(restaurant.StaffMembers.Contains(newStaff), "Restaurant should contain the new Staff member after update.");
            Assert.IsNull(oldStaff.Restaurant, "Old Staff member's Restaurant reference should be null after update.");
            Assert.AreEqual(restaurant, newStaff.Restaurant, "New Staff member's Restaurant reference should point to the correct Restaurant.");
        }

        [Test]
        public void TestAddNullStaff_ShouldThrowException()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => restaurant.AddStaff(null));
            Assert.AreEqual("Staff cannot be null.", ex.Message, "Exception message should indicate that Staff cannot be null.");
        }

        [Test]
        public void TestRemoveNullStaff_ShouldThrowException()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => restaurant.RemoveStaff(null));
            Assert.AreEqual("Staff cannot be null.", ex.Message, "Exception message should indicate that Staff cannot be null.");
        }

        [Test]
        public void TestRemoveNonExistingStaff_ShouldNotAffectRestaurantOrStaff()
        {
            // Arrange
            var menuItems = new List<MenuItem> { new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15) };
            var menus = new List<Menu> { new Menu(1, menuItems) };
            Restaurant restaurant = new Restaurant(1, "The Crown", "221B Baker Street", "020-7946-0958", menus, null);
            Staff staff = new Waiter(staffID: 1, name: "Henry Higgins", contactNumber: "020-7946-0958");

            // Act
            restaurant.RemoveStaff(staff);

            // Assert
            Assert.IsFalse(restaurant.StaffMembers.Contains(staff), "Restaurant should not contain the Staff member.");
            Assert.IsNull(staff.Restaurant, "Staff member's Restaurant reference should be null.");
        }
    }