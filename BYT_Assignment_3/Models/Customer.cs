using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    public class Customer
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalCustomers = 0;

        /// <summary>
        /// Gets the total number of customers.
        /// </summary>
        public static int TotalCustomers
        {
            get => totalCustomers;
            private set
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
        /// <param name="loadedCustomers">List of customers to load.</param>
        public static void SetAll(List<Customer> loadedCustomers)
        {
            customers.Clear();
            if (loadedCustomers != null)
            {
                customers.AddRange(loadedCustomers);
            }
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
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be null or empty.");
                name = value;
            }
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? email;
        public string? Email
        {
            get => email;
            set
            {
                if(!string.IsNullOrEmpty(value) && !IsValidEmail(value))
                    throw new ArgumentException("Invalid email format.");
                email = value;
            }
        }

        private string? phoneNumber; // Made nullable
        public string? PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (value != null && !IsValidPhoneNumber(value))
                    throw new ArgumentException("Invalid phone number format.");
                phoneNumber = value;
            }
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        [XmlIgnore] // Prevent direct serialization of the collection
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        /// <summary>
        /// Adds a reservation to the customer's reservation list.
        /// </summary>
        public void AddReservation(Reservation reservation)
        {
            if(reservation == null)
                throw new ArgumentException("Reservation cannot be null.");
            Reservations.Add(reservation);
        }

        /// <summary>
        /// Removes a reservation from the customer's reservation list.
        /// </summary>
        public void RemoveReservation(Reservation reservation)
        {
            if(reservation == null || !Reservations.Contains(reservation))
                throw new ArgumentException("Reservation not found.");
            Reservations.Remove(reservation);
        }

        // -------------------------------
        // Derived Attributes
        // -------------------------------
        [XmlIgnore]
        public int TotalReservations => Reservations.Count;

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Customer class with mandatory and optional attributes.
        /// </summary>
        public Customer(int customerID, string name, string? email = null, string? phoneNumber = null)
        {
            CustomerID = customerID;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;

            // Add to class extent
            customers.Add(this);
            TotalCustomers = customers.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Customer() { }

        // -------------------------------
        // Validation Helpers
        // -------------------------------
        private bool IsValidEmail(string email)
        {
            // Simple email validation logic
            return email.Contains("@") && email.Contains(".");
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Simple phone number validation logic (e.g., length, numeric)
            return phone.Length >= 7 && phone.Length <= 15 && long.TryParse(phone, out _);
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
                       Email == other.Email &&
                       PhoneNumber == other.PhoneNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomerID, Name, Email, PhoneNumber);
        }
    }
}
