using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class PaymentMethod
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalPaymentMethods = 0;

        /// <summary>
        /// Gets or sets the total number of payment methods.
        /// </summary>
        public static int TotalPaymentMethods
        {
            get => totalPaymentMethods;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalPaymentMethods cannot be negative.");
                totalPaymentMethods = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

        /// <summary>
        /// Gets a read-only list of all payment methods.
        /// </summary>
        public static IReadOnlyList<PaymentMethod> GetAll()
        {
            return paymentMethods.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire payment method list (used during deserialization).
        /// </summary>
        public static void SetAll(List<PaymentMethod> loadedPaymentMethods)
        {
            paymentMethods = loadedPaymentMethods ?? new List<PaymentMethod>();
            TotalPaymentMethods = paymentMethods.Count;
        }

       
        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int PaymentMethodID { get; set; }

        private string methodName;

        public string MethodName
        {
            get => methodName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("MethodName cannot be null or empty.");
                methodName = value;
            }
        }


        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? description;

        public string? Description
        {
            get => description;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 200)
                    throw new ArgumentException("Description length cannot exceed 200 characters.");
                description = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<Payment> payments = new List<Payment>();

        [XmlIgnore]
        public IReadOnlyList<Payment> Payments => payments.AsReadOnly();

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the PaymentMethod class with mandatory and optional attributes.
        /// </summary>
        public PaymentMethod(int paymentMethodID, string methodName, string? description = null)
        {
            PaymentMethodID = paymentMethodID;
            MethodName = methodName;
            Description = description;

            // Add to class extent
            paymentMethods.Add(this);
            TotalPaymentMethods = paymentMethods.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public PaymentMethod() { }
        
       // -------------------------------
        // Association Methods
        // -------------------------------
        public void AddPayment(Payment payment)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");
            if (!payments.Contains(payment))
            {
                payments.Add(payment);
                payment.SetPaymentMethod(this);
            }
        }

        public void RemovePayment(Payment payment)
        {
            if (payment == null || !payments.Contains(payment))
                throw new ArgumentException("Payment not found in the PaymentMethod.");
            payments.Remove(payment);
            payment.RemovePaymentMethod();
        }

        public void UpdatePayment(Payment oldPayment, Payment newPayment)
        {
            if (oldPayment == null || newPayment == null)
                throw new ArgumentNullException("Payment cannot be null.");
            if (!payments.Contains(oldPayment))
                throw new ArgumentException("Old Payment not found in the PaymentMethod.");
            if (payments.Contains(newPayment))
                throw new ArgumentException("New Payment already exists in the PaymentMethod.");

            // Remove old payment
            RemovePayment(oldPayment);

            // Add new payment
            AddPayment(newPayment);
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is PaymentMethod other)
            {
                return PaymentMethodID == other.PaymentMethodID &&
                       MethodName == other.MethodName &&
                       Description == other.Description;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PaymentMethodID, MethodName, Description);
        }
    }
}