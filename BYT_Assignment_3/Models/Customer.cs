using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Customer
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalCustomers = 0;

        /// <summary>
        /// Gets or sets the total number of customers.
        /// </summary>
        public static int TotalCustomers
        {
            get => totalCustomers;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalCustomers cannot be negative.");
                totalCustomers = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Customer> customers = new List<Customer>();

        /// <summary>
        /// Gets a read-only list of all customers.
        /// </summary>
        public static IReadOnlyList<Customer> GetAll()
        {
            return customers.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire customer list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Customer> loadedCustomers)
        {
            customers = loadedCustomers ?? new List<Customer>();
            TotalCustomers = customers.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int CustomerID { get; set; }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty.");
                name = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? contactNumber;

        public string? ContactNumber
        {
            get => contactNumber;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ArgumentException("ContactNumber length cannot exceed 50 characters.");
                contactNumber = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private readonly List<Reservation> reservations = new List<Reservation>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<Reservation> Reservations => reservations.AsReadOnly();

        private readonly List<Feedback> feedbacks = new List<Feedback>();

        [XmlIgnore]
        public IReadOnlyList<Feedback> Feedbacks => feedbacks.AsReadOnly();

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Customer class with mandatory and optional attributes.
        /// </summary>
        public Customer(int customerID, string name, string? contactNumber = null)
        {
            CustomerID = customerID;
            Name = name;
            ContactNumber = contactNumber;

            // Add to class extent and update total
            customers.Add(this);
            TotalCustomers = customers.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Customer()
        {
            // Initialize lists
            reservations = new List<Reservation>();
            feedbacks = new List<Feedback>();
        }

        // -------------------------------
        // Association Methods
        // -------------------------------


        /// <summary>
        /// Adds a Reservation to the Customer.
        /// </summary>
        public void AddReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null.");
            if (!reservations.Contains(reservation))
            {
                reservations.Add(reservation);
                reservation.SetCustomer(this);
            }
        }

        /// <summary>
        /// Removes a Reservation from the Customer.
        /// </summary>
        public void RemoveReservation(Reservation reservation)
        {
            if (reservation == null || !reservations.Contains(reservation))
                throw new ArgumentException("Reservation not found in the Customer.");
            reservations.Remove(reservation);
            reservation.RemoveCustomer();
        }

        /// <summary>
        /// Updates a Reservation in the Customer.
        /// </summary>
        public void UpdateReservation(Reservation oldReservation, Reservation newReservation)
        {
            if (oldReservation == null || newReservation == null)
                throw new ArgumentNullException("Reservation cannot be null.");
            if (!reservations.Contains(oldReservation))
                throw new ArgumentException("Old Reservation not found in the Customer.");
            if (reservations.Contains(newReservation))
                throw new ArgumentException("New Reservation already exists in the Customer.");

            // Remove old reservation
                                                RemoveReservation(oldReservation);

            // Add new reservation
            AddReservation(newReservation);
        }

        /// <summary>
        /// Adds a Feedback to the Customer.
        /// </summary>
        public void AddFeedback(Feedback feedback)
        {
            if (feedback == null)
                throw new ArgumentNullException(nameof(feedback), "Feedback cannot be null.");
            if (!feedbacks.Contains(feedback))
            {
                feedbacks.Add(feedback);
                feedback.SetCustomer(this);
            }
        }

        /// <summary>
        /// Removes a Feedback from the Customer.
        /// </summary>
        public void RemoveFeedback(Feedback feedback)
        {
            if (feedback == null || !feedbacks.Contains(feedback))
                throw new ArgumentException("Feedback not found in the Customer.");
            feedbacks.Remove(feedback);
            feedback.RemoveCustomer();
        }

        /// <summary>
        /// Updates a Feedback in the Customer.
        /// </summary>
        public void UpdateFeedback(Feedback oldFeedback, Feedback newFeedback)
        {
            if (oldFeedback == null || newFeedback == null)
                throw new ArgumentNullException("Feedback cannot be null.");
            if (!feedbacks.Contains(oldFeedback))
                throw new ArgumentException("Old Feedback not found in the Customer.");
            if (feedbacks.Contains(newFeedback))
                throw new ArgumentException("New Feedback already exists in the Customer.");

            // Remove old feedback
            RemoveFeedback(oldFeedback);

            // Add new feedback
            AddFeedback(newFeedback);
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Customer other)
            {
                return CustomerID == other.CustomerID &&
                       Name == other.Name &&
                       ContactNumber == other.ContactNumber;
                // Excluding Reservations and Feedbacks for simplicity
            }
            return false;
        }

        /// <summary>
        /// Association with Order
        /// </summary>
        [XmlIgnore]
        private List<Order> _orders = new List<Order>();

        public IReadOnlyList<Order> GetOrders() { return _orders.AsReadOnly(); }



        public void AddOrder(Order Order)
        {
            if (Order == null)
                throw new ArgumentException("Order cannot be null.");
            if (_orders.Contains(Order))
                return;

            _orders.Add(Order);


            if (Order._customer != this)
            {
                Order.AddCustomer(this);
            }
        }

        public void RemoveOrder(Order Order)
        {
            if (Order == null)
                throw new ArgumentException("Order cannot be null.");

            if (!_orders.Contains(Order))
                throw new KeyNotFoundException("The specified Order is not associated with this Order.");

            _orders.Remove(Order);

            if (Order._customer == this)
            {
                Order.RemoveCustomer(this);
            }
        }

        public void ModifyOrder(Order newOrder, Order oldOrder)
        {
            if (newOrder == null || oldOrder == null)
                throw new ArgumentException("Order cannot be null.");
            if (!_orders.Contains(oldOrder))
                throw new ArgumentException("Order not found.");

            _orders.Add(newOrder);

            // Update reverse relationship
            if (oldOrder._customer == this)
            {
                oldOrder.RemoveCustomer(this);
            }

            if (newOrder._customer != this)
            {
                newOrder.AddCustomer(this);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerID, Name, ContactNumber);
        }
    }
}
