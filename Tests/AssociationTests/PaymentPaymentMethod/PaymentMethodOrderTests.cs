using BYT_Assignment_3.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class PaymentMethodOrderTests
    {
        private PaymentMethod paymentMethod;
        private Payment payment;
        private Order order;
        private Table table;

        [SetUp]
        public void SetUp()
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
            order.AddPayment(payment);

            // Assert
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments,
                "Payment should be in PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, payment.PaymentMethod, "Payment's PaymentMethod should be correctly set.");
            Assert.Contains(payment, (System.Collections.ICollection)order.Payments,
                "Payment should be in Order's Payments list.");
            Assert.AreEqual(order, payment.Order, "Payment's Order should be correctly set.");
        }

        [Test]
        public void AddPayment_ToOrder_AlreadyAssigned_ShouldThrowException()
        {
            // Arrange
            payment.SetPaymentMethod(paymentMethod);
            order.AddPayment(payment);

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.AddPayment(payment),
                "Should not allow a Payment to be associated with multiple Orders.");
            Assert.That(ex.Message, Is.EqualTo("Payment already exists in the order."),
                "Exception message should indicate the Payment already exists in the order.");
        }

        [Test]
        public void AddPayment_ToOrder_PaymentMethodNotSet_ShouldThrowException()
        {
            // Arrange
            // PaymentMethod is not set

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.AddPayment(payment),
                "Should not allow adding a Payment without a PaymentMethod.");
            Assert.That(ex.Message, Is.EqualTo("PaymentMethod must be set before adding to Order."),
                "Exception message should indicate the PaymentMethod must be set before adding to Order.");
        }


        [Test]
        public void AddPayment_ToOrder_NullPayment_ShouldThrowArgumentNullException()
        {
            // Arrange
            // No setup needed

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => order.AddPayment(null),
                "Should throw ArgumentNullException when adding null Payment.");
            Assert.That(ex.ParamName, Is.EqualTo("payment"), "Exception should reference the correct parameter.");
        }

        [Test]
        public void RemovePayment_NotInOrder_ShouldThrowArgumentException()
        {
            // Arrange
            var paymentMethod2 = new PaymentMethod(302, "Debit Card", "MasterCard, Visa");
            var payment2 = new Payment(402, 100.0, DateTime.Now.AddMinutes(-20), "TXN654321");
            payment2.SetPaymentMethod(paymentMethod2);
            // Payment2 is not added to order

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => order.RemovePayment(payment2),
                "Should throw ArgumentException when removing Payment not in Order.");
            Assert.That(ex.Message, Is.EqualTo("Payment not found in the order."),
                "Exception message should be as expected.");
        }

        [Test]
        public void UpdatePayment_InOrder_ReplacesPaymentCorrectly()
        {
            // Arrange
            payment.SetPaymentMethod(paymentMethod);
            order.AddPayment(payment);
            var oldPayment = payment;
            var newPayment = new Payment(402, 75.0, DateTime.Now.AddMinutes(-30), "TXN789012");
            newPayment.SetPaymentMethod(paymentMethod);

            // Act
            order.UpdatePayment(oldPayment, newPayment);

            // Assert
            Assert.IsFalse(order.Payments.Contains(oldPayment),
                "Old Payment should be removed from Order's Payments.");
            Assert.Contains(newPayment, (System.Collections.ICollection)order.Payments,
                "New Payment should be added to Order's Payments.");
            Assert.IsFalse(paymentMethod.Payments.Contains(oldPayment),
                "Old Payment should be removed from PaymentMethod's Payments.");
            Assert.Contains(newPayment, (System.Collections.ICollection)paymentMethod.Payments,
                "New Payment should be added to PaymentMethod's Payments.");
            Assert.AreEqual(order, newPayment.Order, "New Payment's Order should be set correctly.");
            Assert.AreEqual(paymentMethod, newPayment.PaymentMethod, "New Payment's PaymentMethod should be set correctly.");
        }
    }
}
