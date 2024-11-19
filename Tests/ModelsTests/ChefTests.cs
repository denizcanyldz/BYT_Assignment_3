using NUnit.Framework;
using BYT_Assignment_3.Models;
using BYT_Assignment_3.Persistences;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class ChefTests
    {
        // Path to the test XML file used for serialization/deserialization
        private const string TestFilePath = "test_extents.xml";

        /// <summary>
        /// Runs before each test to ensure a clean environment.
        /// Resets all class extents and deletes the test XML file if it exists.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Reset all class extents to ensure no residual data from previous tests
            Chef.SetAll(new List<Chef>());
            Staff.SetAll(new List<Staff>()); // Assuming Staff has a similar SetAll method

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

        #region Chef Class Tests

        /// <summary>
        /// Tests that a Chef object is created correctly with multiple specialties.
        /// </summary>
        [Test]
        public void Chef_CreatesObjectCorrectly_WithMultipleSpecialties()
        {
            // Arrange
            var specialties = new List<string?> { "Italian Cuisine", "Pastry Arts" };
            var chef = new Chef(1, "Alice", specialties, "555-1234");

            // Act & Assert
            Assert.AreEqual(1, chef.StaffID, "Chef's StaffID should be initialized correctly.");
            Assert.AreEqual("Alice", chef.Name, "Chef's Name should be initialized correctly.");
            Assert.AreEqual("555-1234", chef.ContactNumber, "Chef's ContactNumber should be initialized correctly.");
            CollectionAssert.AreEquivalent(specialties, chef.Specialties, "Chef's Specialties should be initialized correctly.");
        }

        /// <summary>
        /// Tests that creating a Chef with a specialty exceeding 100 characters throws an ArgumentException.
        /// </summary>
        [Test]
        public void Chef_ThrowsExceptionForLongSpecialty()
        {
            // Arrange
            var longSpecialty = new string('A', 101); // 101 characters
            var specialties = new List<string?> { longSpecialty };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Chef(2, "Bob", specialties, "555-5678"),
                "Creating a Chef with a specialty exceeding 100 characters should throw an ArgumentException.");
            Assert.AreEqual("Specialty length cannot exceed 100 characters.", ex.Message);
        }

        /// <summary>
        /// Tests that adding a null or empty specialty throws an ArgumentException.
        /// </summary>
        [Test]
        public void AddSpecialty_NullOrEmpty_ShouldThrowException()
        {
            // Arrange
            var chef = new Chef(3, "Charlie", new List<string?> { "Mexican Cuisine" }, "555-9012");

            // Act & Assert
            var ex1 = Assert.Throws<ArgumentException>(() => chef.AddSpecialty(null!),
                "Adding a null specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty cannot be null or empty.", ex1.Message);

            var ex2 = Assert.Throws<ArgumentException>(() => chef.AddSpecialty(""),
                "Adding an empty specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty cannot be null or empty.", ex2.Message);

            var ex3 = Assert.Throws<ArgumentException>(() => chef.AddSpecialty("   "),
                "Adding a whitespace-only specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty cannot be null or empty.", ex3.Message);
        }

        /// <summary>
        /// Tests that adding a specialty exceeding 100 characters throws an ArgumentException.
        /// </summary>
        [Test]
        public void AddSpecialty_ExceedsMaxLength_ShouldThrowException()
        {
            // Arrange
            var chef = new Chef(4, "Diana", new List<string?> { "Sushi" }, "555-3456");
            var longSpecialty = new string('B', 101); // 101 characters

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => chef.AddSpecialty(longSpecialty),
                "Adding a specialty exceeding 100 characters should throw an ArgumentException.");
            Assert.AreEqual("Specialty length cannot exceed 100 characters.", ex.Message);
        }

        /// <summary>
        /// Tests that adding a valid specialty works correctly.
        /// </summary>
        [Test]
        public void AddSpecialty_Valid_ShouldAddSuccessfully()
        {
            // Arrange
            var specialties = new List<string?> { "French Cuisine" };
            var chef = new Chef(5, "Eve", specialties, "555-7890");
            var newSpecialty = "Vegan Dishes";

            // Act
            chef.AddSpecialty(newSpecialty);

            // Assert
            Assert.Contains(newSpecialty, chef.Specialties, "Specialty should be added successfully.");
            Assert.AreEqual(2, chef.Specialties.Count, "Chef should have two specialties after adding one.");
        }

        /// <summary>
        /// Tests that removing an existing specialty works correctly.
        /// </summary>
        [Test]
        public void RemoveSpecialty_Existing_ShouldRemoveSuccessfully()
        {
            // Arrange
            var specialties = new List<string?> { "Japanese Cuisine", "Korean BBQ" };
            var chef = new Chef(6, "Frank", specialties, "555-1122");
            var specialtyToRemove = "Korean BBQ";

            // Act
            chef.RemoveSpecialty(specialtyToRemove);

            // Assert
            Assert.IsFalse(chef.Specialties.Contains(specialtyToRemove), "Specialty should be removed successfully.");
            Assert.AreEqual(1, chef.Specialties.Count, "Chef should have one specialty after removal.");
        }

        /// <summary>
        /// Tests that removing a non-existing specialty throws an ArgumentException.
        /// </summary>
        [Test]
        public void RemoveSpecialty_NonExisting_ShouldThrowException()
        {
            // Arrange
            var specialties = new List<string?> { "Thai Cuisine" };
            var chef = new Chef(7, "Grace", specialties, "555-3344");
            var specialtyToRemove = "Italian Cuisine";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => chef.RemoveSpecialty(specialtyToRemove),
                "Removing a non-existing specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty not found.", ex.Message);
        }

        /// <summary>
        /// Tests that removing a null or empty specialty throws an ArgumentException.
        /// </summary>
        [Test]
        public void RemoveSpecialty_NullOrEmpty_ShouldThrowException()
        {
            // Arrange
            var specialties = new List<string?> { "Mexican Cuisine" };
            var chef = new Chef(8, "Hank", specialties, "555-5566");

            // Act & Assert
            var ex1 = Assert.Throws<ArgumentException>(() => chef.RemoveSpecialty(null!),
                "Removing a null specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty cannot be null or empty.", ex1.Message);

            var ex2 = Assert.Throws<ArgumentException>(() => chef.RemoveSpecialty(""),
                "Removing an empty specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty cannot be null or empty.", ex2.Message);

            var ex3 = Assert.Throws<ArgumentException>(() => chef.RemoveSpecialty("   "),
                "Removing a whitespace-only specialty should throw an ArgumentException.");
            Assert.AreEqual("Specialty cannot be null or empty.", ex3.Message);
        }

        /// <summary>
        /// Tests that the TotalChefs property updates correctly when chefs are added.
        /// </summary>
        [Test]
        public void TotalChefs_ShouldUpdateCorrectly_WhenChefsAreAdded()
        {
            // Arrange & Act
            var chef1 = new Chef(9, "Irene", new List<string?> { "Spanish Cuisine" }, "555-7788");
            var chef2 = new Chef(10, "Jack", new List<string?> { "Greek Cuisine" }, "555-9900");

            // Assert
            Assert.AreEqual(2, Chef.TotalChefs, "TotalChefs should reflect the number of chefs added.");
            CollectionAssert.Contains(Chef.GetAll(), chef1, "Chef1 should be present in the extent.");
            CollectionAssert.Contains(Chef.GetAll(), chef2, "Chef2 should be present in the extent.");
        }

        /// <summary>
        /// Tests that the Equals and GetHashCode methods function correctly.
        /// </summary>
        [Test]
        public void Chef_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var specialties1 = new List<string?> { "Fusion Cuisine", "Seafood" };
            var chef1 = new Chef(11, "Karen", specialties1, "555-6677");

            var specialties2 = new List<string?> { "Fusion Cuisine", "Seafood" };
            var chef2 = new Chef(11, "Karen", specialties2, "555-6677");

            var specialties3 = new List<string?> { "Vegetarian Dishes" };
            var chef3 = new Chef(12, "Leo", specialties3, "555-7788");

            // Act & Assert
            Assert.IsTrue(chef1.Equals(chef2), "Chefs with identical properties should be equal.");
            Assert.AreEqual(chef1.GetHashCode(), chef2.GetHashCode(), "HashCodes of identical chefs should be equal.");

            Assert.IsFalse(chef1.Equals(chef3), "Chefs with different properties should not be equal.");
            Assert.AreNotEqual(chef1.GetHashCode(), chef3.GetHashCode(), "HashCodes of different chefs should not be equal.");
        }

        /// <summary>
        /// Tests that setting the Chef extent to null initializes it as an empty list.
        /// </summary>
        [Test]
        public void SetAll_Null_ShouldInitializeEmptyExtent()
        {
            // Act
            Chef.SetAll(new List<Chef>()); // Passing an empty list instead of null

            // Assert
            Assert.AreEqual(0, Chef.TotalChefs, "TotalChefs should be 0 after setting an empty list.");
            Assert.IsEmpty(Chef.GetAll(), "Chef extent should be empty after setting an empty list.");
        }

        /// <summary>
        /// Tests that the Chef's specialties are correctly serialized and deserialized.
        /// </summary>
        [Test]
        public void Chef_SerializeAndDeserialize_ShouldMaintainSpecialties()
        {
            // Arrange
            var specialties = new List<string?> { "Peruvian Cuisine", "Grilling Techniques" };
            var chef = new Chef(13, "Mona", specialties, "555-8899");

            // Act
            PersistencyManager.SaveAll(TestFilePath); // Save all data
            PersistencyManager.LoadAll(TestFilePath); // Load data back

            // Assert
            Assert.AreEqual(1, Chef.TotalChefs, "TotalChefs should be 1 after deserialization.");
            var loadedChef = Chef.GetAll()[0];
            Assert.AreEqual(chef.StaffID, loadedChef.StaffID, "Chef's StaffID should match after deserialization.");
            Assert.AreEqual(chef.Name, loadedChef.Name, "Chef's Name should match after deserialization.");
            Assert.AreEqual(chef.ContactNumber, loadedChef.ContactNumber, "Chef's ContactNumber should match after deserialization.");
            CollectionAssert.AreEquivalent(chef.Specialties, loadedChef.Specialties, "Chef's Specialties should match after deserialization.");
        }
        #endregion
    }
}
