using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Reservation
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalReservations = 0;

        /// <summary>
        /// Gets or sets the total number of reservations.
        /// </summary>
        public static int TotalReservations
        {
            get => totalReservations;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalReservations cannot be negative.");
                totalReservations = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Reservation> reservations = new List<Reservation>();

        /// <summary>
        /// Gets a read-only list of all reservations.
        /// </summary>
        public static IReadOnlyList<Reservation> GetAll()
        {
            return reservations.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire reservation list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Reservation> loadedReservations)
        {
            reservations = loadedReservations ?? new List<Reservation>();
            TotalReservations = reservations.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int ReservationID { get; set; }

        private DateTime reservationDateTime;

        public DateTime ReservationDateTime
        {
            get => reservationDateTime;
            set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("ReservationDateTime cannot be in the past.");
                reservationDateTime = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? specialRequests;

        public string? SpecialRequests
        {
            get => specialRequests;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 300)
                    throw new ArgumentException("SpecialRequests length cannot exceed 300 characters.");
                specialRequests = value;
            }
        }

        private string status;

        /// <summary>
        /// Gets or sets the status of the reservation.
        /// </summary>
        public string Status
        {
            get => status;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Status cannot be null or empty.");
                status = value;
            }
        }

        // -------------------------------
        // Association Attributes
        // -------------------------------
        private Table table;

        /// <summary>
        /// Gets the table associated with the reservation.
        /// </summary>
        public Table Table => table;

        private Customer customer;

        [XmlIgnore]
        public Customer Customer
        {
            get => customer;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(Customer), "Customer cannot be null.");
                customer = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<OrderItem> orderItems = new List<OrderItem>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<OrderItem> OrderItems => orderItems.AsReadOnly();

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public int NumberOfGuests => orderItems.Sum(item => item.Quantity);

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Reservation class with mandatory and optional attributes.
        /// </summary>
        public Reservation(int reservationID, Customer customer, DateTime reservationDateTime, Table table, string status, string? specialRequests = null)
        {
            ReservationID = reservationID;
            ReservationDateTime = reservationDateTime;
            Status = status;
            SpecialRequests = specialRequests;

            // Associate with Customer and Table
            SetCustomer(customer);
            SetTable(table);

            // Add to class extent
            reservations.Add(this);
            TotalReservations = reservations.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Reservation() { }

        // -------------------------------
        // Validation Helpers
        // -------------------------------
        // No additional validation helpers needed here as validation is handled in property setters

        // -------------------------------
        // Qualified Association Methods
        // -------------------------------
        /// <summary>
        /// Sets the Table for the Reservation, maintaining bidirectional association.
        /// </summary>
        public void SetTable(Table newTable)
        {
            if (newTable == null)
                throw new ArgumentNullException(nameof(newTable), "Table cannot be null.");

            // Check for existing reservation at the same DateTime
            if (newTable.Reservations.ContainsKey(this.ReservationDateTime) && newTable.Reservations[this.ReservationDateTime] != this)
                throw new InvalidOperationException($"A reservation already exists at {this.ReservationDateTime} for this table.");

            table = newTable;

            // Add this reservation to the table's Reservations dictionary if not already present
            if (!newTable.Reservations.ContainsKey(this.ReservationDateTime))
            {
                newTable.AddReservation(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Table, maintaining bidirectional consistency.
        /// </summary>
        public void RemoveTable()
        {
            if (table != null)
            {
                var oldTable = table;
                table = null;

                // Remove this reservation from the old table's Reservations dictionary
                if (oldTable.Reservations.ContainsKey(this.ReservationDateTime))
                {
                    oldTable.RemoveReservation(this.ReservationDateTime);
                }
            }
        }

        // -------------------------------
        // Association Methods
        // -------------------------------
        /// <summary>
        /// Sets the Customer for the Reservation, maintaining bidirectional association.
        /// </summary>
        public void SetCustomer(Customer newCustomer)
        {
            if (newCustomer == null)
                throw new ArgumentNullException(nameof(newCustomer), "Customer cannot be null.");

            Customer = newCustomer;

            // Add this reservation to the customer's reservations list if not already present
            if (!newCustomer.Reservations.Contains(this))
            {
                newCustomer.AddReservation(this);
            }
        }

        /// <summary>
        /// Removes the association with the current Customer, maintaining bidirectional consistency.
        /// </summary>
        public void RemoveCustomer()
        {
            if (Customer != null)
            {
                var oldCustomer = Customer;
                Customer = null;

                // Remove this reservation from the old customer's reservations list
                if (oldCustomer.Reservations.Contains(this))
                {
                    oldCustomer.RemoveReservation(this);
                }
            }
        }

        // -------------------------------
        // Composition Methods
        // -------------------------------
        /// <summary>
        /// Adds an OrderItem to the reservation.
        /// </summary>
        public void AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "OrderItem cannot be null.");
            orderItems.Add(item);
        }

        /// <summary>
        /// Removes an OrderItem from the reservation.
        /// </summary>
        public void RemoveOrderItem(OrderItem item)
        {
            if (item == null || !orderItems.Contains(item))
                throw new ArgumentException("OrderItem not found.");
            orderItems.Remove(item);
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Reservation other)
            {
                return ReservationID == other.ReservationID &&
                       Customer.Equals(other.Customer) &&
                       ReservationDateTime == other.ReservationDateTime &&
                       SpecialRequests == other.SpecialRequests &&
                       Status == other.Status &&
                       Table.Equals(other.Table);
                // Excluding OrderItems for simplicity
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReservationID, Customer, ReservationDateTime, SpecialRequests, Status, Table);
        }
    }
}
