using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Order
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalOrders = 0;

        /// <summary>
        /// Gets or sets the total number of orders.
        /// </summary>
        public static int TotalOrders
        {
            get => totalOrders;
            set
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
            orders = loadedOrders ?? new List<Order>();
            TotalOrders = orders.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int OrderID { get; set; }

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
                if(!string.IsNullOrEmpty(value) && value.Length > 500)
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
                if(!string.IsNullOrEmpty(value) && !IsValidDiscountCode(value))
                    throw new ArgumentException("Invalid DiscountCode format.");
                discountCode = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        private List<OrderItem> orderItems = new List<OrderItem>();

        [XmlIgnore] // Prevent direct serialization of the collection
        public IReadOnlyList<OrderItem> OrderItems => orderItems.AsReadOnly();

        /// <summary>
        /// Adds an order item to the order.
        /// </summary>
        public void AddOrderItem(OrderItem item)
        {
            if(item == null)
                throw new ArgumentException("OrderItem cannot be null.");
            orderItems.Add(item);
        }

        /// <summary>
        /// Removes an order item from the order.
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
        public double TotalPrice
        {
            get
            {
                double total = 0;
                foreach(var item in orderItems)
                {
                    if(item != null)
                        total += item.Price * item.Quantity;
                }
                return total;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Order class with mandatory and optional attributes.
        /// </summary>
        public Order(int orderID, DateTime orderDate, string? notes = null, string? discountCode = null)
        {
            OrderID = orderID;
            OrderDate = orderDate;
            Notes = notes;
            DiscountCode = discountCode;

            // Add to class extent
            orders.Add(this);
            TotalOrders = orders.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Order() { }

        // -------------------------------
        // Validation Helpers
        // -------------------------------
        private bool IsValidDiscountCode(string code)
        {
            // Simple discount code validation logic (e.g., alphanumeric, specific length)
            return code.Length == 10 && System.Text.RegularExpressions.Regex.IsMatch(code, @"^[a-zA-Z0-9]+$");
        }
    }
}