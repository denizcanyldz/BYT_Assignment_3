using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using BYT_Assignment_3.Interfaces;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Chef : Staff, IRole, IChef
    {
        public string RoleName => "Chef";

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

        // New: Collection of MenuItems prepared by the Chef
        [XmlIgnore] // Prevent direct serialization to avoid circular references
        public List<MenuItem> MenuItems { get; private set; } = new List<MenuItem>();

        // For serialization purposes, convert the collection to a list of MenuItemIDs
        [XmlArray("MenuItems")]
        [XmlArrayItem("MenuItemID")]
        public List<int> MenuItemIDs
        {
            get => MenuItems.Select(mi => mi.MenuItemID).ToList();
            set
            {
                // This property is used during deserialization to associate MenuItems
                // The actual MenuItem objects should be linked after all objects are deserialized
            }
        }

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

        /// <summary>
        /// Adds a MenuItem to the chef's collection.
        /// </summary>
        /// <param name="menuItem">The MenuItem to add.</param>
        public void AddMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem), "MenuItem cannot be null.");
            if (MenuItems.Contains(menuItem))
                throw new ArgumentException("MenuItem already exists for this chef.");

            MenuItems.Add(menuItem);
            menuItem.SetChef(this); // Ensure bidirectional association
        }

        /// <summary>
        /// Removes a MenuItem from the chef's collection.
        /// </summary>
        /// <param name="menuItem">The MenuItem to remove.</param>
        public void RemoveMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem), "MenuItem cannot be null.");
            if (!MenuItems.Contains(menuItem))
                throw new ArgumentException("MenuItem not found for this chef.");

            MenuItems.Remove(menuItem);
            menuItem.RemoveChef(); // Ensure bidirectional association
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
            MenuItems = new List<MenuItem>();
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
