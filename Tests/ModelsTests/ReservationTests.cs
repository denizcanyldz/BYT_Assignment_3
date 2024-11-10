using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class ReservationTests
    {
        [SetUp]
        public void SetUp()
        {
            Reservation.SetAll(new List<Reservation>());
        }

        [Test]
        public void Reservation_CreatesObjectCorrectly()
        {
            var table = new Table(1, 4, "Main Hall", "Round");
            var reservation = new Reservation(1, 101, DateTime.Now.AddDays(1), table, "Confirmed");
            Assert.That(reservation.ReservationID, Is.EqualTo(1));
            Assert.That(reservation.CustomerID, Is.EqualTo(101));
            Assert.That(reservation.ReservationDate.Date, Is.EqualTo(DateTime.Now.AddDays(1).Date));
            Assert.That(reservation.Table, Is.EqualTo(table));
            Assert.That(reservation.Status, Is.EqualTo("Confirmed"));
        }

        [Test]
        public void Reservation_TotalReservationsCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Reservation.TotalReservations = -1);
        }

        [Test]
        public void Reservation_CustomerIDMustBePositive()
        {
            var table = new Table(2, 6, "VIP Section", "Square");
            Assert.Throws<ArgumentException>(() => new Reservation(2, -101, DateTime.Now.AddDays(1), table, "Pending"));
        }

        [Test]
        public void Reservation_ReservationDateCannotBeInPast()
        {
            var table = new Table(3, 2, "Outdoor", "Small Round");
            Assert.Throws<ArgumentException>(() => new Reservation(3, 102, DateTime.Now.AddDays(-1), table, "Confirmed"));
        }

        [Test]
        public void Reservation_StatusCannotBeNullOrEmpty()
        {
            var table = new Table(4, 4, "Patio", "Rectangle");
            Assert.Throws<ArgumentException>(() => new Reservation(4, 103, DateTime.Now.AddDays(1), table, ""));
        }

        [Test]
        public void Reservation_IsCorrectlySavedInExtent()
        {
            var table = new Table(5, 6, "Garden", "Hexagon");
            var reservation = new Reservation(5, 104, DateTime.Now.AddDays(1), table, "Confirmed");
            Assert.That(Reservation.GetAll(), Contains.Item(reservation));
        }

        [Test]
        public void Reservation_AddOrderItemAddsCorrectly()
        {
            var table = new Table(6, 6, "Rooftop", "Oval");
            var reservation = new Reservation(6, 105, DateTime.Now.AddDays(1), table, "Confirmed");
            var orderItem = new OrderItem(1, "Burger", 2, 10.0, "No onions"); // Assuming OrderItem has this constructor
            reservation.AddOrderItem(orderItem);
            Assert.That(reservation.OrderItems, Contains.Item(orderItem));
        }

        [Test]
        public void Reservation_RemoveOrderItemRemovesCorrectly()
        {
            var table = new Table(7, 8, "Terrace", "Circle");
            var reservation = new Reservation(7, 106, DateTime.Now.AddDays(1), table, "Confirmed");
            var orderItem = new OrderItem(2, "Salad", 1, 8.0, "Dressing on the side");
            reservation.AddOrderItem(orderItem);
            reservation.RemoveOrderItem(orderItem);
            Assert.That(reservation.OrderItems, Does.Not.Contain(orderItem));
        }

        [Test]
        public void Reservation_NumberOfGuestsCalculatesCorrectly()
        {
            var table = new Table(8, 10, "Banquet Hall", "Long Table");
            var reservation = new Reservation(8, 107, DateTime.Now.AddDays(1), table, "Confirmed");
            reservation.AddOrderItem(new OrderItem(3, "Pizza", 2, 5.0)); // Quantity 2
            reservation.AddOrderItem(new OrderItem(4, "Pasta", 3, 10.0)); // Quantity 3
            Assert.That(reservation.NumberOfGuests, Is.EqualTo(5));
        }
    }
}
