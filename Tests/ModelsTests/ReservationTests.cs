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
            Customer.SetAll(new List<Customer>());
        }

        [Test]
        public void Reservation_CreatesObjectCorrectly()
        {
            var customer = new Customer(101, "Michael");
            var table = new Table(1, 4, "Main Hall", "Round");
            var reservationDate = DateTime.Now.AddDays(1);
            var reservation = new Reservation(1, customer, reservationDate, table, "Confirmed");

            Assert.That(reservation.ReservationID, Is.EqualTo(1));
            Assert.That(reservation.Customer, Is.EqualTo(customer));
            Assert.That(reservation.Customer.CustomerID, Is.EqualTo(101));
            Assert.That(reservation.ReservationDate.Date, Is.EqualTo(reservationDate.Date));
            Assert.That(reservation.Table, Is.EqualTo(table));
            Assert.That(reservation.Status, Is.EqualTo("Confirmed"));
        }

        [Test]
        public void Reservation_TotalReservationsCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Reservation.TotalReservations = -1);
        }

        [Test]
        public void Reservation_CustomerMustNotBeNull()
        {
            var table = new Table(2, 6, "VIP Section", "Square");
            Assert.Throws<ArgumentException>(() => new Reservation(2, null, DateTime.Now.AddDays(1), table, "Pending"),
                "Passing a null customer should throw an exception.");
        }

        [Test]
        public void Reservation_ReservationDateCannotBeInPast()
        {
            var customer = new Customer(102, "John");
            var table = new Table(3, 2, "Outdoor", "Small Round");
            Assert.Throws<ArgumentException>(() => new Reservation(3, customer, DateTime.Now.AddDays(-1), table, "Confirmed"));
        }

        [Test]
        public void Reservation_StatusCannotBeNullOrEmpty()
        {
            var customer = new Customer(103, "Alice");
            var table = new Table(4, 4, "Patio", "Rectangle");
            Assert.Throws<ArgumentException>(() => new Reservation(4, customer, DateTime.Now.AddDays(1), table, ""));
        }

        [Test]
        public void Reservation_IsCorrectlySavedInExtent()
        {
            var customer = new Customer(104, "Eve");
            var table = new Table(5, 6, "Garden", "Hexagon");
            var reservation = new Reservation(5, customer, DateTime.Now.AddDays(1), table, "Confirmed");
            Assert.That(Reservation.GetAll(), Contains.Item(reservation));
        }

        [Test]
        public void Reservation_AddOrderItemAddsCorrectly()
        {
            var customer = new Customer(105, "Frank");
            var table = new Table(6, 6, "Rooftop", "Oval");
            var reservation = new Reservation(6, customer, DateTime.Now.AddDays(1), table, "Confirmed");
            var orderItem = new OrderItem(1, "Burger", 2, 10.0, "No onions");
            reservation.AddOrderItem(orderItem);
            Assert.That(reservation.OrderItems, Contains.Item(orderItem));
        }

        [Test]
        public void Reservation_RemoveOrderItemRemovesCorrectly()
        {
            var customer = new Customer(106, "George");
            var table = new Table(7, 8, "Terrace", "Circle");
            var reservation = new Reservation(7, customer, DateTime.Now.AddDays(1), table, "Confirmed");
            var orderItem = new OrderItem(2, "Salad", 1, 8.0, "Dressing on the side");
            reservation.AddOrderItem(orderItem);
            reservation.RemoveOrderItem(orderItem);
            Assert.That(reservation.OrderItems, Does.Not.Contain(orderItem));
        }

        [Test]
        public void Reservation_NumberOfGuestsCalculatesCorrectly()
        {
            var customer = new Customer(107, "Hank");
            var table = new Table(8, 10, "Banquet Hall", "Long Table");
            var reservation = new Reservation(8, customer, DateTime.Now.AddDays(1), table, "Confirmed");
            reservation.AddOrderItem(new OrderItem(3, "Pizza", 2, 5.0));
            reservation.AddOrderItem(new OrderItem(4, "Pasta", 3, 10.0));
            Assert.That(reservation.NumberOfGuests, Is.EqualTo(5));
        }

        [Test]
        public void Status_ShouldThrowException_WhenNull()
        {
            var customer = new Customer(2, "Irene");
            var table = new Table(1, 4);
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(1, customer, DateTime.Now.AddDays(1), table, null));
            Assert.That(ex.Message, Is.EqualTo("Status cannot be null or empty."));
        }

        [Test]
        public void Table_ShouldThrowException_WhenNull()
        {
            var customer = new Customer(2, "Jacob");
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(1, customer, DateTime.Now.AddDays(1), null, "Reserved"));
            Assert.That(ex.Message, Is.EqualTo("Table cannot be null."));
        }
    }
}
