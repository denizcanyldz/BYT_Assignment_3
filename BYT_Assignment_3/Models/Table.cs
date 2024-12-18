using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Table
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalTables = 0;

        /// <summary>
        /// Gets or sets the total number of tables.
        /// </summary>
        public static int TotalTables
        {
            get => totalTables;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalTables cannot be negative.");
                totalTables = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Table> tables = new List<Table>();

        /// <summary>
        /// Gets a read-only list of all tables.
        /// </summary>
        public static IReadOnlyList<Table> GetAll()
        {
            return tables.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire table list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Table> loadedTables)
        {
            tables = loadedTables ?? new List<Table>();
            TotalTables = tables.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int TableNumber { get; set; }

        private int maxSeats;

        public int MaxSeats
        {
            get => maxSeats;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("MaxSeats must be greater than zero.");
                maxSeats = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? location;

        public string? Location
        {
            get => location;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 100)
                    throw new ArgumentException("Location length cannot exceed 100 characters.");
                location = value;
            }
        }

        private string? seatingArrangement;

        public string? SeatingArrangement
        {
            get => seatingArrangement;
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ArgumentException("SeatingArrangement length cannot exceed 50 characters.");
                seatingArrangement = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<Order> orders = new List<Order>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<Order> Orders => orders.AsReadOnly();

        /// <summary>
        /// Adds an order to the table's order list.
        /// </summary>
        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order cannot be null.");
            orders.Add(order);
        }

        /// <summary>
        /// Removes an order from the table's order list.
        /// </summary>
        public void RemoveOrder(Order order)
        {
            if (order == null || !orders.Contains(order))
                throw new ArgumentException("Order not found.");
            orders.Remove(order);
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public bool IsOccupied => orders.Count > 0;

        private List<Reservation> reservations = new List<Reservation>();

        [XmlIgnore]
        public IReadOnlyList<Reservation> Reservations => reservations.AsReadOnly();

        /// <summary>
        /// Adds a Reservation to the Table.
        /// Enforces unique ReservationDateTime per Table.
        /// </summary>
        public void AddReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentException("Reservation cannot be null.");

            // **Enforce uniqueness of ReservationDateTime**
            if (reservations.Any(r => r.ReservationDateTime == reservation.ReservationDateTime))
                throw new ArgumentException($"A reservation already exists at {reservation.ReservationDateTime} for this table.");

            if (!reservations.Contains(reservation))
            {
                reservations.Add(reservation);
                reservation.SetTable(this);
            }
        }

        /// <summary>
        /// Removes a Reservation from the Table's reservation list.
        /// </summary>
        public void RemoveReservation(Reservation reservation)
        {
            if (reservation == null || !reservations.Contains(reservation))
                throw new ArgumentException("Reservation not found in the Table.");
            reservations.Remove(reservation);
            reservation.RemoveTable();
        }

        /// <summary>
        /// Updates a Reservation in the Table.
        /// Ensures that the new ReservationDateTime does not conflict.
        /// </summary>
        public void UpdateReservation(Reservation oldReservation, Reservation newReservation)
        {
            if (oldReservation == null || newReservation == null)
                throw new ArgumentNullException("Reservation cannot be null.");
            if (!reservations.Contains(oldReservation))
                throw new ArgumentException("Old Reservation not found in the Table.");
            if (reservations.Contains(newReservation))
                throw new ArgumentException("New Reservation already exists in the Table.");

            // **Check for ReservationDateTime conflict**
            if (reservations.Any(r => r.ReservationDateTime == newReservation.ReservationDateTime && r != oldReservation))
                throw new ArgumentException($"A reservation already exists at {newReservation.ReservationDateTime} for this table.");

            // Remove old reservation
            RemoveReservation(oldReservation);

            // Add new reservation
            AddReservation(newReservation);
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Table class with mandatory and optional attributes.
        /// </summary>
        public Table(int tableNumber, int maxSeats, string? location = null, string? seatingArrangement = null)
        {
            TableNumber = tableNumber;
            MaxSeats = maxSeats;
            Location = location;
            SeatingArrangement = seatingArrangement;

            // Add to class extent
            tables.Add(this);
            TotalTables = tables.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Table() { }

        /// <summary>
        /// Determines whether the specified object is equal to the current Table.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Table other)
            {
                return TableNumber == other.TableNumber &&
                       MaxSeats == other.MaxSeats &&
                       Location == other.Location &&
                       SeatingArrangement == other.SeatingArrangement;
                // Excluding Orders and Reservations collections to simplify equality
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(TableNumber, MaxSeats, Location, SeatingArrangement);
        }
    }
}
