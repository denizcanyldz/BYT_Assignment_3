using BYT_Assignment_3.Models;

namespace BYT_Assignment_3.Interfaces;

public interface IWaiter
{
    bool TipsCollected { get; set; }
    IReadOnlyList<Order> Orders { get; }
    void AddOrder(Order order);
    void RemoveOrder(Order order);
    void UpdateOrder(Order oldOrder, Order newOrder);
}