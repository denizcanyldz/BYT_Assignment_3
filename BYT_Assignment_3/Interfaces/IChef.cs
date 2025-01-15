using BYT_Assignment_3.Models;

namespace BYT_Assignment_3.Interfaces;

public interface IChef
{
    List<string?> Specialties { get; set; }
    List<MenuItem> MenuItems { get; }
    void AddSpecialty(string specialty);
    void RemoveSpecialty(string specialty);
    void AddMenuItem(MenuItem menuItem);
    void RemoveMenuItem(MenuItem menuItem);
}