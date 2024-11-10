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
            Waiter.TotalWaiters = 0;
        }

        [Test]
        public void Constructor_ShouldInitializeWaiterCorrectly()
        {
            var waiter = new Waiter(1, "John Doe", "Section A", "Day Shift");

            Assert.That(waiter.StaffID, Is.EqualTo(1));
            Assert.That(waiter.Name, Is.EqualTo("John Doe"));
            Assert.That(waiter.Section, Is.EqualTo("Section A"));
            Assert.That(waiter.ContactNumber, Is.EqualTo("Day Shift"));
            Assert.That(Waiter.TotalWaiters, Is.EqualTo(1));
        }

        [Test]
        public void Section_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longSection = new string('A', 101);
            var ex = Assert.Throws<ArgumentException>(() => new Waiter(1, "John Doe", longSection));
            Assert.That(ex.Message, Is.EqualTo("Section length cannot exceed 100 characters."));
        }

        [Test]
        public void SetAll_ShouldUpdateWaitersListCorrectly()
        {
            var waiter1 = new Waiter(1, "John Doe");

            var newWaiters = new List<Waiter> { waiter1 };
            Waiter.SetAll(newWaiters);

            var allWaiters = Waiter.GetAll();
            Assert.That(allWaiters.Count, Is.EqualTo(1));
            Assert.Contains(waiter1, (System.Collections.ICollection)allWaiters);
            Assert.That(Waiter.TotalWaiters, Is.EqualTo(1));
        }

        [Test]
        public void TotalWaiters_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Waiter.TotalWaiters = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalWaiters cannot be negative."));
        }

        [Test]
        public void GetAll_ShouldReturnAllWaiters()
        {
            var waiter1 = new Waiter(1, "John Doe");
            var waiter2 = new Waiter(2, "Jane Smith");

            var allWaiters = Waiter.GetAll();
            Assert.That(allWaiters.Count, Is.EqualTo(2));
            Assert.Contains(waiter1, (System.Collections.ICollection)allWaiters);
            Assert.Contains(waiter2, (System.Collections.ICollection)allWaiters);
        }
    }
}