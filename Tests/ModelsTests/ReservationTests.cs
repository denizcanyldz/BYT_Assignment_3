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
            Reservation.TotalReservations = 0;
        }

        [Test]
        public void Constructor_ShouldInitializeReservationCorrectly()
        {
            var reservation = new Reservation(1, 1001, DateTime.Now.AddDays(1), "Window seat");

            Assert.That(reservation.ReservationID, Is.EqualTo(1));
            Assert.That(reservation.CustomerID, Is.EqualTo(1001));
            Assert.That(reservation.SpecialRequests, Is.EqualTo("Window seat"));
            Assert.That(Reservation.TotalReservations, Is.EqualTo(1));
        }

        [Test]
        public void CustomerID_ShouldThrowException_WhenNegativeOrZero()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(1, 0, DateTime.Now.AddDays(1)));
            Assert.That(ex.Message, Is.EqualTo("CustomerID must be positive."));
        }

        [Test]
        public void ReservationDate_ShouldThrowException_WhenInThePast()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(1, 1001, DateTime.Now.AddDays(-1)));
            Assert.That(ex.Message, Is.EqualTo("ReservationDate cannot be in the past."));
        }

        [Test]
        public void SpecialRequests_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longRequest = new string('A', 301);
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(1, 1001, DateTime.Now.AddDays(1), longRequest));
            Assert.That(ex.Message, Is.EqualTo("SpecialRequests length cannot exceed 300 characters."));
        }

        [Test]
        public void AddOrderItem_ShouldAddItemToOrderItemsList()
        {
            var reservation = new Reservation(1, 1001, DateTime.Now.AddDays(1));
            var item = new OrderItem(101, "Test Item", 2, 10.0, "Special instructions");

            reservation.AddOrderItem(item);

            Assert.Contains(item, (System.Collections.ICollection)reservation.OrderItems);
            Assert.That(reservation.OrderItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddOrderItem_ShouldThrowException_WhenItemIsNull()
        {
            var reservation = new Reservation(1, 1001, DateTime.Now.AddDays(1));
            var ex = Assert.Throws<ArgumentException>(() => reservation.AddOrderItem(null));
            Assert.That(ex.Message, Is.EqualTo("OrderItem cannot be null."));
        }

        [Test]
        public void RemoveOrderItem_ShouldRemoveItemFromOrderItemsList()
        {
            var reservation = new Reservation(1, 1001, DateTime.Now.AddDays(1));
            var item = new OrderItem(101, "Test Item", 2, 10.0, "Special instructions");
            reservation.AddOrderItem(item);

            reservation.RemoveOrderItem(item);

            Assert.IsFalse(reservation.OrderItems.Contains(item));
            Assert.That(reservation.OrderItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveOrderItem_ShouldThrowException_WhenItemNotFound()
        {
            var reservation = new Reservation(1, 1001, DateTime.Now.AddDays(1));
            var item = new OrderItem(101, "Test Item", 2, 10.0, "Special instructions");

            var ex = Assert.Throws<ArgumentException>(() => reservation.RemoveOrderItem(item));
            Assert.That(ex.Message, Is.EqualTo("OrderItem not found."));
        }

        [Test]
        public void NumberOfGuests_ShouldCalculateTotalQuantityOfOrderItems()
        {
            var reservation = new Reservation(1, 1001, DateTime.Now.AddDays(1));
            reservation.AddOrderItem(new OrderItem(101, "Test Item", 2, 10.0, "Special instructions"));
            reservation.AddOrderItem(new OrderItem(102, "Test Item", 3, 10.0, "Special instructions"));

            Assert.That(reservation.NumberOfGuests, Is.EqualTo(5));
        }

        [Test]
        public void GetAll_ShouldReturnAllReservations()
        {
            var reservation1 = new Reservation(1, 1001, DateTime.Now.AddDays(1));
            var reservation2 = new Reservation(2, 1002, DateTime.Now.AddDays(2));

            var allReservations = Reservation.GetAll();

            Assert.That(allReservations.Count, Is.EqualTo(2));
            Assert.Contains(reservation1, (System.Collections.ICollection)allReservations);
            Assert.Contains(reservation2, (System.Collections.ICollection)allReservations);
        }

        [Test]
        public void SetAll_ShouldUpdateReservationsListCorrectly()
        {
            var reservation1 = new Reservation(1, 1001, DateTime.Now.AddDays(1));

            var newReservations = new List<Reservation> { reservation1 };
            Reservation.SetAll(newReservations);

            var allReservations = Reservation.GetAll();
            Assert.That(allReservations.Count, Is.EqualTo(1));
            Assert.Contains(reservation1, (System.Collections.ICollection)allReservations);
            Assert.That(Reservation.TotalReservations, Is.EqualTo(1));
        }

        [Test]
        public void TotalReservations_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Reservation.TotalReservations = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalReservations cannot be negative."));
        }
    }
}
