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
            var bartender = new Bartender(1, "John Doe", "AB123");
            Assert.That(bartender.StaffID, Is.EqualTo(1));
            Assert.That(bartender.Name, Is.EqualTo("John Doe"));
            Assert.That(bartender.LicenseNumber, Is.EqualTo("AB123"));
        }

        [Test]
        public void Bartender_ThrowsExceptionIfTotalBartendersNegative()
        {
            Assert.Throws<ArgumentException>(() => Bartender.TotalBartenders = -1);
        }

        [Test]
        public void Bartender_ThrowsExceptionForLongLicenseNumber()
        {
            var longLicenseNumber = new string('A', 51);
            Assert.Throws<ArgumentException>(() => new Bartender(1, "John Doe", longLicenseNumber));
        }

        [Test]
        public void Bartender_IsCorrectlySavedInExtent()
        {
            var bartender = new Bartender(2, "Jane Doe");
            Assert.That(Bartender.GetAll(), Contains.Item(bartender));
        }
    }
}