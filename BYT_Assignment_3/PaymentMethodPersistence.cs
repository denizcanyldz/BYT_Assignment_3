public class PaymentMethodPersistence : Persistence<PaymentMethod>
{
    public static void SavePaymentMethods(string filePath)
    {
        Save(filePath, PaymentMethod.paymentMethods);
    }

    public static void LoadPaymentMethods(string filePath)
    {
        PaymentMethod.paymentMethods = Load(filePath);
    }
}
