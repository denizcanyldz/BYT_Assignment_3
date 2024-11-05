using BYT_Assignment_3.Models;

namespace Tests.ModelsTests
{
    [TestFixture]
    public class PaymentMethodTests
    {
        [SetUp]
        public void SetUp()
        {
            
            PaymentMethod.SetAll(new List<PaymentMethod>());
            PaymentMethod.TotalPaymentMethods = 0;
        }

        [Test]
        public void Constructor_ShouldInitializePaymentMethodCorrectly()
        {
            var paymentMethod = new PaymentMethod(1, "Credit Card", "Widely accepted");

            Assert.That(paymentMethod.PaymentMethodID, Is.EqualTo(1));
            Assert.That(paymentMethod.MethodName, Is.EqualTo("Credit Card"));
            Assert.That(paymentMethod.Description, Is.EqualTo("Widely accepted"));
            Assert.That(PaymentMethod.TotalPaymentMethods, Is.EqualTo(1));
        }

        [Test]
        public void MethodName_ShouldThrowException_WhenNullOrEmpty()
        {
            var ex1 = Assert.Throws<ArgumentException>(() => new PaymentMethod(1, "", "No method"));
            Assert.That(ex1.Message, Is.EqualTo("MethodName cannot be null or empty."));

            var ex2 = Assert.Throws<ArgumentException>(() => new PaymentMethod(2, null, "No method"));
            Assert.That(ex2.Message, Is.EqualTo("MethodName cannot be null or empty."));
        }

        [Test]
        public void Description_ShouldThrowException_WhenExceedsMaxLength()
        {
            var longDescription = new string('a', 201);
            var ex = Assert.Throws<ArgumentException>(() => new PaymentMethod(1, "Bank Transfer", longDescription));
            Assert.That(ex.Message, Is.EqualTo("Description length cannot exceed 200 characters."));
        }

        [Test]
        public void GetAll_ShouldReturnAllPaymentMethods()
        {
            var method1 = new PaymentMethod(1, "Credit Card");
            var method2 = new PaymentMethod(2, "PayPal");

            var allMethods = PaymentMethod.GetAll();
            Assert.That(allMethods.Count, Is.EqualTo(2));
            Assert.Contains(method1, (System.Collections.ICollection)allMethods);
            Assert.Contains(method2, (System.Collections.ICollection)allMethods);
        }

        [Test]
        public void SetAll_ShouldUpdatePaymentMethodsListCorrectly()
        {
            var method1 = new PaymentMethod(1, "Credit Card");

            var newMethods = new List<PaymentMethod> { method1 };
            PaymentMethod.SetAll(newMethods);

            var allMethods = PaymentMethod.GetAll();
            Assert.That(allMethods.Count, Is.EqualTo(1));
            Assert.Contains(method1, (System.Collections.ICollection)allMethods);
            Assert.That(PaymentMethod.TotalPaymentMethods, Is.EqualTo(1));
        }

        [Test]
        public void TotalPaymentMethods_ShouldThrowException_WhenSetToNegative()
        {
            var ex = Assert.Throws<ArgumentException>(() => PaymentMethod.TotalPaymentMethods = -1);
            Assert.That(ex.Message, Is.EqualTo("TotalPaymentMethods cannot be negative."));
        }
    }
}
