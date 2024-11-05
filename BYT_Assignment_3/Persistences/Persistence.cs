using System;
using System.IO;
using System.Xml.Serialization;

namespace BYT_Assignment_3.Persistences
{
    public static class Persistence
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Extents));

        /// <summary>
        /// Saves all class extents to the specified XML file.
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <param name="extents">Extents object containing all class extents.</param>
        public static void SaveAll(string filePath, Extents extents)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, extents);
                }
                Console.WriteLine($"Successfully saved data to {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data to {filePath}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Loads all class extents from the specified XML file.
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <returns>Extents object containing all class extents.</returns>
        public static Extents LoadAll(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File {filePath} does not exist. Returning empty extents.");
                    return new Extents();
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    var extents = (Extents)serializer.Deserialize(reader);
                    Console.WriteLine($"Successfully loaded data from {filePath}.");
                    return extents ?? new Extents();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data from {filePath}: {ex.Message}");
                return new Extents();
            }
        }
    }
}
