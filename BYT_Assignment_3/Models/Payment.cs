namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Payment
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalPayments = 0;

        /// <summary>
        /// Gets or sets the total number of payments.
        /// </summary>
        public static int TotalPayments
        {
            get => totalPayments;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalPayments cannot be negative.");
                totalPayments = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Payment> payments = new List<Payment>();

        /// <summary>
        /// Gets a read-only list of all payments.
        /// </summary>
        public static IReadOnlyList<Payment> GetAll()
        {
            return payments.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire payment list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Payment> loadedPayments)
        {
            payments = loadedPayments ?? new List<Payment>();
            TotalPayments = payments.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int PaymentID { get; set; }

        private int orderID;

        public int OrderID
        {
            get => orderID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("OrderID must be positive.");
                orderID = value;
            }
        }

        private double amount;

        public double Amount
        {
            get => amount;
            set
            {
                if(value < 0)
                    throw new ArgumentException("Amount cannot be negative.");
                amount = value;
            }
        }

        private DateTime dateTime;

        /// <summary>
        /// Gets or sets the date and time of the payment.
        /// </summary>
        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("DateTime cannot be in the future.");
                dateTime = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? transactionID;

        public string? TransactionID
        {
            get => transactionID;
            set
            {
                if(!string.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ArgumentException("TransactionID length cannot exceed 50 characters.");
                transactionID = value;
            }
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public bool IsSuccessful => Amount > 0;

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Payment class with mandatory and optional attributes.
        /// </summary>
        public Payment(int paymentID, int orderID, double amount, DateTime dateTime, string? transactionID = null)
        {
            PaymentID = paymentID;
            OrderID = orderID;
            Amount = amount;
            DateTime = dateTime;
            TransactionID = transactionID;

            // Add to class extent
            payments.Add(this);
            TotalPayments = payments.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Payment() { }
    }
}