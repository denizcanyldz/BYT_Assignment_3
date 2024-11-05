using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class InventoryTests
    {
        [SetUp]
        public void SetUp()
        {
            Inventory.SetAll(new List<Inventory>());
        }

        [Test]
        public void Inventory_CreatesObjectCorrectly()
        {
            var inventory = new Inventory(1, DateTime.Now.AddDays(-1));
            Assert.That(inventory.InventoryID, Is.EqualTo(1));
            Assert.That(inventory.LastRestockDate.Date, Is.EqualTo(DateTime.Now.AddDays(-1).Date));
        }

        [Test]
        public void Inventory_ThrowsExceptionForFutureRestockDate()
        {
            Assert.Throws<ArgumentException>(() => new Inventory(2, DateTime.Now.AddDays(1)));
        }

        [Test]
        public void Inventory_IsCorrectlySavedInExtent()
        {
            var inventory = new Inventory(2, DateTime.Now.AddDays(-1));
            Assert.That(Inventory.GetAll(), Contains.Item(inventory));
        }
    }
}