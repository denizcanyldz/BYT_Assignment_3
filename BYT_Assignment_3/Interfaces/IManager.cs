using BYT_Assignment_3.Models;

namespace BYT_Assignment_3.Interfaces;

public interface IManager
{
    string? Department { get; set; }
    void AssignStaff(Staff staff);
    void RemoveStaff(Staff staff);
}