using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class OneToManyChefMenuItemTests
    {
        private Chef chef;
        private MenuItem menuItem1;
        private MenuItem menuItem2;

        [SetUp]
        public void SetUp()
        {
            // Reset the test environment
            chef = new Chef(1, "Gordon Ramsay");
            menuItem1 = new MenuItem(101, "Burger", 10.0, 500, 8.0, 15);
            menuItem2 = new MenuItem(102, "Pizza", 12.0, 700, 10.0, 20);
        }

        [Test]
        public void AddMenuItem_AssociationIsLinkedBothWays()
        {
            // Add menuItem1 to chef
            chef.AddMenuItem(menuItem1);

            Assert.That(chef.MenuItems, Contains.Item(menuItem1),
                "Chef should contain the added MenuItem.");
            Assert.That(menuItem1.Chef, Is.EqualTo(chef),
                "MenuItem should reference the correct Chef.");
        }

        [Test]
        public void RemoveMenuItem_AssociationIsUnlinkedBothWays()
        {
            // Add and then remove menuItem1
            chef.AddMenuItem(menuItem1);
            chef.RemoveMenuItem(menuItem1);

            Assert.That(chef.MenuItems, Does.Not.Contain(menuItem1),
                "MenuItem should no longer be in Chef's list.");
            Assert.That(menuItem1.Chef, Is.Null,
                "MenuItem's Chef reference should be null after removal.");
        }

        [Test]
        public void ModifyMenuItem_AssociationUpdatesProperly()
        {
            var chef2 = new Chef(2, "Jamie Oliver");

            // Assign menuItem1 to chef, then update to chef2
            chef.AddMenuItem(menuItem1);
            chef.ModifyMenuItem(menuItem1, chef2);

            Assert.That(chef.MenuItems, Does.Not.Contain(menuItem1),
                "Original Chef should no longer have the MenuItem.");
            Assert.That(chef2.MenuItems, Contains.Item(menuItem1),
                "New Chef should now contain the MenuItem.");
            Assert.That(menuItem1.Chef, Is.EqualTo(chef2),
                "MenuItem should reference the new Chef.");
        }

        [Test]
        public void AddMenuItem_NullShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => chef.AddMenuItem(null),
                "Adding a null MenuItem should throw an exception.");
        }

        [Test]
        public void RemoveNonexistentMenuItem_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => chef.RemoveMenuItem(menuItem1),
                "Removing a non-existent MenuItem should throw an exception.");
        }

        [Test]
        public void MenuItemAssociation_CountIsUpdatedCorrectly()
        {
            // Add two MenuItems
            chef.AddMenuItem(menuItem1);
            chef.AddMenuItem(menuItem2);

            Assert.That(chef.MenuItems.Count, Is.EqualTo(2),
                "Chef's MenuItems count should reflect the correct number of items.");
        }
    }
}
