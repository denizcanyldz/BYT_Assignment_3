public interface IWaiter
{
    double TipsCollected{get; set;}
    void TakeOrder();
    void ServeOrder();
    void ProcessPayment();
}