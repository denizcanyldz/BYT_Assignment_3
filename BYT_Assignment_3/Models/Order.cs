using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Order
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalOrders = 0;

        /// <summary>
        /// Gets the total number of orders.
        /// </summary>
        public static int TotalOrders
        {
            get => totalOrders;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("TotalOrders cannot be negative.");
                totalOrders = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Order> orders = new List<Order>();

        /// <summary>
        /// Gets a read-only list of all orders.
        /// </summary>
        public static IReadOnlyList<Order> GetAll()
        {
            return orders.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire order list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Order> loadedOrders)
        {
            if (loadedOrders == null)
                throw new ArgumentNullException(nameof(loadedOrders), "Loaded orders list cannot be null.");

            orders = loadedOrders;
            TotalOrders = orders.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        private int orderID;

        public int OrderID
        {
            get => orderID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("OrderID must be greater than zero.");
                orderID = value;
            }
        }

        private DateTime orderDate;

        public DateTime OrderDate
        {
            get => orderDate;
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("OrderDate cannot be in the future.");
                orderDate = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? notes;

        public string? Notes
        {
            get => notes;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 500)
                    throw new ArgumentException("Notes length cannot exceed 500 characters.");
                notes = value;
            }
        }

        private string? discountCode;

        public string? DiscountCode
        {
            get => discountCode;
            set
            {
                if (!string.IsNullOrEmpty(value) && !IsValidDiscountCode(value))
                    throw new ArgumentException("Invalid DiscountCode format.");
                discountCode = value;
            }
        }

        private Table table;

        /// <summary>
        /// Gets or sets the table associated with the order.
        /// </summary>
        public Table Table
        {
            get => table;
            set
            {
                if (value == null)
                    throw new ArgumentException("Table cannot be null.");
                table = value;
            }
        }

        // -------------------------------
        // Composition Attributes
        // -------------------------------
        private List<OrderItem> orderItems = new List<OrderItem>();

        [XmlIgnore]
        public IReadOnlyList<OrderItem> OrderItems => orderItems.AsReadOnly();

        // -------------------------------
        // Association Attributes
        // -------------------------------
        private List<Payment> payments = new List<Payment>();

        [XmlIgnore]
        public IReadOnlyList<Payment> Payments => payments.AsReadOnly();

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        /// <summary>
        /// Gets the total amount of the order, calculated from all order items.
        /// </summary>
        public double TotalAmount => orderItems.Sum(item => item.TotalPrice);

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Order class with mandatory and optional attributes.
        /// </summary>
        public Order(int orderID, DateTime orderDate, Table table, string? notes = null, string? discountCode = null)
        {
            OrderID = orderID;
            OrderDate = orderDate;
            Table = table;
            Notes = notes;
            DiscountCode = discountCode;

            // Add to class extent
            orders.Add(this);
            TotalOrders = orders.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Order()
        {
            // Initialize lists if necessary
            orderItems = new List<OrderItem>();
            payments = new List<Payment>();
        }

        // -------------------------------
        // Validation Helpers
        // -------------------------------
        private bool IsValidDiscountCode(string code)
        {
            return code.Length == 10 && System.Text.RegularExpressions.Regex.IsMatch(code, @"^[a-zA-Z0-9]+$");
        }

        // -------------------------------
        // Composition Methods
        // -------------------------------
        /// <summary>
        /// Creates and adds a new OrderItem to the order.
        /// </summary>
        public OrderItem CreateOrderItem(int orderItemID, string itemName, int quantity, double price, string? specialInstructions = null)
        {
            var orderItem = new OrderItem(orderItemID, itemName, quantity, price, specialInstructions, this);
            return orderItem;
        }

        /// <summary>
        /// Adds an existing OrderItem to the order.
        /// </summary>
        public void AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentException("OrderItem cannot be null.");
            if (orderItems.Contains(item))
                throw new ArgumentException("OrderItem already exists in the order.");
            if (item.ParentOrder != null && item.ParentOrder != this)
                throw new ArgumentException("OrderItem already belongs to another Order.");

            orderItems.Add(item);
            item.SetParentOrder(this);
        }

        /// <summary>
        /// Removes an OrderItem from the order.
        /// </summary>
        public void RemoveOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentException("OrderItem cannot be null.");
            if (!orderItems.Contains(item))
                throw new ArgumentException("OrderItem not found in the order.");
            orderItems.Remove(item);
            item.RemoveParentOrder();
            item.RemoveFromExtent(); // Remove from class extent
        }

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Adds a Payment to the order, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="payment">The Payment to add.</param>
        /// <param name="callPaymentSetOrder">Flag to prevent infinite recursion.</param>
        public void AddPayment(Payment payment, bool callPaymentSetOrder = true)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");
            if (payments.Contains(payment))
                throw new ArgumentException("Payment already exists in the order.");
            if (payment.Order != null && payment.Order != this)
                throw new ArgumentException("Payment already belongs to another Order.");
            if (payment.PaymentMethod == null)
                throw new ArgumentException("PaymentMethod must be set before adding to Order.");

            payments.Add(payment);

            if (callPaymentSetOrder)
            {
                payment.SetOrder(this, false); // Prevent recursion
            }
        }

        /// <summary>
        /// Removes a Payment from the order, ensuring bidirectional consistency.
        /// </summary>
        /// <param name="payment">The Payment to remove.</param>
        /// <param name="callPaymentRemoveOrder">Flag to prevent infinite recursion.</param>
        public void RemovePayment(Payment payment, bool callPaymentRemoveOrder = true)
        {
            if (payment == null)
                throw new ArgumentNullException(nameof(payment), "Payment cannot be null.");
            if (!payments.Contains(payment))
                throw new ArgumentException("Payment not found in the order.");
            payments.Remove(payment);

            if (callPaymentRemoveOrder)
            {
                payment.RemoveOrder(false); // Prevent recursion
            }
        }

        /// <summary>
        /// Updates an existing Payment with a new Payment in the order.
        /// </summary>
        /// <param name="oldPayment">The Payment to be replaced.</param>
        /// <param name="newPayment">The new Payment to replace with.</param>
        public void UpdatePayment(Payment oldPayment, Payment newPayment)
        {
            if (oldPayment == null || newPayment == null)
                throw new ArgumentNullException("Payments cannot be null.");
            if (!payments.Contains(oldPayment))
                throw new ArgumentException("Old Payment not found in the order.");
            if (payments.Contains(newPayment))
                throw new ArgumentException("New Payment already exists in the order.");

            // Remove old payment
            RemovePayment(oldPayment);

            // Add new payment
            AddPayment(newPayment);
        }

        // -------------------------------
        // Order Waiter Association
        // -------------------------------


        private Waiter? waiter;
        public Waiter? Waiter => waiter;

        public void SetWaiter(Waiter waiter, bool callWaiterAddOrder = true)
        {
            if (waiter == null)
                throw new ArgumentNullException(nameof(waiter), "Waiter cannot be null.");
            if (this.waiter == waiter)
                return;

            // Remove existing waiter if applicable
            if (this.waiter != null)
                this.waiter.RemoveOrder(this);

            this.waiter = waiter;

            // Add this order to the waiter's list if applicable
            if (callWaiterAddOrder)
                waiter.AddOrder(this);
        }

        public void RemoveWaiter(bool callWaiterRemoveOrder = true)
        {
            if (this.waiter == null)
                return;

            var oldWaiter = this.waiter;
            this.waiter = null;

            if (callWaiterRemoveOrder)
                oldWaiter.RemoveOrder(this);
        }

        /// <summary>
        /// Order Table association
        /// </summary>
        [XmlIgnore]
        public Table _table { get; private set; }



        public void AddTable(Table table)
        {
            if (table == null)
                throw new ArgumentException("table cannot be null.");
            if (_table == table)
                return;

            _table = table;

            if (table.GetOrders().Contains(this))
            {
                table.AddOrder(this);
            }
        }

        public void RemoveTable(Table table)
        {
            if (table == null)
                throw new ArgumentException("table cannot be null.");

            if (_table != table)
                throw new KeyNotFoundException("The specified table is not associated with this customer.");

            _table = null;

            if (table.GetOrders().Contains(this))
            {
                table.RemoveOrder(this);
            }
        }

        public void ModifyTable(Table newtable, Table oldtable)
        {
            if (newtable == null || oldtable == null)
                throw new ArgumentException("table cannot be null.");
            if (_table == oldtable)
                throw new ArgumentException("table not found.");

            _table = newtable;

            // Update reverse relationship
            if (oldtable.GetOrders().Contains(this))
            {
                oldtable.RemoveOrder(this);
            }

            if (!newtable.GetOrders().Contains(this))
            {
                newtable.AddOrder(this);
            }
        }


        /// <summary>
        /// Association with Customer
        /// </summary>
        [XmlIgnore]
        public Customer _customer { get; private set; }



        public void AddCustomer(Customer Customer)
        {
            if (Customer == null)
                throw new ArgumentException("Customer cannot be null.");
            if (_customer == Customer)
                return;

            _customer = Customer;

            if (Customer.GetOrders().Contains(this))
            {
                Customer.AddOrder(this);
            }
        }

        public void RemoveCustomer(Customer Customer)
        {
            if (Customer == null)
                throw new ArgumentException("Customer cannot be null.");

            if (_customer != Customer)
                throw new KeyNotFoundException("The specified Customer is not associated with this customer.");

            _customer = null;

            if (Customer.GetOrders().Contains(this))
            {
                Customer.RemoveOrder(this);
            }
        }

        public void ModifyCustomer(Customer newCustomer, Customer oldCustomer)
        {
            if (newCustomer == null || oldCustomer == null)
                throw new ArgumentException("Customer cannot be null.");
            if (_customer != oldCustomer)
                throw new ArgumentException("Customer not found.");

            _customer = newCustomer;

            // Update reverse relationship
            if (oldCustomer.GetOrders().Contains(this))
            {
                oldCustomer.RemoveOrder(this);
            }

            if (!newCustomer.GetOrders().Contains(this))
            {
                newCustomer.AddOrder(this);
            }
        }


        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Order other)
            {
                return OrderID == other.OrderID &&
                       OrderDate == other.OrderDate &&
                       Notes == other.Notes &&
                       DiscountCode == other.DiscountCode &&
                       Table.Equals(other.Table);
                // Excluding OrderItems and Payments collections to simplify equality
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderID, OrderDate, Notes, DiscountCode, Table);
        }

        // -------------------------------
        // RemoveFromExtent Method
        // -------------------------------
        /// <summary>
        /// Removes the Order and all its associated OrderItems and Payments from the class extent.
        /// </summary>
        public void RemoveFromExtent()
        {
            // Remove all associated OrderItems
            foreach (var item in orderItems.ToList())
            {
                RemoveOrderItem(item);
            }

            // Remove all associated Payments
            foreach (var payment in payments.ToList())
            {
                RemovePayment(payment);
            }

            // Remove from class extent
            orders.Remove(this);
            TotalOrders = orders.Count;
        }

    }
}
