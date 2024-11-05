using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class ChefTests
    {
        [SetUp]
        public void SetUp()
        {
            Chef.SetAll(new List<Chef>());
        }

        [Test]
        public void Chef_CreatesObjectCorrectly()
        {
            var chef = new Chef(1, "Alice", "Italian Cuisine");
            Assert.That(chef.StaffID, Is.EqualTo(1));
            Assert.That(chef.Name, Is.EqualTo("Alice"));
            Assert.That(chef.Specialty, Is.EqualTo("Italian Cuisine"));
        }

        [Test]
        public void Chef_ThrowsExceptionIfTotalChefsNegative()
        {
            Assert.Throws<ArgumentException>(() => Chef.TotalChefs = -1);
        }

        [Test]
        public void Chef_ThrowsExceptionForLongSpecialty()
        {
            var longSpecialty = new string('A', 101);
            Assert.Throws<ArgumentException>(() => new Chef(1, "Alice", longSpecialty));
        }

        [Test]
        public void Chef_IsCorrectlySavedInExtent()
        {
            var chef = new Chef(2, "Bob");
            Assert.That(Chef.GetAll(), Contains.Item(chef));
        }
    }
}