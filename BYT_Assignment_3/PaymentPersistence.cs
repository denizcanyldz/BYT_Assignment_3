public class PaymentPersistence : Persistence<Payment>
{
    public static void SavePayments(string filePath)
    {
        Save(filePath, Payment.payments);
    }

    public static void LoadPayments(string filePath)
    {
        Payment.payments = Load(filePath);
    }
}
