using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Chef : Staff
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalChefs = 0;

        /// <summary>
        /// Gets or sets the total number of chefs.
        /// </summary>
        public static int TotalChefs
        {
            get => totalChefs;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalChefs cannot be negative.");
                totalChefs = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Chef> chefs = new List<Chef>();

        /// <summary>
        /// Gets a read-only list of all chefs.
        /// </summary>
        public static IReadOnlyList<Chef> GetAll()
        {
            return chefs.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire chef list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Chef> loadedChefs)
        {
            chefs = loadedChefs ?? new List<Chef>();
            TotalChefs = chefs.Count;
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Optional Attributes
        // -------------------------------
        private string? specialty;

        public string? Specialty
        {
            get => specialty;
            set
            {
                if(!string.IsNullOrEmpty(value) && value.Length > 100)
                    throw new ArgumentException("Specialty length cannot exceed 100 characters.");
                specialty = value;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Chef class with mandatory and optional attributes.
        /// </summary>
        public Chef(int staffID, string name, string? specialty = null, string? contactNumber = null)
            : base(staffID, name, contactNumber)
        {
            Specialty = specialty;

            // Add to chef extent
            chefs.Add(this);
            TotalChefs = chefs.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Chef() : base() { }
    }
}