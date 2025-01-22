using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class OrderItem
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalOrderItems = 0;

        /// <summary>
        /// Gets the total number of order items.
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

        public int OrderItemID
        {
            get => orderItemID;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("OrderItemID must be greater than zero.");
                orderItemID = value;
            }
        }

        private string itemName;

        public string ItemName
        {
            get => itemName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
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
                if (value <= 0)
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
                if (value < 0)
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
                if (!string.IsNullOrEmpty(value) && value.Length > 250)
                    throw new ArgumentException("SpecialInstructions length cannot exceed 250 characters.");
                specialInstructions = value;
            }
        }

        // -------------------------------
        // Composition Attribute
        // -------------------------------
        [XmlIgnore]
        public Order? ParentOrder { get; private set; }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        /// <summary>
        /// Gets the total price for this order item (Price * Quantity).
        /// </summary>
        public double TotalPrice => Price * Quantity;

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the OrderItem class with mandatory and optional attributes.
        /// </summary>
        /// <param name="orderItemID">Unique identifier for the order item.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="quantity">Quantity ordered.</param>
        /// <param name="price">Price per unit.</param>
        /// <param name="specialInstructions">Any special instructions for the item.</param>
        /// <param name="parentOrder">The Order to which this item belongs.</param>
        public OrderItem(int orderItemID, string itemName, int quantity, double price, string? specialInstructions = null, Order? parentOrder = null)
        {
            OrderItemID = orderItemID;
            ItemName = itemName;
            Quantity = quantity;
            Price = price;
            SpecialInstructions = specialInstructions;

            if (parentOrder != null)
            {
                SetParentOrder(parentOrder);
            }

            // Add to class extent
            orderItems.Add(this);
            TotalOrderItems = orderItems.Count;
        }
        
        /// <summary>
        /// Initializes a new instance of the OrderItem class with mandatory and optional attributes.
        /// Ensures that the OrderItem is associated with a parent Order upon creation.
        /// </summary>
        /// <param name="orderItemID">Unique identifier for the order item.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="quantity">Quantity ordered.</param>
        /// <param name="price">Price per unit.</param>
        /// <param name="parentOrder">The Order to which this item belongs.</param>
        /// <param name="specialInstructions">Any special instructions for the item.</param>
        public OrderItem(int orderItemID, string itemName, int quantity, double price, Order parentOrder, string? specialInstructions = null)
        {
            if (parentOrder == null)
                throw new ArgumentNullException(nameof(parentOrder), "Parent Order cannot be null.");

            OrderItemID = orderItemID;
            ItemName = itemName;
            Quantity = quantity;
            Price = price;
            SpecialInstructions = specialInstructions;

            ParentOrder = parentOrder;
            parentOrder.AddOrderItem(this); // Ensure bidirectional association

            // Add to class extent
            orderItems.Add(this);
            TotalOrderItems = orderItems.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public OrderItem()
        {
            // Initialize if necessary
        }

        // -------------------------------
        // Composition Methods
        // -------------------------------
        internal void RemoveFromExtent()
        {
            orderItems.Remove(this);
            TotalOrderItems = orderItems.Count;
        }

        /// <summary>
        /// Sets the parent order for this order item.
        /// </summary>
        /// <param name="order">The Order to set as parent.</param>
        internal void SetParentOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "Parent Order cannot be null.");
            if (ParentOrder != null && ParentOrder != order)
                throw new InvalidOperationException("OrderItem already belongs to another Order.");

            ParentOrder = order;
            if (!order.OrderItems.Contains(this))
            {
                order.AddOrderItem(this);
            }
        }

        /// <summary>
        /// Removes the parent order association from this order item.
        /// </summary>
        internal void RemoveParentOrder()
        {
            ParentOrder = null;
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is OrderItem other)
            {
                return OrderItemID == other.OrderItemID &&
                       ItemName == other.ItemName &&
                       Quantity == other.Quantity &&
                       Price == other.Price &&
                       SpecialInstructions == other.SpecialInstructions;
                // Excluding ParentOrder to simplify equality
            }
            return false;
        }

        /// <summary>
        /// Association with MenuItem
        /// </summary>

        [XmlIgnore]
        public MenuItem _menuItem { get; private set; }

        

        public void AddMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentException("MenuItem cannot be null.");
            if (_menuItem == menuItem)
                return;

            _menuItem = menuItem;

            if (menuItem._orderItem != this)
            {
                menuItem.AddOrderItem(this);
            }
        }

        public void RemoveMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentException("MenuItem cannot be null.");

            if (_menuItem != menuItem)
                throw new KeyNotFoundException("The specified MenuItem is not associated with this customer.");

            _menuItem = null;

            if (menuItem._orderItem == this)
            {
                menuItem.RemoveOrderItem(this);
            }
        }

        public void ModifyMenuItem(MenuItem newMenuItem, MenuItem oldMenuItem)
        {
            if (newMenuItem == null || oldMenuItem == null)
                throw new ArgumentException("MenuItem cannot be null.");
            if (_menuItem == oldMenuItem)
                throw new ArgumentException("MenuItem not found.");

            _menuItem = newMenuItem;

            // Update reverse relationship
            if (oldMenuItem._orderItem == this)
            {
                oldMenuItem.RemoveOrderItem(this);
            }

            if (newMenuItem._orderItem != this)
            {
                newMenuItem.AddOrderItem(this);
            }
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(OrderItemID, ItemName, Quantity, Price, SpecialInstructions);
        }
    }
}
