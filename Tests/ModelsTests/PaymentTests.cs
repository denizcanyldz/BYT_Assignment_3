using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class PaymentTests
    {
        [SetUp]
        public void SetUp()
        {

            Payment.SetAll(new List<Payment>());
            Payment.TotalPayments = 0;
        }

        [Test]
        public void Constructor_ShouldInitializePaymentCorrectly()
        {
            var payment = new Payment(1, 101, 250.0, "TXN12345");

            Assert.That(payment.PaymentID, Is.EqualTo(1));
            Assert.That(payment.OrderID, Is.EqualTo(101));
            Assert.That(payment.Amount, Is.EqualTo(250.0));
            Assert.That(payment.TransactionID, Is.EqualTo("TXN12345"));
            Assert.That(Payment.TotalPayments, Is.EqualTo(1));
        }

        [Test]
        public void OrderID_ShouldThrowException_WhenNegativeOrZero()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Payment(1, 0, 100.0));
            Assert.That(ex.Message, Is.EqualTo("OrderID must be positive."));
        }

        [Test]
        public void Amount_ShouldThrowException_WhenNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => new Payment(1, 101, -50.0));
            Assert.That(ex.Message, Is.EqualTo("Amount cannot be negative."));
        }

        [Test]
        public void TransactionID_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longTransactionID = new string('A', 51);
            var ex = Assert.Throws<ArgumentException>(() => new Payment(1, 101, 100.0, longTransactionID));
            Assert.That(ex.Message, Is.EqualTo("TransactionID length cannot exceed 50 characters."));
        }

        [Test]
        public void IsSuccessful_ShouldReturnTrue_WhenAmountIsPositive()
        {
            var payment = new Payment(1, 101, 100.0);
            Assert.IsTrue(payment.IsSuccessful);
        }

        [Test]
        public void IsSuccessful_ShouldReturnFalse_WhenAmountIsZero()
        {
            var payment = new Payment(1, 101, 0.0);
            Assert.IsFalse(payment.IsSuccessful);
        }

        [Test]
        public void GetAll_ShouldReturnAllPayments()
        {
            var payment1 = new Payment(1, 101, 100.0);
            var payment2 = new Payment(2, 102, 200.0);

            var allPayments = Payment.GetAll();
            Assert.That(allPayments.Count, Is.EqualTo(2));
            Assert.Contains(payment1, (System.Collections.ICollection)allPayments);
            Assert.Contains(payment2, (System.Collections.ICollection)allPayments);
        }

        [Test]
        public void SetAll_ShouldUpdatePaymentsListCorrectly()
        {
            var payment1 = new Payment(1, 101, 100.0);

            var newPayments = new List<Payment> { payment1 };
            Payment.SetAll(newPayments);

            var allPayments = Payment.GetAll();
            Assert.That(allPayments.Count, Is.EqualTo(1));
            Assert.Contains(payment1, (System.Collections.ICollection)allPayments);
            Assert.That(Payment.TotalPayments, Is.EqualTo(1));
        }

        [Test]
        public void TotalPayments_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => Payment.TotalPayments = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalPayments cannot be negative."));
        }
    }
}