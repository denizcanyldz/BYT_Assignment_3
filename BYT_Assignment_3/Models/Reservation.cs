using System.Xml.Serialization;

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

        private int customerID;

        public int CustomerID
        {
            get => customerID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("CustomerID must be positive.");
                customerID = value;
            }
        }

        private DateTime reservationDate;

        public DateTime ReservationDate
        {
            get => reservationDate;
            set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("ReservationDate cannot be in the past.");
                reservationDate = value;
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
                if(!string.IsNullOrEmpty(value) && value.Length > 300)
                    throw new ArgumentException("SpecialRequests length cannot exceed 300 characters.");
                specialRequests = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<OrderItem> orderItems = new List<OrderItem>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<OrderItem> OrderItems => orderItems.AsReadOnly();

        /// <summary>
        /// Adds an order item to the reservation.
        /// </summary>
        public void AddOrderItem(OrderItem item)
        {
            if(item == null)
                throw new ArgumentException("OrderItem cannot be null.");
            orderItems.Add(item);
        }

        /// <summary>
        /// Removes an order item from the reservation.
        /// </summary>
        public void RemoveOrderItem(OrderItem item)
        {
            if(item == null || !orderItems.Contains(item))
                throw new ArgumentException("OrderItem not found.");
            orderItems.Remove(item);
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public int NumberOfGuests
        {
            get
            {
                int totalGuests = 0;
                foreach(var item in orderItems)
                {
                    if(item != null)
                        totalGuests += item.Quantity;
                }
                return totalGuests;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Reservation class with mandatory and optional attributes.
        /// </summary>
        public Reservation(int reservationID, int customerID, DateTime reservationDate, string? specialRequests = null)
        {
            ReservationID = reservationID;
            CustomerID = customerID;
            ReservationDate = reservationDate;
            SpecialRequests = specialRequests;

            // Add to class extent
            reservations.Add(this);
            TotalReservations = reservations.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Reservation() { }
    }
}