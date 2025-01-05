using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Adds a Payment to the PaymentMethod, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="payment">The Payment to add.</param>
        /// <param name="callPaymentSetMethod">Flag to prevent infinite recursion.</param>
        public void AddPayment(Payment payment, bool callPaymentSetMethod = true)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");
            if (!payments.Contains(payment))
            {
                payments.Add(payment);
                if (callPaymentSetMethod && payment.PaymentMethod != this)
                {
                    payment.SetPaymentMethod(this, false); // Prevent recursion
                }
            }
        }

        /// <summary>
        /// Removes a Payment from the PaymentMethod, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="payment">The Payment to remove.</param>
        /// <param name="callPaymentRemoveMethod">Flag to prevent infinite recursion.</param>
        public void RemovePayment(Payment payment, bool callPaymentRemoveMethod = true)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");
            if (payments.Contains(payment))
            {
                payments.Remove(payment);
                if (callPaymentRemoveMethod && payment.PaymentMethod == this)
                {
                    payment.RemovePaymentMethod(false); // Prevent recursion
                }
            }
            else
            {
                throw new ArgumentException("Payment not found in the PaymentMethod.");
            }
        }

        /// <summary>
        /// Updates an existing Payment with a new Payment in the PaymentMethod.
        /// </summary>
        /// <param name="oldPayment">The Payment to be replaced.</param>
        /// <param name="newPayment">The new Payment to replace with.</param>
        public void UpdatePayment(Payment oldPayment, Payment newPayment)
        {
            if (oldPayment == null || newPayment == null)
                throw new ArgumentNullException("Payments cannot be null.");
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

        // -------------------------------
        // RemoveFromExtent Method
        // -------------------------------
        /// <summary>
        /// Removes the PaymentMethod and all its associated Payments from the class extent.
        /// </summary>
        internal void RemoveFromExtent()
        {
            // Remove all associated Payments
            foreach (var payment in payments.ToList()) // Use ToList to avoid modification during iteration
            {
                RemovePayment(payment);
            }

            // Remove from class extent
            paymentMethods.Remove(this);
            TotalPaymentMethods = paymentMethods.Count;
        }
    }
}
