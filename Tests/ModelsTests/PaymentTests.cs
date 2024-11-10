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
        }

        [Test]
        public void Payment_CreatesObjectCorrectly()
        {
            var payment = new Payment(1, 123, 25.0, DateTime.Now);
            Assert.That(payment.PaymentID, Is.EqualTo(1));
            Assert.That(payment.OrderID, Is.EqualTo(123));
            Assert.That(payment.Amount, Is.EqualTo(25.0));
        }

        [Test]
        public void Payment_TotalPaymentsCannotBeNegative()
        {
            Assert.Throws<ArgumentException>(() => Payment.TotalPayments = -1);
        }

        [Test]
        public void Payment_OrderIDMustBePositive()
        {
            Assert.Throws<ArgumentException>(() => new Payment(2, -1, 15.0, DateTime.Now));
        }

        [Test]
        public void Payment_IsCorrectlySavedInExtent()
        {
            var payment = new Payment(3, 124, 30.0, DateTime.Now);
            Assert.That(Payment.GetAll(), Contains.Item(payment));
        }
    }
}