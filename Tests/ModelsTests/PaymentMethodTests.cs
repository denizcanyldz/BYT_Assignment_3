using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class PaymentMethodTests
    {
        private PaymentMethod paymentMethod;
        private Payment payment;
        private Order order;
        private Table table;

        [SetUp]
        public void Setup()
        {
            // Initialize a Table
            table = new Table(1, 4, "Window", "Booth");

            // Initialize an Order
            order = new Order(
                orderID: 501,
                orderDate: DateTime.Now.AddHours(-2),
                table: table,
                notes: "No onions",
                discountCode: "DISC10ABCD"
            );

            // Initialize a PaymentMethod
            paymentMethod = new PaymentMethod(301, "Credit Card", "Visa, MasterCard");

            // Initialize a Payment without associating with Order
            payment = new Payment(
                paymentID: 401,
                amount: 150.00,
                dateTime: DateTime.Now.AddHours(-1),
                transactionID: "TXN123456789"
            );
        }

        [TearDown]
        public void Teardown()
        {
            // Reset class extents to ensure test isolation
            PaymentMethod.SetAll(new List<PaymentMethod>());
            Payment.SetAll(new List<Payment>());
            Order.SetAll(new List<Order>());
            Table.SetAll(new List<Table>());
        }

        [Test]
        public void AddPayment_AssociatesBothSides()
        {
            // Arrange
            payment.SetPaymentMethod(paymentMethod);

            // Act
            paymentMethod.AddPayment(payment);
            order.AddPayment(payment); // Ensure it's associated with the order as well

            // Assert
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments,
                "Payment should be in PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, payment.PaymentMethod, "Payment's PaymentMethod should be correctly set.");
            Assert.Contains(payment, (System.Collections.ICollection)order.Payments,
                "Payment should be in Order's Payments list.");
            Assert.AreEqual(order, payment.Order, "Payment's Order should be correctly set.");
        }

        [Test]
        public void RemovePayment_DissociatesBothSides()
        {
            // Arrange
            payment.SetPaymentMethod(paymentMethod);
            paymentMethod.AddPayment(payment);
            order.AddPayment(payment);
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments,
                "Payment should be in PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, payment.PaymentMethod, "Payment's PaymentMethod should be correctly set.");
            Assert.Contains(payment, (System.Collections.ICollection)order.Payments,
                "Payment should be in Order's Payments list.");
            Assert.AreEqual(order, payment.Order, "Payment's Order should be correctly set.");

            // Act
            paymentMethod.RemovePayment(payment);

            // Assert
            Assert.IsFalse(paymentMethod.Payments.Contains(payment),
                "Payment should be removed from PaymentMethod's Payments list.");
            Assert.IsFalse(order.Payments.Contains(payment),
                "Payment should be removed from Order's Payments list.");
            Assert.IsNull(payment.PaymentMethod, "Payment's PaymentMethod should be unset.");
            Assert.IsNull(payment.Order, "Payment's Order should be unset.");
        }

        [Test]
        public void UpdatePayment_ReplacesOldWithNew()
        {
            // Arrange
            payment.SetPaymentMethod(paymentMethod);
            paymentMethod.AddPayment(payment);
            order.AddPayment(payment);
            var newPayment = new Payment(
                paymentID: 402,
                amount: 200.00,
                dateTime: DateTime.Now.AddMinutes(10),
                transactionID: "TXN987654321"
            );
            newPayment.SetPaymentMethod(paymentMethod);

            // Act
            paymentMethod.UpdatePayment(payment, newPayment);

            // Assert
            Assert.IsFalse(paymentMethod.Payments.Contains(payment),
                "Old Payment should be removed from PaymentMethod's Payments list.");
            Assert.IsTrue(paymentMethod.Payments.Contains(newPayment),
                "New Payment should be added to PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, newPayment.PaymentMethod,
                "New Payment's PaymentMethod should be correctly set.");
            Assert.Contains(newPayment, (System.Collections.ICollection)order.Payments,
                "New Payment should be in Order's Payments list.");
            Assert.AreEqual(order, newPayment.Order, "New Payment's Order should be correctly set.");
        }

        [Test]
        public void AddDuplicatePayment_ThrowsException()
        {
            // Arrange
            payment.SetPaymentMethod(paymentMethod);
            paymentMethod.AddPayment(payment);
            order.AddPayment(payment);

            // Act & Assert
            var duplicatePayment = payment;
            var ex = Assert.Throws<ArgumentException>(() => paymentMethod.AddPayment(duplicatePayment),
                "Adding a duplicate Payment should throw an exception.");
            Assert.That(ex.Message, Is.EqualTo("Payment already exists in the order."),
                "Exception message should indicate the Payment already exists in the order.");
        }

        [Test]
        public void RemoveNonExistentPayment_ThrowsException()
        {
            // Arrange
            var nonExistentPayment = new Payment(
                paymentID: 403,
                amount: 250.00,
                dateTime: DateTime.Now.AddMinutes(20),
                transactionID: "TXN1122334455"
            );
            nonExistentPayment.SetPaymentMethod(paymentMethod);
            // Payment is not added to PaymentMethod or Order

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => paymentMethod.RemovePayment(nonExistentPayment),
                "Removing a non-existent Payment should throw an exception.");
            Assert.That(ex.Message, Is.EqualTo("Payment not found in the PaymentMethod."),
                "Exception message should be as expected.");
        }
    }
}
