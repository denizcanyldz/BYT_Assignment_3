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
        // Optional Attributes
        // -------------------------------
        private string? licenseNumber;

        public string? LicenseNumber
        {
            get => licenseNumber;
            set
            {
                if(!string.IsNullOrEmpty(value) && value.Length > 50)
                    throw new ArgumentException("LicenseNumber length cannot exceed 50 characters.");
                licenseNumber = value;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Bartender class with mandatory and optional attributes.
        /// </summary>
        public Bartender(int staffID, string name, string? licenseNumber = null, string? shift = null)
            : base(staffID, name, shift)
        {
            LicenseNumber = licenseNumber;

            // Add to bartender extent
            bartenders.Add(this);
            TotalBartenders = bartenders.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Bartender() : base() { }
    }
}