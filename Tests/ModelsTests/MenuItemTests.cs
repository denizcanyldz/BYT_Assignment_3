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
            var menuItem = new MenuItem(1, "Burger", 5.0, 500, 4.5, 10, true);
            Assert.That(menuItem.MenuItemID, Is.EqualTo(1));
            Assert.That(menuItem.Name, Is.EqualTo("Burger"));
            Assert.That(menuItem.BasePrice, Is.EqualTo(5.0));
            Assert.That(menuItem.Calories, Is.EqualTo(500));
            Assert.That(menuItem.IsAvailable, Is.True);
        }

        [Test]
        public void MenuItem_TotalMenuItemsCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => MenuItem.TotalMenuItems = -1);
        }

        [Test]
        public void MenuItem_NameCannotBeNullOrEmpty()
        {
            Assert.Throws<ArgumentException>(() => new MenuItem(2, "", 5.0, 300, 4.0, 15));
        }

        [Test]
        public void MenuItem_IsCorrectlySavedInExtent()
        {
            var menuItem = new MenuItem(3, "Pasta", 7.5, 600, 6.0, 20);
            Assert.That(MenuItem.GetAll(), Contains.Item(menuItem));
        }
    }
}
