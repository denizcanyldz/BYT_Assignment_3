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
        public int PaymentMethodID { get; private set; }

        private string methodName;

        public string MethodName
        {
            get => methodName;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
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
                if(!string.IsNullOrEmpty(value) && value.Length > 200)
                    throw new ArgumentException("Description length cannot exceed 200 characters.");
                description = value;
            }
        }

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
        
        /// <summary>
        /// Determines whether the specified object is equal to the current PaymentMethod.
        /// </summary>
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

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(PaymentMethodID, MethodName, Description);
        }
    }
}