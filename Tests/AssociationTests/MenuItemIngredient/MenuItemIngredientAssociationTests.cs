using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;

namespace Tests.AssociationTests
{
    [TestFixture]
    public class MenuItemIngredientAssociationTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test
            MenuItem.SetAll(new List<MenuItem>());
            Ingredient.SetAll(new List<Ingredient>());
        }
        
        #region Add and Remove Ingredient Tests

        [Test]
        public void AddIngredient_ShouldAddIngredientToMenuItemAndUpdateReverseConnection()
        {
            // Arrange
            var menuItem = new MenuItem(1, "Pasta", 12.0, 600, 10.0, 15);
            var ingredient = new Ingredient(1, "Tomato Sauce", 5.0, "L", true);

            // Act
            menuItem.AddIngredient(ingredient);

            // Assert
            Assert.Contains(ingredient, (System.Collections.ICollection)menuItem.Ingredients, "Ingredient should be added to MenuItem.");
            Assert.Contains(menuItem, (System.Collections.ICollection)ingredient.MenuItems, "MenuItem should be added to Ingredient.");
        }

        [Test]
        public void RemoveIngredient_ShouldRemoveIngredientFromMenuItemAndUpdateReverseConnection()
        {
            // Arrange
            var menuItem = new MenuItem(2, "Pizza", 15.0, 800, 12.0, 20);
            var ingredient = new Ingredient(2, "Cheese", 10.0, "Kg", true);

            menuItem.AddIngredient(ingredient);

            // Act
            menuItem.RemoveIngredient(ingredient);

            // Assert
            Assert.IsFalse(menuItem.Ingredients.Contains(ingredient), "Ingredient should be removed from MenuItem.");
            Assert.IsFalse(ingredient.MenuItems.Contains(menuItem), "MenuItem should be removed from Ingredient.");
        }

        #endregion

        #region Update Ingredient Tests

        [Test]
        public void UpdateIngredient_ShouldReplaceExistingIngredientWithNewIngredientAndMaintainReverseConnections()
        {
            // Arrange
            var menuItem = new MenuItem(3, "Salad", 8.0, 300, 7.0, 10);
            var ingredientOld = new Ingredient(3, "Lettuce", 20.0, "Leaves", false);
            var ingredientNew = new Ingredient(4, "Spinach", 25.0, "Leaves", false);

            menuItem.AddIngredient(ingredientOld);

            // Act
            menuItem.UpdateIngredient(ingredientOld, ingredientNew);

            // Assert
            // Verify old ingredient is removed
            Assert.IsFalse(menuItem.Ingredients.Contains(ingredientOld), "Old ingredient should be removed from MenuItem.");
            Assert.IsFalse(ingredientOld.MenuItems.Contains(menuItem), "MenuItem should be removed from old Ingredient.");

            // Verify new ingredient is added
            Assert.Contains(ingredientNew, (System.Collections.ICollection)menuItem.Ingredients, "New ingredient should be added to MenuItem.");
            Assert.Contains(menuItem, (System.Collections.ICollection)ingredientNew.MenuItems, "MenuItem should be added to new Ingredient.");
        }

        [Test]
        public void UpdateIngredient_ShouldThrowException_WhenExistingIngredientIsNotInMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem(4, "Soup", 7.0, 250, 6.0, 8);
            var ingredientOld = new Ingredient(5, "Carrot", 15.0, "Pieces", false);
            var ingredientNew = new Ingredient(6, "Potato", 18.0, "Pieces", false);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.UpdateIngredient(ingredientOld, ingredientNew));
            Assert.AreEqual("Existing ingredient not found in the menu item.", ex.Message);
        }

        [Test]
        public void UpdateIngredient_ShouldThrowException_WhenNewIngredientIsAlreadyInMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem(5, "Burger", 10.0, 500, 9.0, 12);
            var ingredientOld = new Ingredient(7, "Onion", 10.0, "Pieces", false);
            var ingredientNew = new Ingredient(8, "Cheese", 5.0, "Slices", true);

            menuItem.AddIngredient(ingredientOld);
            menuItem.AddIngredient(ingredientNew);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.UpdateIngredient(ingredientOld, ingredientNew));
            Assert.AreEqual("New ingredient already exists in the menu item.", ex.Message);
        }

        [Test]
        public void UpdateIngredient_ShouldThrowException_WhenExistingIngredientIsNull()
        {
            // Arrange
            var menuItem = new MenuItem(6, "Steak", 20.0, 700, 18.0, 25);
            var ingredientNew = new Ingredient(9, "Garlic", 2.0, "Cloves", true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.UpdateIngredient(null, ingredientNew));
            Assert.AreEqual("Existing ingredient cannot be null. (Parameter 'existingIngredient')", ex.Message);
        }

        [Test]
        public void UpdateIngredient_ShouldThrowException_WhenNewIngredientIsNull()
        {
            // Arrange
            var menuItem = new MenuItem(7, "Fish and Chips", 13.0, 600, 11.0, 18);
            var ingredientOld = new Ingredient(10, "Fish", 30.0, "Kg", true);

            menuItem.AddIngredient(ingredientOld);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.UpdateIngredient(ingredientOld, null));
            Assert.AreEqual("New ingredient cannot be null. (Parameter 'newIngredient')", ex.Message);
        }

        [Test]
        public void UpdateIngredient_ShouldThrowException_WhenBothIngredientsAreNull()
        {
            // Arrange
            var menuItem = new MenuItem(8, "Chicken Salad", 11.0, 400, 10.0, 15);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.UpdateIngredient(null, null));
            Assert.AreEqual("Existing ingredient cannot be null. (Parameter 'existingIngredient')", ex.Message);
        }

        #endregion

        #region Exception Handling Tests

        [Test]
        public void AddIngredient_ShouldThrowException_WhenIngredientIsNull()
        {
            // Arrange
            var menuItem = new MenuItem(9, "Tacos", 9.0, 350, 8.0, 14);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.AddIngredient(null));
            Assert.AreEqual("Ingredient cannot be null. (Parameter 'ingredient')", ex.Message);
        }

        [Test]
        public void AddIngredient_ShouldThrowException_WhenIngredientAlreadyExists()
        {
            // Arrange
            var menuItem = new MenuItem(10, "Nachos", 7.0, 300, 6.0, 10);
            var ingredient = new Ingredient(11, "Cheese", 5.0, "Kg", true);

            menuItem.AddIngredient(ingredient);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.AddIngredient(ingredient));
            Assert.AreEqual("Ingredient already exists in the menu item.", ex.Message);
        }

        [Test]
        public void RemoveIngredient_ShouldThrowException_WhenIngredientIsNull()
        {
            // Arrange
            var menuItem = new MenuItem(11, "Lasagna", 14.0, 650, 12.0, 20);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.RemoveIngredient(null));
            Assert.AreEqual("Ingredient cannot be null. (Parameter 'ingredient')", ex.Message);
        }

        [Test]
        public void RemoveIngredient_ShouldThrowException_WhenIngredientNotFound()
        {
            // Arrange
            var menuItem = new MenuItem(12, "Risotto", 13.0, 550, 11.0, 18);
            var ingredient = new Ingredient(12, "Mushrooms", 4.0, "Kg", false);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.RemoveIngredient(ingredient));
            Assert.AreEqual("Ingredient not found in the menu item.", ex.Message);
        }

        #endregion

        #region Equality and HashCode Tests

        [Test]
        public void UpdateIngredient_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var menuItem = new MenuItem(13, "Sushi", 20.0, 500, 18.0, 22);
            var ingredient1 = new Ingredient(13, "Rice", 10.0, "Kg", true);
            var ingredient2 = new Ingredient(14, "Fish", 15.0, "Kg", true);

            menuItem.AddIngredient(ingredient1);

            // Act
            menuItem.UpdateIngredient(ingredient1, ingredient2);

            // Assert
            Assert.IsTrue(menuItem.Ingredients.Contains(ingredient2), "MenuItem should contain the new ingredient.");
            Assert.IsFalse(menuItem.Ingredients.Contains(ingredient1), "MenuItem should no longer contain the old ingredient.");
            Assert.IsFalse(ingredient1.MenuItems.Contains(menuItem), "Old ingredient should no longer reference the MenuItem.");
            Assert.IsTrue(ingredient2.MenuItems.Contains(menuItem), "New ingredient should reference the MenuItem.");
        }

        #endregion
    }
}
