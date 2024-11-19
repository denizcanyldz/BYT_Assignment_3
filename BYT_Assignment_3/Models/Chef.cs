using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Models
{
    public class Chef : Staff
    {
        // -------------------------------
        // Class/Static Attributes
        // -------------------------------
        private static int totalChefs = 0;

        /// <summary>
        /// Gets the total number of chefs.
        /// </summary>
        public static int TotalChefs
        {
            get => totalChefs;
            private set
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
        /// Validates each chef's specialties before adding.
        /// </summary>
        /// <param name="loadedChefs">List of chefs to load.</param>
        public static void SetAll(List<Chef> loadedChefs)
        {
            chefs.Clear();

            foreach (var chef in loadedChefs)
            {
                try
                {
                    Console.WriteLine($"Processing Chef: {chef.Name} (StaffID: {chef.StaffID})");

                    // Validate each specialty in the chef's specialties list
                    if (chef.Specialties == null)
                        throw new ArgumentNullException(nameof(chef.Specialties), "Specialties cannot be null.");

                    foreach (var specialty in chef.Specialties)
                    {
                        if (specialty == null)
                        {
                            Console.WriteLine("Encountered a null specialty.");
                        }
                        else
                        {
                            Console.WriteLine($"Specialty: '{specialty}'");
                        }

                        if (string.IsNullOrWhiteSpace(specialty))
                            throw new ArgumentException("Specialty cannot be null or empty.");
                        if (specialty.Length > 100)
                            throw new ArgumentException("Specialty length cannot exceed 100 characters.");
                    }

                    // Add to chefs list
                    chefs.Add(chef);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding Chef (StaffID: {chef.StaffID}): {ex.Message}");
                    throw;
                }
            }

            TotalChefs = chefs.Count;
            Staff.TotalStaff = Staff.GetAll().Count;
        }

        // -------------------------------
        // Multi-Value Attributes
        // -------------------------------
        [XmlArray("Specialties")]
        [XmlArrayItem("Specialty", IsNullable = true)]
        public List<string?> Specialties { get; set; } = new List<string?>();

        /// <summary>
        /// Adds a specialty to the chef's specialties list.
        /// </summary>
        /// <param name="specialty">The specialty to add.</param>
        public void AddSpecialty(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Specialty cannot be null or empty.");
            if (specialty.Length > 100)
                throw new ArgumentException("Specialty length cannot exceed 100 characters.");
            Specialties.Add(specialty);
        }

        /// <summary>
        /// Removes a specialty from the chef's specialties list.
        /// </summary>
        /// <param name="specialty">The specialty to remove.</param>
        public void RemoveSpecialty(string specialty)
        {
            if (string.IsNullOrWhiteSpace(specialty))
                throw new ArgumentException("Specialty cannot be null or empty.");
            if (!Specialties.Contains(specialty))
                throw new ArgumentException("Specialty not found.");
            Specialties.Remove(specialty);
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Chef class with mandatory and optional attributes.
        /// </summary>
        /// <param name="staffID">The staff ID.</param>
        /// <param name="name">The name of the chef.</param>
        /// <param name="specialties">A list of specialties.</param>
        /// <param name="contactNumber">The contact number.</param>
        public Chef(int staffID, string name, List<string?>? specialties = null, string? contactNumber = null)
            : base(staffID, name, contactNumber)
        {
            if (specialties != null)
            {
                foreach (var specialty in specialties)
                {
                    AddSpecialty(specialty!); // The '!' operator is safe here because validation is done in AddSpecialty
                }
            }

            // Add to chef extent
            chefs.Add(this);
            TotalChefs = chefs.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Chef() : base()
        {
            Specialties = new List<string?>();
        }

        // -------------------------------
        // Override Equals and GetHashCode
        // -------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Chef other)
            {
                if (StaffID != other.StaffID ||
                    Name != other.Name ||
                    ContactNumber != other.ContactNumber)
                    return false;

                if (Specialties.Count != other.Specialties.Count)
                    return false;

                for (int i = 0; i < Specialties.Count; i++)
                {
                    if (Specialties[i] != other.Specialties[i])
                        return false;
                }

                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(StaffID, Name, ContactNumber);
            foreach (var specialty in Specialties)
            {
                hash = HashCode.Combine(hash, specialty);
            }
            return hash;
        }
    }
}
