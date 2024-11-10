using BYT_Assignment_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class WaiterBartenderTests
    {
        [SetUp]
        public void SetUp()
        {
            WaiterBartender.SetAll(new List<WaiterBartender>());
        }

        [Test]
        public void WaiterBartender_CreatesObjectCorrectly()
        {
            var waiterBartender = new WaiterBartender(1, "Alice", 100.0, "123-456-7890");
            Assert.That(waiterBartender.StaffID, Is.EqualTo(1));
            Assert.That(waiterBartender.Name, Is.EqualTo("Alice"));
            Assert.That(waiterBartender.Bonus, Is.EqualTo(100.0));
            Assert.That(waiterBartender.ContactNumber, Is.EqualTo("123-456-7890"));
        }

        [Test]
        public void WaiterBartender_TotalWaiterBartendersCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => WaiterBartender.TotalWaiterBartenders = -1);
        }

        [Test]
        public void WaiterBartender_BonusCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => new WaiterBartender(2, "Bob", -10.0));
        }

        [Test]
        public void WaiterBartender_IsCorrectlySavedInExtent()
        {
            var waiterBartender = new WaiterBartender(3, "Charlie", 50.0);
            Assert.That(WaiterBartender.GetAll(), Contains.Item(waiterBartender));
        }
    }
}
