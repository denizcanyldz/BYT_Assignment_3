using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class AggregationMenuMenuItemTests
    {
        private Menu menu;
        private MenuItem item1, item2;

        [SetUp]
        public void SetUp()
        {
            menu = new Menu(1, [item1, item2]);
            item1 = new MenuItem(101, "Burger", 11.0, 500, 2.0, 23, true);
            item2 = new MenuItem(102, "Pizza", 12.0, 700, 3, 34, true);
        }

        [Test]
        public void AddMenuItem_AssociationIsLinkedBothWays()
        {
            menu.AddMenuItem(item1);

            Assert.That(menu.MenuItems, Contains.Item(item1), "Menu should contain the MenuItem.");
            Assert.That(item1.Menu, Is.EqualTo(menu), "MenuItem should reference the correct Menu.");
        }

        [Test]
        public void RemoveMenuItem_AssociationIsUnlinkedBothWays()
        {
            menu.AddMenuItem(item1);
            menu.RemoveMenuItem(item1);

            Assert.That(menu.MenuItems, Does.Not.Contain(item1), "Menu should not contain the MenuItem.");
            Assert.IsNull(item1.Menu, "MenuItem's Menu reference should be null after removal.");
        }

        [Test]
        public void UpdateMenuItem_AssociationIsUpdatedProperly()
        {
            menu.AddMenuItem(item1);
            menu.UpdateMenuItem(item1, item2);

            Assert.That(menu.MenuItems, Contains.Item(item2), "Menu should contain the new MenuItem.");
            Assert.That(menu.MenuItems, Does.Not.Contain(item1), "Menu should not contain the old MenuItem.");
            Assert.That(item2.Menu, Is.EqualTo(menu), "New MenuItem should reference the Menu.");
            Assert.IsNull(item1.Menu, "Old MenuItem's Menu reference should be null.");
        }

        [Test]
        public void AddMenuItem_NullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => menu.AddMenuItem(null), "Adding a null MenuItem should throw an exception.");
        }

        [Test]
        public void RemoveNonexistentMenuItem_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => menu.RemoveMenuItem(item1), "Removing a non-existent MenuItem should throw an exception.");
        }

        [Test]
        public void DeletingMenu_ShouldNotAffectMenuItems()
        {

            menu.AddMenuItem(item1);
            menu.AddMenuItem(item2);

            // Act: Delete the Menu by removing its reference
            Menu.SetAll(new List<Menu>()); // Clearing all menus

            // Assert: The MenuItems should still exist independently
            Assert.That(MenuItem.GetAll(), Contains.Item(item1),
                "MenuItem1 should still exist after Menu is deleted.");
            Assert.That(MenuItem.GetAll(), Contains.Item(item2),
                "MenuItem2 should still exist after Menu is deleted.");
        }

    }
}
