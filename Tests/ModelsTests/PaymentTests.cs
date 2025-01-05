using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class PaymentTests
    {
        private Table table;
        private Order order;

        [SetUp]
        public void SetUp()
        {
            // Reset class extents before each test to ensure test isolation
            PaymentMethod.SetAll(new List<PaymentMethod>());
            Payment.SetAll(new List<Payment>());
            Order.SetAll(new List<Order>());
            Table.SetAll(new List<Table>());
        }

        [Test]
        public void Payment_CreatesObjectCorrectly()
        {
            // Arrange
            table = new Table(1, 4, "Window", "Booth");
            order = new Order(
                orderID: 123,
                orderDate: DateTime.Now.AddHours(-1),
                table: table,
                notes: "No onions",
                discountCode: "DISC10ABCD"
            );

            // Act
            var payment = new Payment(
                paymentID: 1,
                amount: 25.0,
                dateTime: DateTime.Now.AddMinutes(-30),
                transactionID: "TXN123456"
            );
            payment.SetPaymentMethod(new PaymentMethod(302, "Debit Card", "Visa, MasterCard"));
            order.AddPayment(payment);

            // Assert
            Assert.That(payment.PaymentID, Is.EqualTo(1));
            Assert.That(payment.Order, Is.EqualTo(order), "Payment's Order should be correctly set.");
            Assert.That(payment.Amount, Is.EqualTo(25.0));
            Assert.That(payment.IsSuccessful, Is.True, "Payment should be marked as successful.");
            Assert.That(Payment.GetAll(), Contains.Item(payment),
                "Payment should be present in the class extent.");
        }

        [Test]
        public void Payment_TotalPaymentsCannotBeNegative()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => Payment.TotalPayments = -1,
                "Setting TotalPayments to a negative value should throw an exception.");
            Assert.That(ex.Message, Is.EqualTo("TotalPayments cannot be negative."),
                "Exception message should be as expected.");
        }

        [Test]
        public void Payment_OrderMustBeSet()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var payment = new Payment(
                    paymentID: 2,
                    amount: 15.0,
                    dateTime: DateTime.Now,
                    transactionID: "TXN654321"
                );
                // Attempt to set Order as null by not adding to any Order
            }, "Creating a Payment without an Order should throw an exception.");

            Assert.That(ex.Message, Is.EqualTo("PaymentMethod cannot be null."),
                "Exception message should be as expected.");
        }

        [Test]
        public void Payment_IsCorrectlySavedInExtent()
        {
            // Arrange
            table = new Table(1, 4, "Window", "Booth");
            order = new Order(
                orderID: 124,
                orderDate: DateTime.Now.AddHours(-2),
                table: table,
                notes: "Extra cheese",
                discountCode: "DISC20EFGH"
            );
            var payment = new Payment(
                paymentID: 3,
                amount: 30.0,
                dateTime: DateTime.Now.AddMinutes(-45),
                transactionID: "TXN789012"
            );
            payment.SetPaymentMethod(new PaymentMethod(303, "Cash", "USD, EUR"));
            order.AddPayment(payment);

            // Act & Assert
            Assert.That(Payment.GetAll(), Contains.Item(payment),
                "Payment should be present in the class extent.");
        }

        [Test]
        public void Payment_DateTimeCannotBeInFuture()
        {
            // Arrange
            table = new Table(1, 4, "Window", "Booth");
            order = new Order(
                orderID: 125,
                orderDate: DateTime.Now.AddHours(-1),
                table: table,
                notes: "Gluten-free",
                discountCode: "DISC30IJKL"
            );

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var payment = new Payment(
                    paymentID: 4,
                    amount: 40.0,
                    dateTime: DateTime.Now.AddHours(1), // Future DateTime
                    transactionID: "TXN345678"
                );
                payment.SetPaymentMethod(new PaymentMethod(304, "Mobile Pay", "Apple Pay"));
                order.AddPayment(payment);
            }, "Creating a Payment with a future DateTime should throw an exception.");

            Assert.That(ex.Message, Is.EqualTo("DateTime cannot be in the future."),
                "Exception message should be as expected.");
        }

        [Test]
        public void Payment_TransactionID_LengthValidation()
        {
            // Arrange
            table = new Table(1, 4, "Window", "Booth");
            order = new Order(
                orderID: 126,
                orderDate: DateTime.Now.AddHours(-1),
                table: table,
                notes: "No salt",
                discountCode: "DISC40MNOP"
            );

            // Act & Assert for valid length
            var payment = new Payment(
                paymentID: 5,
                amount: 50.0,
                dateTime: DateTime.Now.AddMinutes(-15),
                transactionID: "TXN11223344556677889900" // <=50 chars
            );
            payment.SetPaymentMethod(new PaymentMethod(305, "Check", "Personal Checks"));
            order.AddPayment(payment);
            Assert.That(payment.TransactionID, Is.EqualTo("TXN11223344556677889900"),
                "TransactionID should be set correctly when within valid length.");

            // Act & Assert for invalid length
            var longTransactionID = new string('A', 51); // 51 characters
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var invalidPayment = new Payment(
                    paymentID: 6,
                    amount: 60.0,
                    dateTime: DateTime.Now.AddMinutes(-10),
                    transactionID: longTransactionID
                );
                invalidPayment.SetPaymentMethod(new PaymentMethod(306, "Gift Card", "Amazon, iTunes"));
                order.AddPayment(invalidPayment);
            }, "Creating a Payment with a TransactionID exceeding 50 characters should throw an exception.");

            Assert.That(ex.Message, Is.EqualTo("TransactionID length cannot exceed 50 characters."),
                "Exception message should be as expected.");
        }
    }
}
