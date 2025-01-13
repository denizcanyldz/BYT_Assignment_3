using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Bartender : Staff
    {
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
            CurrentRole = StaffRole.Bartender;
            // Add to bartender extent
            bartenders.Add(this);
            TotalBartenders = bartenders.Count;
        }

        /// <summary>       
        /// Parameterless constructor for serialization.
        /// </summary>
        public Bartender() : base() { }

        public void MixDrink()
        {
            Console.WriteLine($"{Name} is mixing a drink.");
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current Bartender.
        /// </summary>
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

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(StaffID, Name, ContactNumber);
        }
    }
}