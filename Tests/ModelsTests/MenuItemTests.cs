using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class MenuItemTests
    {
        [SetUp]
        public void SetUp()
        {
            MenuItem.SetAll(new List<MenuItem>());
        }

        [Test]
        public void MenuItem_CreatesObjectCorrectly()
        {
            var menuItem = new MenuItem(1, "Pasta", 15.0, "Italian pasta", true);
            Assert.That(menuItem.MenuItemID, Is.EqualTo(1));
            Assert.That(menuItem.Name, Is.EqualTo("Pasta"));
            Assert.That(menuItem.BasePrice, Is.EqualTo(15.0));
            Assert.That(menuItem.Description, Is.EqualTo("Italian pasta"));
            Assert.That(menuItem.IsAvailable, Is.True);
        }

        [Test]
        public void MenuItem_ThrowsExceptionForNegativeTotalMenuItems()
        {
            Assert.Throws<ArgumentException>(() => MenuItem.TotalMenuItems = -1);
        }

        [Test]
        public void MenuItem_ThrowsExceptionForNegativeBasePrice()
        {
            Assert.Throws<ArgumentException>(() => new MenuItem(2, "Soup", -10));
        }

        [Test]
        public void MenuItem_IsCorrectlySavedInExtent()
        {
            var menuItem = new MenuItem(3, "Salad", 8.5);
            Assert.That(MenuItem.GetAll(), Contains.Item(menuItem));
        }
    }
}
