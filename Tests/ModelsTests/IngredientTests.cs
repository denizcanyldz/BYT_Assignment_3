using NUnit.Framework;
using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using BYT_Assignment_3.Persistences;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class IngredientTests
    {
        // Path to the test XML file used for serialization/deserialization
        private const string TestFilePath = "test_ingredients.xml";

        /// <summary>
        /// Runs before each test to ensure a clean environment.
        /// Resets all class extents and deletes the test XML file if it exists.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Reset all class extents to ensure no residual data from previous tests
            Ingredient.SetAll(new List<Ingredient>());

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

        #region Ingredient Class Tests

        /// <summary>
        /// Tests that an Ingredient object is created correctly with all mandatory and optional attributes.
        /// </summary>
        [Test]
        public void Ingredient_CreatesObjectCorrectly_WithAllAttributes()
        {
            // Arrange
            var ingredient = new Ingredient(1, "Tomato", 2.5, "Kg", true);

            // Act & Assert
            Assert.AreEqual(1, ingredient.IngredientID, "IngredientID should be set correctly.");
            Assert.AreEqual("Tomato", ingredient.Name, "Name should be set correctly.");
            Assert.AreEqual(2.5, ingredient.Quantity, "Quantity should be set correctly.");
            Assert.AreEqual("Kg", ingredient.Unit, "Unit should be set correctly.");
            Assert.IsTrue(ingredient.IsPerishable, "IsPerishable should be set correctly.");
        }

        /// <summary>
        /// Tests that creating an Ingredient with an invalid IngredientID throws an ArgumentException.
        /// </summary>
        [Test]
        public void Ingredient_InvalidIngredientID_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidIngredientID = -0; 

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Ingredient(invalidIngredientID, "Onion", 1.0, "Kg", false));
            Assert.AreEqual("IngredientID must be positive.", ex.Message, "Exception message should indicate invalid IngredientID.");
        }

        /// <summary>
        /// Tests that creating an Ingredient with a null or empty Name throws an ArgumentException.
        /// </summary>
        [Test]
        public void Ingredient_NullOrEmptyName_ShouldThrowException()
        {
            // Arrange
            var invalidNames = new List<string> { null, "", "   " };

            foreach (var invalidName in invalidNames)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => new Ingredient(2, invalidName!, 1.0, "Kg", false));
                Assert.AreEqual("Name cannot be null or empty.", ex.Message, "Exception message should indicate invalid Name.");
            }
        }

        /// <summary>
        /// Tests that creating an Ingredient with a null or empty Unit throws an ArgumentException.
        /// </summary>
        [Test]
        public void Ingredient_NullOrEmptyUnit_ShouldThrowException()
        {
            // Arrange
            var invalidUnits = new List<string> { null, "", "   " };

            foreach (var invalidUnit in invalidUnits)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => new Ingredient(3, "Salt", 0.5, invalidUnit!, false));
                Assert.AreEqual("Unit cannot be null or empty.", ex.Message, "Exception message should indicate invalid Unit.");
            }
        }

        /// <summary>
        /// Tests that creating an Ingredient with a negative Quantity throws an ArgumentException.
        /// </summary>
        [Test]
        public void Ingredient_NegativeQuantity_ShouldThrowException()
        {
            // Arrange
            var invalidQuantities = new List<double> { -1.0, -0.5 };

            foreach (var invalidQuantity in invalidQuantities)
            {
                // Act & Assert
                var ex = Assert.Throws<ArgumentException>(() => new Ingredient(4, "Sugar", invalidQuantity, "Kg", false));
                Assert.AreEqual("Quantity cannot be negative.", ex.Message, "Exception message should indicate invalid Quantity.");
            }
        }

        /// <summary>
        /// Tests that adding an Ingredient to the class extent increases the TotalIngredients count.
        /// </summary>
        [Test]
        public void Ingredient_AddingIngredient_ShouldIncreaseTotalIngredients()
        {
            // Arrange
            var initialCount = Ingredient.TotalIngredients;

            var ingredient = new Ingredient(5, "Garlic", 0.3, "Kg", true);

            // Act & Assert
            Assert.AreEqual(initialCount + 1, Ingredient.TotalIngredients, "TotalIngredients should increment by one.");
            Assert.Contains(ingredient, (System.Collections.ICollection)Ingredient.GetAll(), "Ingredient should be added to the extent.");
        }

        /// <summary>
        /// Tests that the AverageQuantity property calculates correctly.
        /// </summary>
        [Test]
        public void Ingredient_AverageQuantity_ShouldCalculateCorrectly()
        {
            // Arrange
            var ingredient1 = new Ingredient(6, "Pepper", 1.0, "Kg", false);
            var ingredient2 = new Ingredient(7, "Salt", 0.5, "Kg", false);
            var ingredient3 = new Ingredient(8, "Sugar", 2.0, "Kg", false);

            var expectedAverage = (1.0 + 0.5 + 2.0) / 3.0;

            // Act
            var actualAverage = Ingredient.AverageQuantity;

            // Assert
            Assert.AreEqual(expectedAverage, actualAverage, "AverageQuantity should be calculated correctly.");
        }

        
        /// <summary>
        /// Tests that setting the Ingredient extent to null initializes it as an empty list.
        /// </summary>
        [Test]
        public void SetAll_Null_ShouldInitializeEmptyExtent()
        {
            // Act
            Ingredient.SetAll(null);

            // Assert
            Assert.AreEqual(0, Ingredient.TotalIngredients, "TotalIngredients should be 0 after setting null.");
            Assert.IsEmpty(Ingredient.GetAll(), "Ingredient extent should be empty after setting null.");
        }

        /// <summary>
        /// Tests that the Ingredient's Equals and GetHashCode methods function correctly.
        /// </summary>
        [Test]
        public void Ingredient_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var ingredient1 = new Ingredient(12, "Basil", 0.2, "Kg", true);
            var ingredient2 = new Ingredient(12, "Basil", 0.2, "Kg", true);
            var ingredient3 = new Ingredient(13, "Oregano", 0.1, "Kg", true);

            // Act & Assert
            Assert.IsTrue(ingredient1.Equals(ingredient2), "Ingredients with identical properties should be equal.");
            Assert.AreEqual(ingredient1.GetHashCode(), ingredient2.GetHashCode(), "HashCodes of identical ingredients should be equal.");

            Assert.IsFalse(ingredient1.Equals(ingredient3), "Ingredients with different properties should not be equal.");
            Assert.AreNotEqual(ingredient1.GetHashCode(), ingredient3.GetHashCode(), "HashCodes of different ingredients should not be equal.");
        }

        /// <summary>
        /// Tests that creating an Ingredient with null Comments does not throw an exception.
        /// </summary>
        [Test]
        public void Ingredient_NullComments_ShouldAddSuccessfully()
        {
            // Arrange
            // Note: The Ingredient class does not have a Comments property. This test is irrelevant.
            // If there's a Comments property, adjust accordingly. Otherwise, you can remove this test.
        }

        #endregion

        // ... (Other tests for different classes can be placed here)
    }
}
