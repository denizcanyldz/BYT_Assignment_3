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

        [TearDown]
        public void TearDown()
        {
            Waiter.SetAll(new List<Waiter>());
            Waiter.TotalWaiters = 0;
        }

        [Test]
        public void Waiter_CreatesObjectCorrectly()
        {
            // Arrange & Act
            var waiter = new Waiter(
                staffID: 1,
                name: "Daniel",
                contactNumber: "987-654-3210"
                
            );

            // Assert
            Assert.That(waiter.StaffID, Is.EqualTo(1));
            Assert.That(waiter.Name, Is.EqualTo("Daniel"));
            Assert.That(waiter.TipsCollected, Is.True);
            Assert.That(waiter.ContactNumber, Is.EqualTo("987-654-3210"));
        }

        [Test]
        public void Waiter_TotalWaitersCannotBeNegative()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Waiter.TotalWaiters = -1);
            Assert.AreEqual("TotalWaiters cannot be negative.", ex.Message);
        }

        [Test]
        public void Waiter_IsCorrectlySavedInExtent()
        {
            // Arrange
            var waiter = new Waiter(
                staffID: 2,
                name: "Eve"
                // contactNumber defaults to null
            );

            // Act
            var allWaiters = Waiter.GetAll();

            // Assert
            Assert.That(allWaiters, Contains.Item(waiter));
        }
    }
}