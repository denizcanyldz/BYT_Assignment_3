using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{ 
    [TestFixture]
    public class ManagerTests 
    {
        [SetUp]
        public void SetUp()
        {
            Manager.SetAll(new List<Manager>());
        }

        [Test]
        public void Manager_CreatesObjectCorrectly()
        {
            var manager = new Manager(1, "Sam Smith", "HR");
            Assert.That(manager.ManagerID, Is.EqualTo(1));
            Assert.That(manager.Name, Is.EqualTo("Sam Smith"));
            Assert.That(manager.Department, Is.EqualTo("HR"));
        }

        [Test]
        public void Manager_ThrowsExceptionForNegativeTotalManagers()
        {
            Assert.Throws<ArgumentException>(() => Manager.TotalManagers = -1);
        }

        [Test]
        public void Manager_IsCorrectlySavedInExtent()
        {
            var manager = new Manager(2, "Jamie Lee");
            Assert.That(Manager.GetAll(), Contains.Item(manager));
        }
    }
}