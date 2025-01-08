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
        // Qualified Association: DateTime -> Reservation
        [XmlIgnore] // Prevent direct serialization of the dictionary
        public Dictionary<DateTime, Reservation> Reservations { get; private set; } = new Dictionary<DateTime, Reservation>();

        // For serialization purposes, convert the dictionary to a list
        [XmlArray("Reservations")]
        [XmlArrayItem("Reservation")]
        public List<Reservation> ReservationList
        {
            get => Reservations.Values.ToList();
            set
            {
                Reservations = new Dictionary<DateTime, Reservation>();
                if (value != null)
                {
                    foreach (var reservation in value)
                    {
                        AddReservation(reservation);
                    }
                }
            }
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public bool IsOccupied => Reservations.Any();

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
        public Table()
        {
            // Initialize Reservations dictionary
            Reservations = new Dictionary<DateTime, Reservation>();
        }

        // -------------------------------
        // Qualified Association Methods
        // -------------------------------
        /// <summary>
        /// Adds a Reservation to the Table at the specified DateTime.
        /// Ensures that no duplicate reservations exist at the same DateTime.
        /// </summary>
        /// <param name="reservation">The Reservation to add.</param>
        public void AddReservation(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null.");

            DateTime reservationDateTime = reservation.ReservationDateTime;

            if (Reservations.ContainsKey(reservationDateTime))
                throw new InvalidOperationException($"A reservation already exists at {reservationDateTime} for this table.");

            Reservations[reservationDateTime] = reservation;

            // Set the Table in the Reservation if not already set
            if (reservation.Table != this)
            {
                reservation.SetTable(this);
            }
        }

        /// <summary>
        /// Removes a Reservation from the Table at the specified DateTime.
        /// </summary>
        /// <param name="reservationDateTime">The DateTime of the Reservation to remove.</param>
        public void RemoveReservation(DateTime reservationDateTime)
        {
            if (Reservations.ContainsKey(reservationDateTime))
            {
                var reservation = Reservations[reservationDateTime];
                Reservations.Remove(reservationDateTime);
                reservation.RemoveTable();
            }
            else
            {
                throw new KeyNotFoundException("No reservation found at the specified DateTime for this table.");
            }
        }

        /// <summary>
        /// Updates an existing Reservation's DateTime.
        /// Ensures that the new DateTime does not conflict with existing reservations.
        /// </summary>
        /// <param name="currentDateTime">The current DateTime of the Reservation.</param>
        /// <param name="newDateTime">The new DateTime to assign to the Reservation.</param>
        public void UpdateReservationDateTime(DateTime currentDateTime, DateTime newDateTime)
        {
            if (!Reservations.ContainsKey(currentDateTime))
                throw new KeyNotFoundException("No reservation found at the current DateTime for this table.");

            if (Reservations.ContainsKey(newDateTime))
                throw new InvalidOperationException($"A reservation already exists at {newDateTime} for this table.");

            var reservation = Reservations[currentDateTime];
            Reservations.Remove(currentDateTime);
            Reservations[newDateTime] = reservation;

            reservation.ReservationDateTime = newDateTime;
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Table other)
            {
                return TableNumber == other.TableNumber &&
                       MaxSeats == other.MaxSeats &&
                       Location == other.Location &&
                       SeatingArrangement == other.SeatingArrangement;
                // Excluding Reservations for simplicity
            }
            return false;
        }

        /// <summary>
        /// Association with Order
        /// </summary>
        /// 

        [XmlIgnore]
        private List<Order> _orders = new List<Order>();

        public IReadOnlyList<Order> GetOrders() { return _orders.AsReadOnly(); }



        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order cannot be null.");
            if (_orders.Contains(order))
                return;

            _orders.Add(order);

            if (order._table != this)
            {
                order.AddTable(this);
            }
        }

        public void RemoveOrder(Order order)
        {
            if (order == null)
                throw new ArgumentException("Order cannot be null.");

            if (!_orders.Contains(order))
                throw new KeyNotFoundException("The specified Order is not associated with this customer.");

            _orders.Remove(order);

            if (order._table == this)
            {
                order.RemoveTable(this);
            }
        }

        public void ModifyOrder(Order newOrder, Order oldOrder)
        {
            if (newOrder == null || oldOrder == null)
                throw new ArgumentException("Order cannot be null.");
            if (!_orders.Contains(oldOrder))
                throw new ArgumentException("Order not found.");

            int index = _orders.IndexOf(oldOrder);
            _orders[index] = newOrder;

            // Update reverse relationship
            if (oldOrder._table == this)
            {
                oldOrder.RemoveTable(this);
            }

            if (newOrder._table != this)
            {
                newOrder.AddTable(this);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TableNumber, MaxSeats, Location, SeatingArrangement);
        }
    }
}
