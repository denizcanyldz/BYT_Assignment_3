using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class PaymentMethodTests
    {
        private PaymentMethod paymentMethod;
        private Payment payment;

        [SetUp]
        public void Setup()
        {
            // Initialize a PaymentMethod using positional arguments
            paymentMethod = new PaymentMethod(301, "Credit Card", "Visa, MasterCard");

            // Initialize a Payment
            payment = new Payment(
                paymentID: 401,
                orderID: 501,
                amount: 150.00,
                dateTime: DateTime.Now,
                transactionID: "TXN123456789"
            );
        }

        [TearDown]
        public void Teardown()
        {
            // Reset class extents to ensure test isolation
            PaymentMethod.SetAll(new List<PaymentMethod>());
            Payment.SetAll(new List<Payment>());
        }

        [Test]
        public void AddPayment_AssociatesBothSides()
        {
            // Act
            paymentMethod.AddPayment(payment);

            // Assert
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments,
                "Payment should be in PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, payment.PaymentMethod, "Payment's PaymentMethod should be correctly set.");
        }

        [Test]
        public void RemovePayment_DissociatesBothSides()
        {
            // Arrange
            paymentMethod.AddPayment(payment);
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments,
                "Payment should be in PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, payment.PaymentMethod, "Payment's PaymentMethod should be correctly set.");

            // Act
            paymentMethod.RemovePayment(payment);

            // Assert
            Assert.IsFalse(paymentMethod.Payments.Contains(payment),
                "Payment should be removed from PaymentMethod's Payments list.");
            Assert.Throws<ArgumentException>(() =>
            {
                var method = payment.PaymentMethod;
            }, "Accessing PaymentMethod after removal should throw an exception.");
        }

        [Test]
        public void UpdatePayment_ReplacesOldWithNew()
        {
            // Arrange
            paymentMethod.AddPayment(payment);
            var newPayment = new Payment(
                paymentID: 402,
                orderID: 502,
                amount: 200.00,
                dateTime: DateTime.Now.AddMinutes(10),
                transactionID: "TXN987654321"
            );

            // Act
            paymentMethod.UpdatePayment(payment, newPayment);

            // Assert
            Assert.IsFalse(paymentMethod.Payments.Contains(payment),
                "Old Payment should be removed from PaymentMethod's Payments list.");
            Assert.IsTrue(paymentMethod.Payments.Contains(newPayment),
                "New Payment should be added to PaymentMethod's Payments list.");
            Assert.AreEqual(paymentMethod, newPayment.PaymentMethod,
                "New Payment's PaymentMethod should be correctly set.");
        }

        [Test]
        public void AddDuplicatePayment_ThrowsException()
        {
            // Arrange
            paymentMethod.AddPayment(payment);

            // Act & Assert
            var duplicatePayment = payment;
            Assert.Throws<ArgumentException>(() => paymentMethod.AddPayment(duplicatePayment),
                "Adding a duplicate Payment should throw an exception.");
        }

        [Test]
        public void RemoveNonExistentPayment_ThrowsException()
        {
            // Arrange
            var nonExistentPayment = new Payment(
                paymentID: 403,
                orderID: 503,
                amount: 250.00,
                dateTime: DateTime.Now.AddMinutes(20),
                transactionID: "TXN1122334455"
            );

            // Act & Assert
            Assert.Throws<ArgumentException>(() => paymentMethod.RemovePayment(nonExistentPayment),
                "Removing a non-existent Payment should throw an exception.");
        }
    }
}