using BYT_Assignment_3.Models;

namespace Tests.AssociationTests.MenuItemMenu;

[TestFixture]
    public class MenuMenuItemAssociationTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the Menu and MenuItem class extents before each test to ensure test isolation
            Menu.SetAll(new List<Menu>());
            Menu.TotalMenus = 0;
            MenuItem.SetAll(new List<MenuItem>());
            MenuItem.TotalMenuItems = 0;
        }

        [Test]
        public void TestAddMenuItem()
        {
            // Arrange
            Menu menu = new Menu(1, new List<MenuItem>());
            MenuItem menuItem = new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15);

            // Act
            menu.AddMenuItem(menuItem);

            // Assert
            Assert.IsTrue(menu.MenuItems.Contains(menuItem), "Menu should contain the added MenuItem.");
            Assert.AreEqual(menu, menuItem.Menu, "MenuItem's Menu reference should point to the correct Menu.");
        }

        [Test]
        public void TestRemoveMenuItem()
        {
            // Arrange
            Menu menu = new Menu(1, new List<MenuItem>());
            MenuItem menuItem = new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15);
            menu.AddMenuItem(menuItem);

            // Act
            menu.RemoveMenuItem(menuItem);

            // Assert
            Assert.IsFalse(menu.MenuItems.Contains(menuItem), "Menu should not contain the removed MenuItem.");
            Assert.IsNull(menuItem.Menu, "MenuItem's Menu reference should be null after removal.");
        }

        [Test]
        public void TestUpdateMenuItem()
        {
            // Arrange
            Menu menu = new Menu(1, new List<MenuItem>());
            MenuItem oldItem = new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15);
            MenuItem newItem = new MenuItem(2, "Shepherd's Pie", 12.0, 700, 10.0, 20);
            menu.AddMenuItem(oldItem);

            // Act
            menu.UpdateMenuItem(oldItem, newItem);

            // Assert
            Assert.IsFalse(menu.MenuItems.Contains(oldItem), "Menu should not contain the old MenuItem after update.");
            Assert.IsTrue(menu.MenuItems.Contains(newItem), "Menu should contain the new MenuItem after update.");
            Assert.IsNull(oldItem.Menu, "Old MenuItem's Menu reference should be null after update.");
            Assert.AreEqual(menu, newItem.Menu, "New MenuItem's Menu reference should point to the correct Menu.");
        }

        [Test]
        public void TestAddNullMenuItem_ShouldThrowException()
        {
            // Arrange
            Menu menu = new Menu(1, new List<MenuItem>());

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menu.AddMenuItem(null));
            Assert.AreEqual("MenuItem cannot be null.", ex.Message, "Exception message should indicate that MenuItem cannot be null.");
        }

        [Test]
        public void TestRemoveNullMenuItem_ShouldThrowException()
        {
            // Arrange
            Menu menu = new Menu(1, new List<MenuItem>());

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menu.RemoveMenuItem(null));
            Assert.AreEqual("MenuItem cannot be null.", ex.Message, "Exception message should indicate that MenuItem cannot be null.");
        }

        [Test]
        public void TestRemoveNonExistingMenuItem_ShouldNotAffectMenuOrMenuItem()
        {
            // Arrange
            Menu menu = new Menu(1, new List<MenuItem>());
            MenuItem menuItem = new MenuItem(1, "Fish and Chips", 10.0, 500, 9.0, 15);

            // Act
            menu.RemoveMenuItem(menuItem);

            // Assert
            Assert.IsFalse(menu.MenuItems.Contains(menuItem), "Menu should not contain the MenuItem.");
            Assert.IsNull(menuItem.Menu, "MenuItem's Menu reference should be null.");
        }
    }