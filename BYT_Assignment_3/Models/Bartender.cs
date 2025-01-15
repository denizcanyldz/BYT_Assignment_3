using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using BYT_Assignment_3.Interfaces;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Bartender : Staff, IRole, IBartender
    {
        public string RoleName => "Bartender";

        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalBartenders = 0;

        /// <summary>
        /// Gets or sets the total number of bartenders.
        /// </summary>
        public static int TotalBartenders
        {
            get => totalBartenders;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalBartenders cannot be negative.");
                totalBartenders = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Bartender> bartenders = new List<Bartender>();

        /// <summary>
        /// Gets a read-only list of all bartenders.
        /// </summary>
        public static IReadOnlyList<Bartender> GetAll()
        {
            return bartenders.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire bartender list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Bartender> loadedBartenders)
        {
            bartenders = loadedBartenders ?? new List<Bartender>();
            TotalBartenders = bartenders.Count;
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Bartender class with mandatory attributes.
        /// </summary>
        public Bartender(int staffID, string name, string? contactNumber = null)
            : base(staffID, name, contactNumber)
        {
            // Add to bartender extent
            bartenders.Add(this);
            TotalBartenders = bartenders.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Bartender() : base() { }

        // -------------------------------
        // IBartender Implementation
        // -------------------------------
        public void MixDrink(string drinkName)
        {
            if (string.IsNullOrWhiteSpace(drinkName))
                throw new ArgumentException("Drink name cannot be null or empty.");

            Console.WriteLine($"{Name} is mixing a {drinkName}.");
            // Implement drink mixing logic here
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Bartender other)
            {
                return StaffID == other.StaffID &&
                       Name == other.Name &&
                       ContactNumber == other.ContactNumber;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StaffID, Name, ContactNumber);
        }
    }
}
