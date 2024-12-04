using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
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

        [Test]
        public void AddIngredient_ShouldThrowException_WhenIngredientIsNull()
        {
            // Arrange
            var menuItem = new MenuItem(3, "Salad", 8.0, 300, 7.0, 10);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.AddIngredient(null));
            Assert.AreEqual("Ingredient cannot be null. (Parameter 'ingredient')", ex.Message);
        }

        [Test]
        public void AddIngredient_ShouldThrowException_WhenIngredientAlreadyExists()
        {
            // Arrange
            var menuItem = new MenuItem(4, "Burger", 10.0, 500, 9.0, 12);
            var ingredient = new Ingredient(3, "Lettuce", 20.0, "Leaves", false);

            menuItem.AddIngredient(ingredient);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.AddIngredient(ingredient));
            Assert.AreEqual("Ingredient already exists in the menu item.", ex.Message);
        }

        [Test]
        public void RemoveIngredient_ShouldThrowException_WhenIngredientIsNull()
        {
            // Arrange
            var menuItem = new MenuItem(5, "Soup", 7.0, 250, 6.0, 8);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => menuItem.RemoveIngredient(null));
            Assert.AreEqual("Ingredient cannot be null. (Parameter 'ingredient')", ex.Message);
        }

        [Test]
        public void RemoveIngredient_ShouldThrowException_WhenIngredientNotFound()
        {
            // Arrange
            var menuItem = new MenuItem(6, "Steak", 20.0, 700, 18.0, 25);
            var ingredient = new Ingredient(4, "Salt", 1.0, "Kg", false);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => menuItem.RemoveIngredient(ingredient));
            Assert.AreEqual("Ingredient not found in the menu item.", ex.Message);
        }

        [Test]
        public void Ingredient_ShouldMaintainListOfMenuItems()
        {
            // Arrange
            var ingredient = new Ingredient(5, "Chicken", 50.0, "Kg", true);
            var menuItem1 = new MenuItem(7, "Chicken Sandwich", 9.0, 450, 8.0, 12);
            var menuItem2 = new MenuItem(8, "Chicken Salad", 11.0, 400, 10.0, 15);

            // Act
            menuItem1.AddIngredient(ingredient);
            menuItem2.AddIngredient(ingredient);

            // Assert
            Assert.Contains(menuItem1, (System.Collections.ICollection)ingredient.MenuItems, "Ingredient should include MenuItem1.");
            Assert.Contains(menuItem2, (System.Collections.ICollection)ingredient.MenuItems, "Ingredient should include MenuItem2.");
            Assert.AreEqual(2, ingredient.MenuItems.Count, "Ingredient should have two associated MenuItems.");
        }

        [Test]
        public void EditingIngredient_ShouldReflectInMenuItem()
        {
            // Arrange
            var menuItem = new MenuItem(9, "Fish and Chips", 13.0, 600, 11.0, 18);
            var ingredient = new Ingredient(6, "Cod Fish", 30.0, "Kg", true);

            menuItem.AddIngredient(ingredient);

            // Act
            ingredient.Quantity = 25.0; // Editing the ingredient

            // Assert
            Assert.AreEqual(25.0, menuItem.Ingredients.First().Quantity, "Ingredient's updated quantity should be reflected in MenuItem.");
        }
    }
}
