using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class IngredientTests
    {
        [SetUp]
        public void SetUp()
        {
            Ingredient.SetAll(new List<Ingredient>());
        }

        [Test]
        public void Ingredient_CreatesObjectCorrectly()
        {
            var ingredient = new Ingredient(1, "Tomato", 10.5, "Fresh tomatoes");
            Assert.That(ingredient.IngredientID, Is.EqualTo(1));
            Assert.That(ingredient.Name, Is.EqualTo("Tomato"));
            Assert.That(ingredient.Quantity, Is.EqualTo(10.5));
            Assert.That(ingredient.Description, Is.EqualTo("Fresh tomatoes"));
        }

        [Test]
        public void Ingredient_ThrowsExceptionForNegativeTotalIngredients()
        {
            Assert.Throws<ArgumentException>(() => Ingredient.TotalIngredients = -1);
        }

        [Test]
        public void Ingredient_ThrowsExceptionForNegativeQuantity()
        {
            Assert.Throws<ArgumentException>(() => new Ingredient(2, "Tomato", -5));
        }

        [Test]
        public void Ingredient_IsCorrectlySavedInExtent()
        {
            var ingredient = new Ingredient(2, "Cheese", 5.0);
            Assert.That(Ingredient.GetAll(), Contains.Item(ingredient));
        }
    }
}