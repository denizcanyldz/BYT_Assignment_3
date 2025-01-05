using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class InventoryIngredientTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test to ensure test isolation
            Ingredient.SetAll(new List<Ingredient>());
            Inventory.SetAll(new List<Inventory>());
        }

        [Test]
        public void AddIngredient_ToInventory_SetsBothSidesCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "kg", true);

            // Act
            inventory.AddIngredient(ingredient);

            // Assert
            Assert.Contains(ingredient, (System.Collections.ICollection)inventory.Ingredients, "Ingredient should be added to Inventory.");
            Assert.AreEqual(inventory, ingredient.ParentInventory, "Ingredient's ParentInventory should be set correctly.");
        }

        [Test]
        public void AddIngredient_ToInventory_AlreadyAssigned_ShouldThrowException()
        {
            // Arrange
            var inventory1 = new Inventory(1, DateTime.Now.AddDays(-1));
            var inventory2 = new Inventory(2, DateTime.Now.AddDays(-2));
            var ingredient = new Ingredient(1, "Cheese", 20.0, "kg", false);

            // Act
            inventory1.AddIngredient(ingredient);

            // Assert
            var ex = Assert.Throws<InvalidOperationException>(() => inventory2.AddIngredient(ingredient));
            Assert.That(ex.Message, Is.EqualTo("This Ingredient is already assigned to an Inventory."), "Should not allow an Ingredient to be added to multiple Inventories.");
        }

        [Test]
        public void RemoveIngredient_FromInventory_UnsetsAssociation()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Lettuce", 30.0, "kg", true);
            inventory.AddIngredient(ingredient);

            // Act
            inventory.RemoveIngredient(ingredient);

            // Assert
            Assert.IsFalse(inventory.Ingredients.Contains(ingredient), "Ingredient should be removed from Inventory.");
            Assert.IsNull(ingredient.ParentInventory, "Ingredient's ParentInventory should be null after removal.");
        }

        [Test]
        public void UpdateIngredient_InInventory_ReplacesIngredientCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var oldIngredient = new Ingredient(1, "Onion", 40.0, "kg", true);
            var newIngredient = new Ingredient(2, "Garlic", 10.0, "kg", true);
            inventory.AddIngredient(oldIngredient);

            // Act
            inventory.UpdateIngredient(oldIngredient, newIngredient);

            // Assert
            Assert.IsFalse(inventory.Ingredients.Contains(oldIngredient), "Old Ingredient should be removed from Inventory.");
            Assert.Contains(newIngredient, (System.Collections.ICollection)inventory.Ingredients, "New Ingredient should be added to Inventory.");
            Assert.IsNull(oldIngredient.ParentInventory, "Old Ingredient's ParentInventory should be null after update.");
            Assert.AreEqual(inventory, newIngredient.ParentInventory, "New Ingredient's ParentInventory should be set correctly.");
        }

        [Test]
        public void AddIngredient_Null_ShouldThrowArgumentNullException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => inventory.AddIngredient(null));
            Assert.That(ex.ParamName, Is.EqualTo("ingredient"), "Should throw ArgumentNullException when adding null Ingredient.");
        }

        [Test]
        public void RemoveIngredient_NotInInventory_ShouldThrowArgumentException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Basil", 5.0, "kg", true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.RemoveIngredient(ingredient));
            Assert.That(ex.Message, Is.EqualTo("Ingredient not found in the inventory."), "Should throw ArgumentException when removing an Ingredient not in Inventory.");
        }

        [Test]
        public void UpdateIngredient_NullOldIngredient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var newIngredient = new Ingredient(2, "Parsley", 15.0, "kg", true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => inventory.UpdateIngredient(null, newIngredient));
            Assert.That(ex.Message, Does.Contain("Ingredient cannot be null."), "Should throw ArgumentNullException when old Ingredient is null.");
        }

        [Test]
        public void UpdateIngredient_NullNewIngredient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var oldIngredient = new Ingredient(1, "Pepper", 25.0, "kg", true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => inventory.UpdateIngredient(oldIngredient, null));
            Assert.That(ex.Message, Does.Contain("Ingredient cannot be null."), "Should throw ArgumentNullException when new Ingredient is null.");
        }

        [Test]
        public void DeleteInventory_RemovesAllAssociatedIngredients()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient1 = new Ingredient(1, "Salt", 100.0, "kg", false);
            var ingredient2 = new Ingredient(2, "Sugar", 80.0, "kg", false);
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);

            // Act
            inventory.RemoveFromExtent(); // Simulate deleting the Inventory

            // Assert
            Assert.IsFalse(Inventory.GetAll().Contains(inventory), "Inventory should be removed from class extent.");
            Assert.IsEmpty(Ingredient.GetAll(), "All Ingredients associated with the deleted Inventory should be removed.");
        }
    }
}
