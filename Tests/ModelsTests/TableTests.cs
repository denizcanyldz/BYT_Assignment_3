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
            Table.TotalTables = 0;
        }

        [Test]
        public void Constructor_ShouldInitializeTableCorrectly()
        {
            var table = new Table(1, 4, "Main Hall", "Round");

            Assert.That(table.TableNumber, Is.EqualTo(1));
            Assert.That(table.MaxSeats, Is.EqualTo(4));
            Assert.That(table.Location, Is.EqualTo("Main Hall"));
            Assert.That(table.SeatingArrangement, Is.EqualTo("Round"));
            Assert.That(Table.TotalTables, Is.EqualTo(1));
        }

        [Test]
        public void MaxSeats_ShouldThrowException_WhenZeroOrNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Table(1, 0));
            Assert.That(ex.Message, Is.EqualTo("MaxSeats must be greater than zero."));
        }

        [Test]
        public void Location_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longLocation = new string('A', 101);
            var ex = Assert.Throws<ArgumentException>(() => new Table(1, 4, longLocation));
            Assert.That(ex.Message, Is.EqualTo("Location length cannot exceed 100 characters."));
        }

        [Test]
        public void SeatingArrangement_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longArrangement = new string('A', 51);
            var ex = Assert.Throws<ArgumentException>(() => new Table(1, 4, seatingArrangement: longArrangement));
            Assert.That(ex.Message, Is.EqualTo("SeatingArrangement length cannot exceed 50 characters."));
        }

        [Test]
        public void AddOrder_ShouldAddOrderToTable()
        {
            var table = new Table(1, 4);
            var order = new Order(1, DateTime.Now);
            table.AddOrder(order);

            Assert.Contains(order, (System.Collections.ICollection)table.Orders);
            Assert.IsTrue(table.IsOccupied);
        }

        [Test]
        public void AddOrder_ShouldThrowException_WhenOrderIsNull()
        {
            var table = new Table(1, 4);
            var ex = Assert.Throws<ArgumentException>(() => table.AddOrder(null));
            Assert.That(ex.Message, Is.EqualTo("Order cannot be null."));
        }

        [Test]
        public void RemoveOrder_ShouldRemoveOrderFromTable()
        {
            var table = new Table(1, 4);
            var order = new Order(1, DateTime.Now);
            table.AddOrder(order);

            table.RemoveOrder(order);
            Assert.IsFalse(table.Orders.Contains(order));
            Assert.IsFalse(table.IsOccupied);
        }

        [Test]
        public void RemoveOrder_ShouldThrowException_WhenOrderNotFound()
        {
            var table = new Table(1, 4);
            var order1 = new Order(1, DateTime.Now);
            var order2 = new Order(2, DateTime.Now);

            table.AddOrder(order1);
            var ex = Assert.Throws<ArgumentException>(() => table.RemoveOrder(order2));
            Assert.That(ex.Message, Is.EqualTo("Order not found."));
        }

        [Test]
        public void GetAll_ShouldReturnAllTables()
        {
            var table1 = new Table(1, 4);
            var table2 = new Table(2, 6);

            var allTables = Table.GetAll();
            Assert.That(allTables.Count, Is.EqualTo(2));
            Assert.Contains(table1, (System.Collections.ICollection)allTables);
            Assert.Contains(table2, (System.Collections.ICollection)allTables);
        }

        [Test]
        public void SetAll_ShouldUpdateTablesListCorrectly()
        {
            var table1 = new Table(1, 4);

            var newTables = new List<Table> { table1 };
            Table.SetAll(newTables);

            var allTables = Table.GetAll();
            Assert.That(allTables.Count, Is.EqualTo(1));
            Assert.Contains(table1, (System.Collections.ICollection)allTables);
            Assert.That(Table.TotalTables, Is.EqualTo(1));
        }

        [Test]
        public void TotalTables_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Table.TotalTables = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalTables cannot be negative."));
        }
    }
}