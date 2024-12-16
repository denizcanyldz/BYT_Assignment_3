using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class OneToManyTests
    {
        [SetUp]
        public void SetUp()
        {
            // Resetting all customers and reservations before each test.
            Customer.SetAll(new List<Customer>());
            Reservation.SetAll(new List<Reservation>());
        }

        [Test]
        public void AddReservation_AssociationIsLinkedBothWays()
        {
            var customer = new Customer(1, "John Doe", "john@example.com", "1234567890");
            var table =new Table(1, 4, "Main Hall", "Round");
            
            // Creating a reservation and assigning it to the customer
            var reservation = new Reservation(100, customer, DateTime.Now.AddDays(1), table, "Confirmed");

            Assert.That(customer.GetReservations(), Contains.Item(reservation), 
                "Customer should contain the created reservation.");
            Assert.That(reservation.Customer, Is.EqualTo(customer), 
                "Reservation should reference the correct customer.");
        }

        [Test]
        public void RemoveReservation_AssociationIsUnlinkedBothWays()
        {
            var customer = new Customer(2, "Jane Doe", "jane@example.com");
            var table = new Table(1, 4, "Main Hall", "Round");
            var reservation = new Reservation(200, customer, DateTime.Now.AddDays(1), table, "Confirmed");

            // Remove the reservation from the customer
            customer.RemoveReservation(reservation);

            Assert.That(customer.GetReservations(), Does.Not.Contain(reservation),
                "Reservation should no longer be in customer's list.");
            Assert.That(reservation.Customer, Is.Null,
                "Reservation's Customer reference should be null after removal.");
        }

        [Test]
        public void SetCustomer_UpdatesAssociationProperly()
        {
            var customerA = new Customer(3, "Alice", "alice@example.com");
            var customerB = new Customer(4, "Bob", "bob@example.com");
            var table = new Table(1, 4, "Main Hall", "Round");
            var reservation = new Reservation(300, customerA, DateTime.Now.AddDays(5), table, "Pending");

            // Reassign reservation to a different customer using SetCustomer
            reservation.SetCustomer(customerB);

            Assert.That(customerA.GetReservations(), Does.Not.Contain(reservation),
                "Customer A should no longer have the reservation.");
            Assert.That(customerB.GetReservations(), Contains.Item(reservation),
                "Customer B should now have the reservation.");
            Assert.That(reservation.Customer, Is.EqualTo(customerB),
                "Reservation should now reference Customer B.");
        }

        [Test]
        public void UpdateReservationCustomer_ReassignsReservationToNewCustomer()
        {
            var customerA = new Customer(5, "Charlie", "charlie@example.com");
            var customerB = new Customer(6, "Diana", "diana@example.com");
            var table = new Table(1, 4, "Main Hall", "Round");

            var reservation = new Reservation(400, customerA, DateTime.Now.AddDays(2), table, "Confirmed");
            
            // Move reservation from Customer A to Customer B
            customerA.UpdateReservationCustomer(reservation, customerB);

            Assert.That(customerA.GetReservations(), Does.Not.Contain(reservation),
                "Customer A should no longer have the reservation after update.");
            Assert.That(customerB.GetReservations(), Contains.Item(reservation),
                "Customer B should now have the reservation after update.");
            Assert.That(reservation.Customer, Is.EqualTo(customerB),
                "Reservation should link to Customer B.");
        }

        [Test]
        public void AddReservation_NullShouldThrowException()
        {
            var customer = new Customer(7, "Eve", "eve@example.com");
            Assert.Throws<ArgumentException>(() => customer.AddReservation(null),
                "Adding a null reservation should throw an exception.");
        }

        [Test]
        public void RemoveNonexistentReservation_ShouldThrowException()
        {
            var customer = new Customer(8, "Frank", "frank@example.com");
            var table = new Table(1, 4, "Main Hall", "Round");
            var reservation = new Reservation(500, customer, DateTime.Now.AddDays(1), table, "Pending");

            // Remove it properly first
            customer.RemoveReservation(reservation);

            // Attempt to remove again (now nonexistent)
            Assert.Throws<ArgumentException>(() => customer.RemoveReservation(reservation),
                "Removing a non-existent reservation should throw an exception.");
        }

        [Test]
        public void NumberOfReservationsIsUpdatedCorrectly()
        {
            var customer = new Customer(9, "George", "george@example.com");
            var table = new Table(1, 4, "Main Hall", "Round");

            var reservation1 = new Reservation(600, customer, DateTime.Now.AddDays(1), table, "Pending");
            var reservation2 = new Reservation(601, customer, DateTime.Now.AddDays(2), table, "Pending");

            Assert.That(customer.TotalReservations, Is.EqualTo(2),
                "TotalReservations should reflect the number of linked reservations.");
        }
    }
}
