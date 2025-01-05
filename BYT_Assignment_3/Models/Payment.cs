using System;
using System.Collections.Generic;
using System.Xml.Serialization;

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
        /// Gets the total number of payments.
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
            if (loadedPayments == null)
                throw new ArgumentNullException(nameof(loadedPayments), "Loaded payments list cannot be null.");
            
            payments = loadedPayments;
            TotalPayments = payments.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        private int paymentID;
        public int PaymentID
        {
            get => paymentID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("PaymentID must be greater than zero.");
                paymentID = value;
            }
        }

        // -------------------------------
        // Association Attributes
        // -------------------------------
        private Order order;

        /// <summary>
        /// Gets the Order associated with this Payment.
        /// </summary>
        [XmlIgnore]
        public Order Order
        {
            get => order;
            private set
            {
                order = value;
            }
        }

        // For serialization purposes, store OrderID
        [XmlElement("OrderID")]
        public int OrderID
        {
            get => Order.OrderID;
            set
            {
                // This property is used during deserialization to associate Payment with Order
                // The actual Order object should be linked after all objects are deserialized
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

        private PaymentMethod paymentMethod;

        /// <summary>
        /// Gets the PaymentMethod associated with this Payment.
        /// </summary>
        [XmlIgnore] // Prevent circular reference during serialization
        public PaymentMethod PaymentMethod
        {
            get => paymentMethod;
            private set
            {
                paymentMethod = value;
            }
        }

        // For serialization purposes, store PaymentMethodID
        [XmlElement("PaymentMethodID")]
        public int PaymentMethodID
        {
            get => PaymentMethod.PaymentMethodID;
            set
            {
                // This property is used during deserialization to associate Payment with PaymentMethod
                // The actual PaymentMethod object should be linked after all objects are deserialized
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Payment class with mandatory and optional attributes.
        /// </summary>
        public Payment(int paymentID, double amount, DateTime dateTime, string? transactionID = null)
        {
            PaymentID = paymentID;
            Amount = amount;
            DateTime = dateTime;
            TransactionID = transactionID;

            // Add to class extent
            payments.Add(this);
            TotalPayments = payments.Count;
        }
        


        /// <summary>
        /// Initializes a new instance of the Payment class with mandatory and optional attributes.
        /// </summary>
        public Payment()
        {
            // Parameterless constructor for serialization
        }

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Sets the Order for this Payment, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="newOrder">The Order to associate with.</param>
        public void SetOrder(Order newOrder, bool callOrderAddPayment = true)
        {
            if (newOrder == null)
                throw new ArgumentNullException(nameof(newOrder), "Order cannot be null.");

            if (this.Order != null && this.Order != newOrder)
            {
                this.Order.RemovePayment(this, false); // Prevent recursion
            }

            this.Order = newOrder;

            if (callOrderAddPayment && !newOrder.Payments.Contains(this))
            {
                newOrder.AddPayment(this, false); // Prevent recursion
            }
        }
       


        /// <summary>
        /// Removes the association with the current Order, maintaining bidirectional consistency.
        /// </summary>
        public void RemoveOrder(bool callPaymentRemoveOrder = true)
        { 
            if (this.Order != null)
            {
                var oldOrder = this.Order;
                this.Order = null;

                if (callPaymentRemoveOrder && oldOrder.Payments.Contains(this))
                {
                    oldOrder.RemovePayment(this, false); // Prevent recursion
                }

                // Additionally, remove from PaymentMethod
                if (this.PaymentMethod != null)
                {
                    this.PaymentMethod.RemovePayment(this, false); // Prevent recursion
                }
            }
        }


        /// <summary>
        /// Sets the PaymentMethod for the Payment.
        /// </summary>
        public void SetPaymentMethod(PaymentMethod method, bool callPaymentMethodAddPayment = true)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method), "PaymentMethod cannot be null.");

            if (this.PaymentMethod != null && this.PaymentMethod != method)
            {
                this.PaymentMethod.RemovePayment(this, false); // Prevent recursion
            }

            this.PaymentMethod = method;

            if (callPaymentMethodAddPayment && !method.Payments.Contains(this))
            {
                method.AddPayment(this, false); // Prevent recursion
            }
        }


        /// <summary>
        /// Removes the PaymentMethod association from the Payment.
        /// </summary>
        public void RemovePaymentMethod(bool callPaymentMethodRemovePayment = true)
        {
            if (this.PaymentMethod != null)
            {
                var oldMethod = this.PaymentMethod;
                this.PaymentMethod = null;

                if (callPaymentMethodRemovePayment && oldMethod.Payments.Contains(this))
                {
                    oldMethod.RemovePayment(this, false); // Prevent recursion
                }

                // Additionally, remove from Order
                if (this.Order != null)
                {
                    this.Order.RemovePayment(this, false); // Prevent recursion
                }
            }
        }


        // -------------------------------
        // RemoveFromExtent Method
        // -------------------------------
        /// <summary>
        /// Removes the Payment and all its associations from the class extent.
        /// </summary>
        internal void RemoveFromExtent()
        {
            // Remove from class extent
            payments.Remove(this);
            TotalPayments = payments.Count;

            // Remove associations
            RemoveOrder();
            RemovePaymentMethod();
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Payment other)
            {
                return PaymentID == other.PaymentID &&
                       (Order?.OrderID == other.Order?.OrderID) &&
                       Amount == other.Amount &&
                       DateTime == other.DateTime &&
                       TransactionID == other.TransactionID &&
                       (PaymentMethod?.PaymentMethodID == other.PaymentMethod?.PaymentMethodID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                PaymentID,
                Order?.OrderID ?? 0,
                Amount,
                DateTime,
                TransactionID,
                PaymentMethod?.PaymentMethodID ?? 0
            );
        }
    }
}
