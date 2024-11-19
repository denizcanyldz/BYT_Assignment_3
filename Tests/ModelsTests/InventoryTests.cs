using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using BYT_Assignment_3.Persistences;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class InventoryTests
    {
        // Path to the test XML file used for serialization/deserialization
        private const string TestFilePath = "test_inventories.xml";

        /// <summary>
        /// Runs before each test to ensure a clean environment.
        /// Resets all class extents and deletes the test XML file if it exists.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Reset all class extents to ensure no residual data from previous tests
            Inventory.SetAll(new List<Inventory>());

            // Ensure the test file does not exist before each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }
        }

        /// <summary>
        /// Runs after each test to clean up the test environment.
        /// Deletes the test XML file and resets all class extents.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Clean up the test file after each test
            if (File.Exists(TestFilePath))
            {
                File.Delete(TestFilePath);
            }

            // Reset class extents after each test
            SetUp();
        }

        #region Inventory Class Tests

        /// <summary>
        /// Tests that an Inventory object is created correctly with all mandatory attributes.
        /// </summary>
        [Test]
        public void Inventory_CreatesObjectCorrectly_WithAllAttributes()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-5));

            // Act & Assert
            Assert.AreEqual(1, inventory.InventoryID, "InventoryID should be set correctly.");
            Assert.AreEqual(DateTime.Now.AddDays(-5).Date, inventory.LastRestockDate.Date, "LastRestockDate should be set correctly.");
            Assert.IsEmpty(inventory.Ingredients, "Ingredients list should be empty upon creation.");
            Assert.AreEqual(0, inventory.TotalItems, "TotalItems should be zero upon creation.");
        }

        /// <summary>
        /// Tests that creating an Inventory with an invalid InventoryID throws an ArgumentException.
        /// </summary>
        [Test]
        public void Inventory_InvalidInventoryID_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidInventoryID = 0; // Use 0 as it's invalid

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Inventory(invalidInventoryID, DateTime.Now.AddDays(-5)));
            Assert.AreEqual("InventoryID must be positive.", ex.Message, "Exception message should indicate invalid InventoryID.");
        }

        /// <summary>
        /// Tests that creating an Inventory with a future LastRestockDate throws an ArgumentException.
        /// </summary>
        [Test]
        public void Inventory_FutureLastRestockDate_ShouldThrowArgumentException()
        {
            // Arrange
            var futureDate = DateTime.Now.AddDays(1); // Tomorrow

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Inventory(2, futureDate));
            Assert.AreEqual("LastRestockDate cannot be in the future.", ex.Message, "Exception message should indicate invalid LastRestockDate.");
        }

        /// <summary>
        /// Tests that adding a valid Ingredient to the Inventory increases TotalItems.
        /// </summary>
        [Test]
        public void Inventory_AddingValidIngredient_ShouldIncreaseTotalItems()
        {
            // Arrange
            var inventory = new Inventory(3, DateTime.Now.AddDays(-2));
            var ingredient = new Ingredient(1, "Tomato", 10.0, "Kg", true);

            // Act
            inventory.AddIngredient(ingredient);

            // Assert
            Assert.AreEqual(1, inventory.TotalItems, "TotalItems should be incremented by one after adding an ingredient.");
            Assert.Contains(ingredient, (System.Collections.ICollection)inventory.Ingredients, "Ingredient should be present in the Ingredients list.");
        }

        /// <summary>
        /// Tests that adding a null Ingredient throws an ArgumentNullException.
        /// </summary>
        [Test]
        public void Inventory_AddingNullIngredient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var inventory = new Inventory(4, DateTime.Now.AddDays(-1));

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => inventory.AddIngredient(null!));
            Assert.AreEqual("Ingredient cannot be null. (Parameter 'ingredient')", ex.Message, "Exception message should indicate null Ingredient.");
        }

        /// <summary>
        /// Tests that adding a duplicate Ingredient throws an ArgumentException.
        /// </summary>
        [Test]
        public void Inventory_AddingDuplicateIngredient_ShouldThrowArgumentException()
        {
            // Arrange
            var inventory = new Inventory(5, DateTime.Now.AddDays(-3));
            var ingredient = new Ingredient(2, "Onion", 5.0, "Kg", false);
            inventory.AddIngredient(ingredient);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.AddIngredient(ingredient));
            Assert.AreEqual("Ingredient already exists in the inventory.", ex.Message, "Exception message should indicate duplicate Ingredient.");
        }

        /// <summary>
        /// Tests that removing an existing Ingredient decreases TotalItems.
        /// </summary>
        [Test]
        public void Inventory_RemovingExistingIngredient_ShouldDecreaseTotalItems()
        {
            // Arrange
            var inventory = new Inventory(6, DateTime.Now.AddDays(-4));
            var ingredient = new Ingredient(3, "Salt", 2.0, "Kg", false);
            inventory.AddIngredient(ingredient);
            Assert.AreEqual(1, inventory.TotalItems, "TotalItems should be 1 after adding an ingredient.");

            // Act
            inventory.RemoveIngredient(ingredient);

            // Assert
            Assert.AreEqual(0, inventory.TotalItems, "TotalItems should be decremented by one after removing an ingredient.");
            Assert.IsFalse(inventory.Ingredients.Contains(ingredient), "Ingredient should no longer be present in the Ingredients list.");
        }

        /// <summary>
        /// Tests that removing a null Ingredient throws an ArgumentNullException.
        /// </summary>
        [Test]
        public void Inventory_RemovingNullIngredient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var inventory = new Inventory(7, DateTime.Now.AddDays(-5));

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => inventory.RemoveIngredient(null!));
            Assert.AreEqual("Ingredient cannot be null. (Parameter 'ingredient')", ex.Message, "Exception message should indicate null Ingredient.");
        }

        /// <summary>
        /// Tests that removing a non-existing Ingredient throws an ArgumentException.
        /// </summary>
        [Test]
        public void Inventory_RemovingNonExistingIngredient_ShouldThrowArgumentException()
        {
            // Arrange
            var inventory = new Inventory(8, DateTime.Now.AddDays(-6));
            var ingredient = new Ingredient(4, "Sugar", 3.0, "Kg", true);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.RemoveIngredient(ingredient));
            Assert.AreEqual("Ingredient not found in the inventory.", ex.Message, "Exception message should indicate Ingredient not found.");
        }

        /// <summary>
        /// Tests that the TotalItems property reflects the correct number of Ingredients.
        /// </summary>
        [Test]
        public void Inventory_TotalItems_ShouldReflectCorrectCount()
        {
            // Arrange
            var inventory = new Inventory(9, DateTime.Now.AddDays(-7));
            var ingredient1 = new Ingredient(5, "Pepper", 1.0, "Kg", false);
            var ingredient2 = new Ingredient(6, "Garlic", 0.5, "Kg", true);

            // Act
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);

            // Assert
            Assert.AreEqual(2, inventory.TotalItems, "TotalItems should correctly reflect the number of Ingredients.");
        }

        /// <summary>
        /// Tests that the Equals and GetHashCode methods function correctly.
        /// </summary>
        [Test]
        public void Inventory_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var inventory1 = new Inventory(10, DateTime.Now.AddDays(-8));
            var inventory2 = new Inventory(11, DateTime.Now.AddDays(-8)); // Different InventoryID
            var inventory3 = inventory1; // Same instance as inventory1

            // Act & Assert
            Assert.IsTrue(inventory1.Equals(inventory3), "An Inventory should be equal to itself.");
            Assert.AreEqual(inventory1.GetHashCode(), inventory3.GetHashCode(), "HashCodes of the same instance should be equal.");

            Assert.IsFalse(inventory1.Equals(inventory2), "Inventories with different InventoryIDs should not be equal.");
            Assert.AreNotEqual(inventory1.GetHashCode(), inventory2.GetHashCode(), "HashCodes of different inventories should not be equal.");
        }


        /// <summary>
        /// Tests that setting the Inventory extent to null initializes it as an empty list.
        /// </summary>
        [Test]
        public void SetAll_Null_ShouldInitializeEmptyExtent()
        {
            // Act
            Inventory.SetAll(null);

            // Assert
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0 after setting null.");
            Assert.IsEmpty(Inventory.GetAll(), "Inventory extent should be empty after setting null.");
        }

        #endregion
    }
}
