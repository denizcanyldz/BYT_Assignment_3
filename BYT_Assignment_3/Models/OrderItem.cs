namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class OrderItem
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalOrderItems = 0;

        /// <summary>
        /// Gets or sets the total number of order items.
        /// </summary>
        public static int TotalOrderItems
        {
            get => totalOrderItems;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalOrderItems cannot be negative.");
                totalOrderItems = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<OrderItem> orderItems = new List<OrderItem>();

        /// <summary>
        /// Gets a read-only list of all order items.
        /// </summary>
        public static IReadOnlyList<OrderItem> GetAll()
        {
            return orderItems.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire order item list (used during deserialization).
        /// </summary>
        public static void SetAll(List<OrderItem> loadedOrderItems)
        {
            orderItems = loadedOrderItems ?? new List<OrderItem>();
            TotalOrderItems = orderItems.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int OrderItemID { get; private set; }

        private string itemName;

        public string ItemName
        {
            get => itemName;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ItemName cannot be null or empty.");
                itemName = value;
            }
        }

        private int quantity;

        public int Quantity
        {
            get => quantity;
            set
            {
                if(value <= 0)
                    throw new ArgumentException("Quantity must be greater than zero.");
                quantity = value;
            }
        }

        private double price;

        public double Price
        {
            get => price;
            set
            {
                if(value < 0)
                    throw new ArgumentException("Price cannot be negative.");
                price = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? specialInstructions;

        public string? SpecialInstructions
        {
            get => specialInstructions;
            set
            {
                if(!string.IsNullOrEmpty(value) && value.Length > 250)
                    throw new ArgumentException("SpecialInstructions length cannot exceed 250 characters.");
                specialInstructions = value;
            }
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        public double TotalPrice => Price * Quantity;

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the OrderItem class with mandatory and optional attributes.
        /// </summary>
        public OrderItem(int orderItemID, string itemName, int quantity, double price, string? specialInstructions = null)
        {
            OrderItemID = orderItemID;
            ItemName = itemName;
            Quantity = quantity;
            Price = price;
            SpecialInstructions = specialInstructions;

            // Add to class extent
            orderItems.Add(this);
            TotalOrderItems = orderItems.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public OrderItem() { }
    }
}