using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYT_Assignment_3.Models
{
    [Serializable]
    public class Manager
    {
        // -------------------------------
        // Class/Static Attribute
        // -------------------------------
        private static int totalManagers = 0;

        /// <summary>
        /// Gets or sets the total number of managers.
        /// </summary>
        public static int TotalManagers
        {
            get => totalManagers;
            set
            {
                if (value < 0)
                    throw new ArgumentException("TotalManagers cannot be negative.");
                totalManagers = value;
            }
        }

        // -------------------------------
        // Class Extent
        // -------------------------------
        private static List<Manager> managers = new List<Manager>();

        /// <summary>
        /// Gets a read-only list of all managers.
        /// </summary>
        public static IReadOnlyList<Manager> GetAll()
        {
            return managers.AsReadOnly();
        }

        /// <summary>
        /// Sets the entire manager list (used during deserialization).
        /// </summary>
        public static void SetAll(List<Manager> loadedManagers)
        {
            managers = loadedManagers ?? new List<Manager>();
            TotalManagers = managers.Count;
        }

        // -------------------------------
        // Mandatory Attributes (Simple)
        // -------------------------------
        public int ManagerID { get; private set; }

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
        private string? department;

        public string? Department
        {
            get => department;
            set
            {
                if(!string.IsNullOrEmpty(value) && value.Length > 100)
                    throw new ArgumentException("Department length cannot exceed 100 characters.");
                department = value;
            }
        }

        // -------------------------------
        // Constructors
        // -------------------------------
        /// <summary>
        /// Initializes a new instance of the Manager class with mandatory and optional attributes.
        /// </summary>
        public Manager(int managerID, string name, string? department = null)
        {
            ManagerID = managerID;
            Name = name;
            Department = department;

            // Add to class extent
            managers.Add(this);
            TotalManagers = managers.Count;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Manager() { }
        
        /// <summary>
        /// Determines whether the specified object is equal to the current Manager.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is Manager other)
            {
                return ManagerID == other.ManagerID &&
                       Name == other.Name &&
                       Department == other.Department;
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        public override int GetHashCode()
        {
            return HashCode.Combine(ManagerID, Name, Department);
        }
    }
}