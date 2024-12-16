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

        private Customer customer;
        public Customer Customer
        {
            get => customer;
            private set
            {
                if (value == null)
                    throw new ArgumentException("Customer cannot be null.");
                customer = value;
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

        private string status;

        /// <summary>
        /// Gets or sets the status of the reservation.
        /// </summary>
        public string Status
        {
            get => status;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Status cannot be null or empty.");
                status = value;
            }
        }

        private Table table;

        /// <summary>
        /// Gets or sets the table associated with the reservation.
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
        /// <summary>
        /// Gets the total number of guests based on order items.
        /// </summary>
        public int NumberOfGuests => orderItems.Sum(item => item.Quantity);

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Reservation class with mandatory and optional attributes.
        /// </summary>
        public Reservation(int reservationID, Customer customer, DateTime reservationDate, Table table, string status, string? specialRequests = null)
        {
            ReservationID = reservationID;
            SetCustomer(customer);
            ReservationDate = reservationDate;
            Table = table;
            Status = status;
            SpecialRequests = specialRequests;

            // Add to class extent
            reservations.Add(this);
            TotalReservations = reservations.Count;
        }


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Reservation() { }

        /// <summary>
        /// Set the Customer for this Reservation.
        /// </summary>
        public void SetCustomer(Customer newCustomer)
        {
            if (newCustomer == null)
                throw new ArgumentException("Customer cannot be null.");

            // If there's an existing customer, remove this reservation from it first
            if (customer != null && customer != newCustomer)
            {
                var oldCustomer = customer;
                customer = null; 
                oldCustomer.RemoveReservation(this);
            }

            customer = newCustomer;
            if (!newCustomer.GetReservations().Contains(this))
            {
                newCustomer.AddReservation(this);
            }
        }

        /// <summary>
        /// Remove the Customer association from this Reservation.
        /// Also updates the reverse relationship.
        /// </summary>
        public void RemoveCustomer()
        {
            if (customer != null)
            {
                var oldCustomer = customer;
                customer = null;
                if (oldCustomer.GetReservations().Contains(this))
                {
                    oldCustomer.RemoveReservation(this);
                }
            }
        }
        
        /// <summary>
        /// Determines whether the specified object is equal to the current Reservation.
        /// </summary>
       public override bool Equals(object obj)
        {
            if (obj is Reservation other)
            {
                return ReservationID == other.ReservationID &&
                       Customer == other.Customer &&
                       ReservationDate == other.ReservationDate &&
                       SpecialRequests == other.SpecialRequests &&
                       Status == other.Status &&
                       Table.Equals(other.Table);
            }
            return false;
        }
        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
       public override int GetHashCode()
        {
            return HashCode.Combine(ReservationID, Customer, ReservationDate, SpecialRequests, Status, Table);
        }
    }
}