using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class MenuItemIngredientTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test to ensure test isolation
            Ingredient.SetAll(new List<Ingredient>());
            MenuItem.SetAll(new List<MenuItem>());
            Inventory.SetAll(new List<Inventory>());
            Restaurant.SetAll(new List<Restaurant>());
        }

        [Test]
        public void AddIngredient_ToMenuItem_SetsBothSidesCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "kg", true);
            var menuItem = new MenuItem(1, "Tomato Soup", 5.99, 150, 4.99, 10);
            inventory.AddIngredient(ingredient); // Assign ingredient to inventory

            // Act
            menuItem.AddIngredient(ingredient);

            // Assert
            Assert.Contains(ingredient, (System.Collections.ICollection)menuItem.Ingredients, "Ingredient should be added to MenuItem.");
            Assert.Contains(menuItem, (System.Collections.ICollection)ingredient.MenuItems, "MenuItem should be added to Ingredient's MenuItems.");
        }

        [Test]
        public void AddIngredient_ToMultipleMenuItems_AssociatesCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient = new Ingredient(1, "Cheese", 20.0, "kg", false);
            var menuItem1 = new MenuItem(1, "Cheese Pizza", 8.99, 300, 7.99, 15);
            var menuItem2 = new MenuItem(2, "Cheese Sandwich", 4.99, 200, 3.99, 5);
            inventory.AddIngredient(ingredient); // Assign ingredient to inventory
    
            // Act
            menuItem1.AddIngredient(ingredient);
            menuItem2.AddIngredient(ingredient);
    
            // Assert
            Assert.Contains(ingredient, (System.Collections.ICollection)menuItem1.Ingredients, "Ingredient should be added to MenuItem1.");
            Assert.Contains(ingredient, (System.Collections.ICollection)menuItem2.Ingredients, "Ingredient should be added to MenuItem2.");
            Assert.Contains(menuItem1, (System.Collections.ICollection)ingredient.MenuItems, "MenuItem1 should be associated with Ingredient.");
            Assert.Contains(menuItem2, (System.Collections.ICollection)ingredient.MenuItems, "MenuItem2 should be associated with Ingredient.");
        }

        [Test]
        public void AddIngredient_ToMenuItem_IngredientNotInInventory_ShouldThrowException()
        {
            // Arrange
            var ingredient = new Ingredient(1, "Basil", 10.0, "kg", true);
            var menuItem = new MenuItem(1, "Pesto Pasta", 7.99, 250, 6.99, 12);
    
            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => menuItem.AddIngredient(ingredient), "Should throw InvalidOperationException when adding Ingredient not assigned to any Inventory.");
            Assert.That(ex.Message, Is.EqualTo("Ingredient must be assigned to an Inventory before being added to a MenuItem."), "Exception message should be as expected.");
        }

        [Test]
        public void AddIngredient_ToMenuItem_WithInventory_AssociatesCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "kg", true);
            var menuItem = new MenuItem(1, "Tomato Soup", 5.99, 150, 4.99, 10);
            inventory.AddIngredient(ingredient); // Assign ingredient to inventory
    
            // Act
            menuItem.AddIngredient(ingredient);
    
            // Assert
            Assert.Contains(ingredient, (System.Collections.ICollection)menuItem.Ingredients, "Ingredient should be added to MenuItem.");
            Assert.Contains(menuItem, (System.Collections.ICollection)ingredient.MenuItems, "MenuItem should be associated with Ingredient.");
        }


        [Test]
        public void RemoveIngredient_FromMenuItem_UnsetsAssociation()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient = new Ingredient(1, "Lettuce", 30.0, "kg", true);
            var menuItem = new MenuItem(1, "Caesar Salad", 6.99, 180, 5.99, 7);
            inventory.AddIngredient(ingredient);
            menuItem.AddIngredient(ingredient);

            // Act
            menuItem.RemoveIngredient(ingredient);

            // Assert
            Assert.IsFalse(menuItem.Ingredients.Contains(ingredient), "Ingredient should be removed from MenuItem.");
            Assert.IsFalse(ingredient.MenuItems.Contains(menuItem), "MenuItem should be removed from Ingredient's MenuItems.");
        }

        [Test]
        public void AddIngredient_ToMenuItem_Null_ShouldThrowArgumentNullException()
        {
            // Arrange
            var menuItem = new MenuItem(1, "Veggie Burger", 9.99, 350, 8.99, 15);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.AddIngredient(null), "Should throw ArgumentNullException when adding null Ingredient.");
            Assert.That(ex.ParamName, Is.EqualTo("ingredient"), "Exception should reference the correct parameter.");
        }

        [Test]
        public void RemoveIngredient_NotInMenuItem_ShouldThrowArgumentException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient = new Ingredient(1, "Mushroom", 25.0, "kg", false);
            var menuItem = new MenuItem(1, "Mushroom Risotto", 12.99, 400, 10.99, 20);
            inventory.AddIngredient(ingredient);
            // Note: Ingredient not added to MenuItem

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.RemoveIngredient(ingredient), "Should throw ArgumentException when removing Ingredient not in MenuItem.");
            Assert.That(ex.Message, Is.EqualTo("Ingredient not found in the menu item."), "Exception message should be as expected.");
        }

        [Test]
        public void UpdateIngredient_InMenuItem_ReplacesIngredientCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var oldIngredient = new Ingredient(1, "Pepperoni", 15.0, "kg", false);
            var newIngredient = new Ingredient(2, "Sausage", 20.0, "kg", false);
            var menuItem = new MenuItem(1, "Pepperoni Pizza", 10.99, 500, 9.99, 20);
            inventory.AddIngredient(oldIngredient);
            inventory.AddIngredient(newIngredient);
            menuItem.AddIngredient(oldIngredient);

            // Act
            menuItem.UpdateIngredient(oldIngredient, newIngredient);

            // Assert
            Assert.IsFalse(menuItem.Ingredients.Contains(oldIngredient), "Old Ingredient should be removed from MenuItem.");
            Assert.Contains(newIngredient, (System.Collections.ICollection)menuItem.Ingredients, "New Ingredient should be added to MenuItem.");
            Assert.IsFalse(oldIngredient.MenuItems.Contains(menuItem), "MenuItem should be removed from old Ingredient's MenuItems.");
            Assert.Contains(menuItem, (System.Collections.ICollection)newIngredient.MenuItems, "MenuItem should be added to new Ingredient's MenuItems.");
        }

        [Test]
        public void DeleteMenuItem_RemovesAllAssociations()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient1 = new Ingredient(1, "Onion", 20.0, "kg", true);
            var ingredient2 = new Ingredient(2, "Garlic", 10.0, "kg", true);
            var menuItem = new MenuItem(1, "Bruschetta", 5.99, 100, 4.99, 5);
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);
            menuItem.AddIngredient(ingredient1);
            menuItem.AddIngredient(ingredient2);

            // Act
            menuItem.RemoveFromExtent(); // Simulate deleting the MenuItem

            // Assert
            Assert.IsFalse(MenuItem.GetAll().Contains(menuItem), "MenuItem should be removed from class extent.");
            Assert.IsFalse(ingredient1.MenuItems.Contains(menuItem), "MenuItem should be removed from Ingredient1's MenuItems.");
            Assert.IsFalse(ingredient2.MenuItems.Contains(menuItem), "MenuItem should be removed from Ingredient2's MenuItems.");
        }

        [Test]
        public void DeleteIngredient_RemovesAllAssociations()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-10));
            var ingredient = new Ingredient(1, "Spinach", 25.0, "kg", true);
            var menuItem1 = new MenuItem(1, "Spinach Salad", 6.99, 150, 5.99, 10);
            var menuItem2 = new MenuItem(2, "Spinach Pasta", 8.99, 300, 7.99, 15);
            inventory.AddIngredient(ingredient);
            menuItem1.AddIngredient(ingredient);
            menuItem2.AddIngredient(ingredient);

            // Act
            ingredient.RemoveFromExtent(); // Simulate deleting the Ingredient

            // Assert
            Assert.IsFalse(Ingredient.GetAll().Contains(ingredient), "Ingredient should be removed from class extent.");
            Assert.IsFalse(menuItem1.Ingredients.Contains(ingredient), "Ingredient should be removed from MenuItem1's Ingredients.");
            Assert.IsFalse(menuItem2.Ingredients.Contains(ingredient), "Ingredient should be removed from MenuItem2's Ingredients.");
        }
    }
}
