using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class WaiterTests
    {
        [SetUp]
        public void SetUp()
        {
            Waiter.SetAll(new List<Waiter>());
        }

        [Test]
        public void Waiter_CreatesObjectCorrectly()
        {
            var waiter = new Waiter(1, "Daniel", true, "987-654-3210");
            Assert.That(waiter.StaffID, Is.EqualTo(1));
            Assert.That(waiter.Name, Is.EqualTo("Daniel"));
            Assert.That(waiter.TipsCollected, Is.True);
            Assert.That(waiter.ContactNumber, Is.EqualTo("987-654-3210"));
        }

        [Test]
        public void Waiter_TotalWaitersCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Waiter.TotalWaiters = -1);
        }

        [Test]
        public void Waiter_IsCorrectlySavedInExtent()
        {
            var waiter = new Waiter(2, "Eve", false);
            Assert.That(Waiter.GetAll(), Contains.Item(waiter));
        }
    }
}