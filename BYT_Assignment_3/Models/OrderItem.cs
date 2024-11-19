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
            if (loadedOrderItems == null)
                throw new ArgumentNullException(nameof(loadedOrderItems), "Loaded order items list cannot be null.");
            
            orderItems = loadedOrderItems;
            TotalOrderItems = orderItems.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        
        private int orderItemID;
        public int OrderItemID{
            get => orderItemID;
            set{
                if(value <0)
                    throw new ArgumentException("OrderItemID must be greater that zero.");
                orderItemID = value;
            }
        }

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
        
        /// <summary>
        /// Determines whether the specified object is equal to the current OrderItem.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is OrderItem other)
            {
                return OrderItemID == other.OrderItemID &&
                       ItemName == other.ItemName &&
                       Quantity == other.Quantity &&
                       Price == other.Price &&
                       SpecialInstructions == other.SpecialInstructions;
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(OrderItemID, ItemName, Quantity, Price, SpecialInstructions);
        }
    }
}