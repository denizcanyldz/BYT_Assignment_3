using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
   [TestFixture]
    public class InventoryTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset the Inventory class extent before each test to ensure test isolation
            Inventory.SetAll(new List<Inventory>());
        }

        #region Constructor Tests

        [Test]
        public void Constructor_WithValidData_ShouldCreateInventory()
        {
            // Arrange
            int inventoryID = 1;
            DateTime lastRestockDate = DateTime.Now.AddDays(-1); // Yesterday

            // Act
            var inventory = new Inventory(inventoryID, lastRestockDate);

            // Assert
            Assert.AreEqual(inventoryID, inventory.InventoryID, "InventoryID should be set correctly.");
            Assert.AreEqual(lastRestockDate, inventory.LastRestockDate, "LastRestockDate should be set correctly.");
            Assert.AreEqual(1, Inventory.TotalInventories, "TotalInventories should increment to 1 after creating one inventory.");
            CollectionAssert.Contains(Inventory.GetAll(), inventory, "The created inventory should be in the inventories extent.");
        }

        [Test]
        public void Constructor_WithFutureLastRestockDate_ShouldThrowException()
        {
            // Arrange
            int inventoryID = 2;
            DateTime futureDate = DateTime.Now.AddDays(1); // Tomorrow

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Inventory(inventoryID, futureDate));
            Assert.AreEqual("LastRestockDate cannot be in the future.", ex.Message, "Exception message should indicate that LastRestockDate cannot be in the future.");
        }

        #endregion

        #region Property Tests

        [Test]
        public void TotalInventories_ShouldReflectNumberOfInventories()
        {
            // Arrange
            var inventory1 = new Inventory(1, DateTime.Now.AddDays(-2));
            var inventory2 = new Inventory(2, DateTime.Now.AddDays(-1));

            // Act
            int total = Inventory.TotalInventories;

            // Assert
            Assert.AreEqual(2, total, "TotalInventories should reflect the number of created inventories.");
        }

        [Test]
        public void TotalInventories_ShouldNotAllowNegativeValue()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Inventory.TotalInventories = -5);
            Assert.AreEqual("TotalInventories cannot be negative.", ex.Message, "Exception message should indicate that TotalInventories cannot be negative.");
        }

        [Test]
        public void LastRestockDate_SetToFuture_ShouldThrowException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            DateTime futureDate = DateTime.Now.AddDays(5); // 5 days from now

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.LastRestockDate = futureDate);
            Assert.AreEqual("LastRestockDate cannot be in the future.", ex.Message, "Exception message should indicate that LastRestockDate cannot be in the future.");
        }

        [Test]
        public void LastRestockDate_SetToPast_ShouldUpdateCorrectly()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            DateTime newPastDate = DateTime.Now.AddDays(-5);

            // Act
            inventory.LastRestockDate = newPastDate;

            // Assert
            Assert.AreEqual(newPastDate, inventory.LastRestockDate, "LastRestockDate should update correctly when set to a past date.");
        }

        #endregion

        #region Method Tests

        [Test]
        public void AddIngredient_WithValidIngredient_ShouldAddToInventory()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "Pieces", false);

            // Act
            inventory.AddIngredient(ingredient);

            // Assert
            Assert.AreEqual(1, inventory.TotalItems, "TotalItems should increment after adding an ingredient.");
            CollectionAssert.Contains(inventory.Ingredients, ingredient, "Ingredients list should contain the added ingredient.");
        }

        [Test]
        public void AddIngredient_WithNull_ShouldThrowException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.AddIngredient(null));
            Assert.AreEqual("Ingredient cannot be null.", ex.Message, "Exception message should indicate that Ingredient cannot be null.");
        }

        [Test]
        public void RemoveIngredient_WithExistingIngredient_ShouldRemoveFromInventory()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "Pieces", false);
            inventory.AddIngredient(ingredient);

            // Act
            inventory.RemoveIngredient(ingredient);

            // Assert
            Assert.AreEqual(0, inventory.TotalItems, "TotalItems should decrement after removing an ingredient.");
            CollectionAssert.DoesNotContain(inventory.Ingredients, ingredient, "Ingredients list should not contain the removed ingredient.");
        }

        [Test]
        public void RemoveIngredient_WithNull_ShouldThrowException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.RemoveIngredient(null));
            Assert.AreEqual("Ingredient not found.", ex.Message, "Exception message should indicate that Ingredient not found.");
        }

        [Test]
        public void RemoveIngredient_WithNonExistingIngredient_ShouldThrowException()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "Pieces", false);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => inventory.RemoveIngredient(ingredient));
            Assert.AreEqual("Ingredient not found.", ex.Message, "Exception message should indicate that Ingredient not found.");
        }

        [Test]
        public void GetAll_ShouldReturnReadOnlyList()
        {
            // Arrange
            var inventory1 = new Inventory(1, DateTime.Now.AddDays(-2));
            var inventory2 = new Inventory(2, DateTime.Now.AddDays(-1));

            // Act
            var allInventories = Inventory.GetAll();

            // Assert
            Assert.AreEqual(2, allInventories.Count, "GetAll should return the correct number of inventories.");
            Assert.AreEqual(inventory1, allInventories[0], "GetAll should return the correct first inventory.");
            Assert.AreEqual(inventory2, allInventories[1], "GetAll should return the correct second inventory.");

            // Ensure that the list is read-only by attempting to add a new inventory
            Assert.Throws<NotSupportedException>(() =>
            {
                // Cast to IList<Inventory> to access the Add method
                ((IList<Inventory>)allInventories).Add(new Inventory(3, DateTime.Now.AddDays(-3)));
            }, "GetAll should return a read-only list.");
        }

        [Test]
        public void SetAll_WithLoadedInventories_ShouldSetExtentCorrectly()
        {
            // Arrange
            var loadedInventories = new List<Inventory>
            {
                new Inventory(1, DateTime.Now.AddDays(-2)),
                new Inventory(2, DateTime.Now.AddDays(-1))
            };

            // Act
            Inventory.SetAll(loadedInventories);

            // Assert
            Assert.AreEqual(2, Inventory.TotalInventories, "TotalInventories should reflect the loaded inventories.");
            CollectionAssert.AreEqual(loadedInventories, Inventory.GetAll(), "Inventories extent should match the loaded inventories.");
        }

        [Test]
        public void SetAll_WithNull_ShouldInitializeEmptyList()
        {
            // Act
            Inventory.SetAll(null);

            // Assert
            Assert.AreEqual(0, Inventory.TotalInventories, "TotalInventories should be 0 when SetAll is called with null.");
            Assert.IsEmpty(Inventory.GetAll(), "Inventories extent should be empty when SetAll is called with null.");
        }

        #endregion

        #region Derived Attribute Tests

        [Test]
        public void TotalItems_ShouldReflectNumberOfIngredients()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient1 = new Ingredient(1, "Tomato", 50.0, "Pieces", false);
            var ingredient2 = new Ingredient(2, "Lettuce", 30.0, "Leaves", false);

            // Act
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);

            // Assert
            Assert.AreEqual(2, inventory.TotalItems, "TotalItems should reflect the number of added ingredients.");
        }

        [Test]
        public void TotalItems_AfterRemovingIngredient_ShouldReflectCorrectCount()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient1 = new Ingredient(1, "Tomato", 50.0, "Pieces", false);
            var ingredient2 = new Ingredient(2, "Lettuce", 30.0, "Leaves", false);
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);

            // Act
            inventory.RemoveIngredient(ingredient1);

            // Assert
            Assert.AreEqual(1, inventory.TotalItems, "TotalItems should decrement after removing an ingredient.");
        }

        #endregion

        #region Edge Case Tests

        [Test]
        public void AddIngredient_WithDuplicateIngredient_ShouldAllowDuplicates()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient = new Ingredient(1, "Tomato", 50.0, "Pieces", false);

            // Act
            inventory.AddIngredient(ingredient);
            inventory.AddIngredient(ingredient); // Adding duplicate

            // Assert
            Assert.AreEqual(2, inventory.TotalItems, "TotalItems should reflect duplicate ingredients.");
            Assert.AreEqual(2, inventory.Ingredients.Count, "Ingredients list should contain duplicates.");
        }

        [Test]
        public void RemoveIngredient_AfterAddingMultipleIngredients_ShouldRemoveOnlySpecifiedIngredient()
        {
            // Arrange
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            var ingredient1 = new Ingredient(1, "Tomato", 50.0, "Pieces", false);
            var ingredient2 = new Ingredient(2, "Lettuce", 30.0, "Leaves", false);
            var ingredient3 = new Ingredient(3, "Cheese", 20.0, "Grams", true);
            inventory.AddIngredient(ingredient1);
            inventory.AddIngredient(ingredient2);
            inventory.AddIngredient(ingredient3);

            // Act
            inventory.RemoveIngredient(ingredient2);

            // Assert
            Assert.AreEqual(2, inventory.TotalItems, "TotalItems should decrement after removing an ingredient.");
            CollectionAssert.DoesNotContain(inventory.Ingredients, ingredient2, "Ingredients list should not contain the removed ingredient.");
            CollectionAssert.Contains(inventory.Ingredients, ingredient1, "Ingredients list should still contain other ingredients.");
            CollectionAssert.Contains(inventory.Ingredients, ingredient3, "Ingredients list should still contain other ingredients.");
        }

        #endregion
    }
}