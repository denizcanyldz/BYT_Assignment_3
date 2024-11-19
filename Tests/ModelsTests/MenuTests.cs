using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
namespace Tests.ModelsTests
{
    [TestFixture]
    public class MenuTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the Menu class extent before each test to ensure test isolation
            Menu.SetAll(new List<Menu>());
        }

        [Test]
        public void Constructor_WithValidData_ShouldCreateMenu()
        {
            // Arrange
            int menuId = 1;
            var menuItems = new List<MenuItem>
            {
                new MenuItem(1, "Burger", 10.0, 500, 9.0, 15),
                new MenuItem(2, "Pizza", 12.0, 700, 10.0, 20)
            };

            // Act
            var menu = new Menu(menuId, menuItems);

            // Assert
            Assert.AreEqual(menuId, menu.MenuId, "MenuId should be set correctly.");
            Assert.AreEqual(menuItems, menu.MenuItems, "MenuItems should be assigned correctly.");
            Assert.AreEqual(1, Menu.TotalMenus, "TotalMenus should increment to 1 after creating one menu.");
            CollectionAssert.Contains(Menu.GetAll(), menu, "The created menu should be in the menus extent.");
        }

        [Test]
        public void Constructor_ShouldIncrementTotalMenus()
        {
            // Arrange
            var menuItems1 = new List<MenuItem>
            {
                new MenuItem(1, "Burger", 10.0, 500, 9.0, 15)
            };
            var menuItems2 = new List<MenuItem>
            {
                new MenuItem(2, "Salad", 8.0, 300, 7.0, 10)
            };

            // Act
            var menu1 = new Menu(1, menuItems1);
            var menu2 = new Menu(2, menuItems2);

            // Assert
            Assert.AreEqual(2, Menu.TotalMenus, "TotalMenus should increment correctly with each new menu.");
            CollectionAssert.Contains(Menu.GetAll(), menu1, "First menu should be in the menus extent.");
            CollectionAssert.Contains(Menu.GetAll(), menu2, "Second menu should be in the menus extent.");
        }

        [Test]
        public void SetAll_WithLoadedMenus_ShouldSetExtentCorrectly()
        {
            // Arrange
            var loadedMenus = new List<Menu>
            {
                new Menu(1, new List<MenuItem> { new MenuItem(1, "Burger", 10.0, 500, 9.0, 15) }),
                new Menu(2, new List<MenuItem> { new MenuItem(2, "Salad", 8.0, 300, 7.0, 10) })
            };

            // Act
            Menu.SetAll(loadedMenus);

            // Assert
            Assert.AreEqual(2, Menu.TotalMenus, "TotalMenus should reflect the loaded menus.");
            CollectionAssert.AreEqual(loadedMenus, Menu.GetAll(), "Menus extent should match the loaded menus.");
        }

        [Test]
        public void Constructor_WithDuplicateMenuId_ShouldAllowDuplicates()
        {
            // Arrange
            var menuItems1 = new List<MenuItem>
            {
                new MenuItem(1, "Burger", 10.0, 500, 9.0, 15)
            };
            var menuItems2 = new List<MenuItem>
            {
                new MenuItem(2, "Pizza", 12.0, 700, 10.0, 20)
            };

            // Act
            var menu1 = new Menu(1, menuItems1);
            var menu2 = new Menu(1, menuItems2); // Duplicate MenuId

            // Assert
            Assert.AreEqual(2, Menu.TotalMenus, "TotalMenus should increment correctly even with duplicate MenuId.");
            CollectionAssert.Contains(Menu.GetAll(), menu1, "First menu should be in the menus extent.");
            CollectionAssert.Contains(Menu.GetAll(), menu2, "Second menu should be in the menus extent.");
        }

        [Test]
        public void TotalMenus_ShouldNotAllowNegativeValue()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Menu.TotalMenus = -1);
            Assert.AreEqual("TotalMenus cannot be negative.", ex.Message,
                "Exception message should indicate that TotalMenus cannot be negative.");
        }
    }
}