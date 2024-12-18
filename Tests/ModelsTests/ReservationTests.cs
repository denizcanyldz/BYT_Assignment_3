using System;
using System.Collections.Generic;
using NUnit.Framework;
using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class ReservationTests
    {
        [SetUp]
        public void SetUp()
        {
            // Reset all class extents before each test to ensure test isolation
            Reservation.SetAll(new List<Reservation>());
            Table.SetAll(new List<Table>());
            Customer.SetAll(new List<Customer>());
        }

        [Test]
        public void Reservation_CreatesObjectCorrectly_WithMandatoryAttributes()
        {
            // Arrange
            var table = new Table(
                tableNumber: 1,
                maxSeats: 4,
                location: "Main Hall",
                seatingArrangement: "Round"
            );

            var customer = new Customer(
                customerID: 101,
                name: "John Doe"
            );

            var reservationDateTime = DateTime.Now.AddDays(1);

            var reservation = new Reservation(
                reservationID: 1,
                customer: customer,
                reservationDateTime: reservationDateTime,
                table: table,
                status: "Confirmed"
                // specialRequests is optional and defaults to null
            );

            // Act & Assert
            Assert.AreEqual(1, reservation.ReservationID, "ReservationID should be set correctly.");
            Assert.AreEqual(customer, reservation.Customer, "Customer should be associated correctly.");
            Assert.AreEqual(reservationDateTime.Date, reservation.ReservationDateTime.Date, "ReservationDateTime should be set correctly.");
            Assert.AreEqual(table, reservation.Table, "Table should be associated correctly.");
            Assert.AreEqual("Confirmed", reservation.Status, "Status should be set correctly.");
            Assert.IsNull(reservation.SpecialRequests, "SpecialRequests should be null when not provided.");
            Assert.AreEqual(1, Reservation.TotalReservations, "TotalReservations should be incremented.");
            Assert.Contains(reservation, (System.Collections.ICollection)Reservation.GetAll(), "Reservation should be added to the class extent.");
        }

        [Test]
        public void Reservation_TotalReservationsCannotBeNegative()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Reservation.TotalReservations = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalReservations cannot be negative."), "Exception message should indicate invalid TotalReservations.");
        }

        [Test]
        public void Reservation_CustomerMustNotBeNull()
        {
            // Arrange
            var table = new Table(
                tableNumber: 2,
                maxSeats: 6,
                location: "VIP Section",
                seatingArrangement: "Square"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new Reservation(
                reservationID: 2,
                customer: null!,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Pending"
            ));
            Assert.That(ex.ParamName, Is.EqualTo("customer"), "Exception parameter name should indicate 'customer'.");
            Assert.That(ex.Message, Does.Contain("Customer cannot be null."));
        }

        [Test]
        public void Reservation_ReservationDateTimeCannotBeInPast()
        {
            // Arrange
            var table = new Table(
                tableNumber: 3,
                maxSeats: 2,
                location: "Outdoor",
                seatingArrangement: "Small Round"
            );

            var customer = new Customer(
                customerID: 102,
                name: "Jane Smith"
            );

            var pastDateTime = DateTime.Now.AddDays(-1);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(
                reservationID: 3,
                customer: customer,
                reservationDateTime: pastDateTime,
                table: table,
                status: "Confirmed"
            ));
            Assert.That(ex.Message, Is.EqualTo("ReservationDateTime cannot be in the past."), "Exception message should indicate invalid ReservationDateTime.");
        }

        [Test]
        public void Reservation_StatusCannotBeNullOrEmpty()
        {
            // Arrange
            var table = new Table(
                tableNumber: 4,
                maxSeats: 4,
                location: "Patio",
                seatingArrangement: "Rectangle"
            );

            var customer = new Customer(
                customerID: 103,
                name: "Alice Johnson"
            );

            // Act & Assert
            var exNull = Assert.Throws<ArgumentException>(() => new Reservation(
                reservationID: 4,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: null!
            ));
            Assert.That(exNull.Message, Is.EqualTo("Status cannot be null or empty."), "Exception message should indicate null Status.");

            var exEmpty = Assert.Throws<ArgumentException>(() => new Reservation(
                reservationID: 5,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: ""
            ));
            Assert.That(exEmpty.Message, Is.EqualTo("Status cannot be null or empty."), "Exception message should indicate empty Status.");
        }

        [Test]
        public void Reservation_IsCorrectlySavedInExtent()
        {
            // Arrange
            var table = new Table(
                tableNumber: 5,
                maxSeats: 6,
                location: "Garden",
                seatingArrangement: "Hexagon"
            );

            var customer = new Customer(
                customerID: 104,
                name: "Bob Brown"
            );

            var reservation = new Reservation(
                reservationID: 5,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Confirmed"
            );

            // Act & Assert
            Assert.Contains(reservation, (System.Collections.ICollection)Reservation.GetAll(), "Reservation should be added to the class extent.");
            Assert.AreEqual(1, Reservation.GetAll().Count, "Reservation extent should contain exactly one reservation.");
        }

        [Test]
        public void Reservation_AddOrderItemAddsCorrectly()
        {
            // Arrange
            var table = new Table(
                tableNumber: 6,
                maxSeats: 6,
                location: "Rooftop",
                seatingArrangement: "Oval"
            );

            var customer = new Customer(
                customerID: 105,
                name: "Charlie Davis"
            );

            var reservation = new Reservation(
                reservationID: 6,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Confirmed"
            );

            var orderItem = new OrderItem(
                orderItemID: 1,
                itemName: "Burger",
                quantity: 2,
                price: 10.0,
                specialInstructions: "No onions"
            );

            // Act
            reservation.AddOrderItem(orderItem);

            // Assert
            Assert.Contains(orderItem, (System.Collections.ICollection)reservation.OrderItems, "OrderItem should be added to the Reservation.");
            Assert.AreEqual(1, reservation.OrderItems.Count, "Reservation should have one OrderItem.");
        }

        [Test]
        public void Reservation_RemoveOrderItemRemovesCorrectly()
        {
            // Arrange
            var table = new Table(
                tableNumber: 7,
                maxSeats: 8,
                location: "Terrace",
                seatingArrangement: "Circle"
            );

            var customer = new Customer(
                customerID: 106,
                name: "Diana Evans"
            );

            var reservation = new Reservation(
                reservationID: 7,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Confirmed"
            );

            var orderItem = new OrderItem(
                orderItemID: 2,
                itemName: "Salad",
                quantity: 1,
                price: 8.0,
                specialInstructions: "Dressing on the side"
            );

            reservation.AddOrderItem(orderItem);

            // Act
            reservation.RemoveOrderItem(orderItem);

            // Assert
            Assert.IsFalse(reservation.OrderItems.Contains(orderItem), "OrderItem should be removed from the Reservation.");
            Assert.AreEqual(0, reservation.OrderItems.Count, "Reservation should have no OrderItems after removal.");
        }

        [Test]
        public void Reservation_NumberOfGuestsCalculatesCorrectly()
        {
            // Arrange
            var table = new Table(
                tableNumber: 8,
                maxSeats: 10,
                location: "Banquet Hall",
                seatingArrangement: "Long Table"
            );

            var customer = new Customer(
                customerID: 107,
                name: "Ethan Foster"
            );

            var reservation = new Reservation(
                reservationID: 8,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Confirmed"
            );

            var orderItem1 = new OrderItem(
                orderItemID: 3,
                itemName: "Pizza",
                quantity: 2,
                price: 5.0
            );

            var orderItem2 = new OrderItem(
                orderItemID: 4,
                itemName: "Pasta",
                quantity: 3,
                price: 10.0
            );

            // Act
            reservation.AddOrderItem(orderItem1);
            reservation.AddOrderItem(orderItem2);

            // Assert
            Assert.AreEqual(5, reservation.NumberOfGuests, "NumberOfGuests should be calculated correctly.");
        }

        [Test]
        public void Reservation_Status_ShouldThrowException_WhenNull()
        {
            // Arrange
            var table = new Table(
                tableNumber: 9,
                maxSeats: 4,
                location: "Corner",
                seatingArrangement: "Square"
            );

            var customer = new Customer(
                customerID: 108,
                name: "Fiona Green"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(
                reservationID: 9,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: null!
            ));
            Assert.That(ex.Message, Is.EqualTo("Status cannot be null or empty."), "Exception message should indicate null Status.");
        }

        [Test]
        public void Reservation_Table_ShouldThrowException_WhenNull()
        {
            // Arrange
            var customer = new Customer(
                customerID: 109,
                name: "George Harris"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new Reservation(
                reservationID: 10,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: null!,
                status: "Reserved"
            ));
            Assert.That(ex.Message, Is.EqualTo("Table cannot be null."), "Exception message should indicate null Table.");
        }

        [Test]
        public void Reservation_AddOrderItem_Null_ShouldThrowException()
        {
            // Arrange
            var table = new Table(
                tableNumber: 10,
                maxSeats: 4,
                location: "Window",
                seatingArrangement: "Square"
            );

            var customer = new Customer(
                customerID: 110,
                name: "Hannah Irwin"
            );

            var reservation = new Reservation(
                reservationID: 11,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Confirmed"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => reservation.AddOrderItem(null!));
            Assert.That(ex.Message, Is.EqualTo("OrderItem cannot be null."), "Exception message should indicate null OrderItem.");
        }

        [Test]
        public void Reservation_RemoveNonExistentOrderItem_ShouldThrowException()
        {
            // Arrange
            var table = new Table(
                tableNumber: 11,
                maxSeats: 8,
                location: "Terrace",
                seatingArrangement: "Circle"
            );

            var customer = new Customer(
                customerID: 111,
                name: "Ian Jackson"
            );

            var reservation = new Reservation(
                reservationID: 12,
                customer: customer,
                reservationDateTime: DateTime.Now.AddDays(1),
                table: table,
                status: "Confirmed"
            );

            var orderItem = new OrderItem(
                orderItemID: 5,
                itemName: "Soup",
                quantity: 1,
                price: 3.99,
                specialInstructions: null
            ); // Not added to the reservation

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => reservation.RemoveOrderItem(orderItem));
            Assert.That(ex.Message, Is.EqualTo("OrderItem not found."), "Exception message should indicate non-existent OrderItem.");
        }

        [Test]
        public void Reservation_EqualsAndHashCode_ShouldFunctionCorrectly()
        {
            // Arrange
            var table1 = new Table(
                tableNumber: 12,
                maxSeats: 2,
                location: "L12",
                seatingArrangement: "Round"
            );

            var table2 = new Table(
                tableNumber: 13,
                maxSeats: 3,
                location: "M13",
                seatingArrangement: "Square"
            );

            var customer1 = new Customer(
                customerID: 112,
                name: "Jack King"
            );

            var customer2 = new Customer(
                customerID: 113,
                name: "Karen Lee"
            );

            var reservationDateTime1 = new DateTime(2023, 10, 10, 12, 0, 0);
            var reservationDateTime2 = new DateTime(2023, 10, 11, 13, 30, 0);

            var reservation1 = new Reservation(
                reservationID: 13,
                customer: customer1,
                reservationDateTime: reservationDateTime1,
                table: table1,
                status: "Fast delivery",
                specialRequests: "Extra napkins"
            );

            var reservation2 = new Reservation(
                reservationID: 13,
                customer: customer1,
                reservationDateTime: reservationDateTime1,
                table: table1,
                status: "Fast delivery",
                specialRequests: "Extra napkins"
            );

            var reservation3 = new Reservation(
                reservationID: 14,
                customer: customer2,
                reservationDateTime: reservationDateTime2,
                table: table2,
                status: "Pending",
                specialRequests: null
            );

            // Act & Assert
            Assert.IsTrue(reservation1.Equals(reservation2), "Reservations with identical properties should be equal.");
            Assert.AreEqual(reservation1.GetHashCode(), reservation2.GetHashCode(), "HashCodes of identical reservations should be equal.");

            Assert.IsFalse(reservation1.Equals(reservation3), "Reservations with different properties should not be equal.");
            Assert.AreNotEqual(reservation1.GetHashCode(), reservation3.GetHashCode(), "HashCodes of different reservations should not be equal.");
        }

        [Test]
        public void Reservation_CreatesObjectWithoutOptionalAttributes()
        {
            // Arrange
            var table = new Table(
                tableNumber: 14,
                maxSeats: 5,
                location: "N14",
                seatingArrangement: "Pentagon"
            );

            var customer = new Customer(
                customerID: 114,
                name: "Laura Martin"
            );

            var reservationDateTime = DateTime.Now.AddDays(1);

            var reservation = new Reservation(
                reservationID: 15,
                customer: customer,
                reservationDateTime: reservationDateTime,
                table: table,
                status: "Pending"
                // specialRequests is optional and defaults to null
            );

            // Act & Assert
            Assert.IsNull(reservation.SpecialRequests, "SpecialRequests should be null when not provided.");
            Assert.AreEqual(1, Reservation.TotalReservations, "TotalReservations should be incremented.");
            Assert.Contains(reservation, (System.Collections.ICollection)Reservation.GetAll(), "Reservation should be added to the class extent.");
        }
    }
}
