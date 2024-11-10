using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class BartenderTests
    {
        [SetUp]
        public void SetUp()
        {
            Bartender.SetAll(new List<Bartender>());
        }

        [Test]
        public void Bartender_CreatesObjectCorrectly()
        {
            var bartender = new Bartender(1, "Gooner", "123-456-7890");
            Assert.That(bartender.StaffID, Is.EqualTo(1));
            Assert.That(bartender.Name, Is.EqualTo("Gooner"));
            Assert.That(bartender.ContactNumber, Is.EqualTo("123-456-7890"));
        }

        [Test]
        public void Bartender_TotalBartendersCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Bartender.TotalBartenders = -1);
        }

        [Test]
        public void Bartender_IsCorrectlySavedInExtent()
        {
            var bartender = new Bartender(2, "Jane");
            Assert.That(Bartender.GetAll(), Contains.Item(bartender));
        }

        [Test]
        public void Bartender_TotalBartendersUpdatesCorrectly()
        {
            new Bartender(3, "Tom");
            new Bartender(4, "Emma");
            Assert.That(Bartender.TotalBartenders, Is.EqualTo(2));
        }

        [Test]
        public void Bartender_SetAllCorrectlyUpdatesTotalBartenders()
        {
            var bartenderList = new List<Bartender>
            {
                new Bartender(5, "Alice"),
                new Bartender(6, "Bob")
            };
            Bartender.SetAll(bartenderList);
            Assert.That(Bartender.TotalBartenders, Is.EqualTo(2));
            Assert.That(Bartender.GetAll().Count, Is.EqualTo(2));
        }
    }
}