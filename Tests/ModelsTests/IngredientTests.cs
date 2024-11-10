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
            var ingredient = new Ingredient(1, "Tomato", 1.5, "kg", true);
            Assert.That(ingredient.IngredientID, Is.EqualTo(1));
            Assert.That(ingredient.Name, Is.EqualTo("Tomato"));
            Assert.That(ingredient.Quantity, Is.EqualTo(1.5));
            Assert.That(ingredient.Unit, Is.EqualTo("kg"));
        }

        [Test]
        public void Ingredient_TotalIngredientsCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Ingredient.TotalIngredients = -1);
        }

        [Test]
        public void Ingredient_NameCannotBeNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new Ingredient(2, "", 0.5, "g", true));
        }

        [Test]
        public void Ingredient_IsCorrectlySavedInExtent()
        {
            var ingredient = new Ingredient(3, "Lettuce", 0.2, "kg", false);
            Assert.That(Ingredient.GetAll(), Contains.Item(ingredient));
        }
    }
}