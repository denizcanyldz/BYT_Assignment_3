using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class TableTests
    {
        [SetUp]
        public void SetUp()
        {
            Table.SetAll(new List<Table>());
        }

        [Test]
        public void Table_CreatesObjectCorrectly()
        {
            var table = new Table(1, 4, "Main Hall", "Round");
            Assert.That(table.TableNumber, Is.EqualTo(1));
            Assert.That(table.MaxSeats, Is.EqualTo(4));
            Assert.That(table.Location, Is.EqualTo("Main Hall"));
            Assert.That(table.SeatingArrangement, Is.EqualTo("Round"));
        }

        [Test]
        public void Table_TotalTablesCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Table.TotalTables = -1);
        }

        [Test]
        public void Table_MaxSeatsMustBeGreaterThanZero()
        {
            Assert.Throws<ArgumentException>(() => new Table(2, 0, "VIP Section", "Square"));
        }

        [Test]
        public void Table_IsCorrectlySavedInExtent()
        {
            var table = new Table(3, 6, "Outdoor", "Small Round");
            Assert.That(Table.GetAll(), Contains.Item(table));
        }
    }
}