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
            // Initialize a PaymentMethod
            paymentMethod = new PaymentMethod(paymentMethodID: 301, methodName: "Credit Card",
                description: "Visa, MasterCard");

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
            // Clean up class extents if necessary
            PaymentMethod.SetAll(new List<PaymentMethod>());
            Payment.SetAll(new List<Payment>());
        }

        [Test]
        public void AddPayment_AssociatesBothSides()
        {
            // Act
            paymentMethod.AddPayment(payment);

            // Assert
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments);
            Assert.AreEqual(paymentMethod, payment.PaymentMethod);
        }

        [Test]
        public void RemovePayment_DissociatesBothSides()
        {
            // Arrange
            paymentMethod.AddPayment(payment);
            Assert.Contains(payment, (System.Collections.ICollection)paymentMethod.Payments);
            Assert.AreEqual(paymentMethod, payment.PaymentMethod);

            // Act
            paymentMethod.RemovePayment(payment);

            // Assert
            Assert.IsFalse(paymentMethod.Payments.Contains(payment));
            Assert.Throws<ArgumentException>(() =>
            {
                var method = payment.PaymentMethod;
            });
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
            Assert.IsFalse(paymentMethod.Payments.Contains(payment));
            Assert.IsTrue(paymentMethod.Payments.Contains(newPayment));
            Assert.AreEqual(paymentMethod, newPayment.PaymentMethod);
        }

        [Test]
        public void AddDuplicatePayment_ThrowsException()
        {
            // Arrange
            paymentMethod.AddPayment(payment);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => paymentMethod.AddPayment(payment));
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
            Assert.Throws<ArgumentException>(() => paymentMethod.RemovePayment(nonExistentPayment));
        }
    }
}