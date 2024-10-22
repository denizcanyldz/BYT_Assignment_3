public class PaymentMethod
{
    public static List<PaymentMethod> paymentMethods = new List<PaymentMethod>();

    public int MethodID { get; set; }
    public string MethodName { get; set; }

    public PaymentMethod(int methodID, string methodName)
    {
        MethodID = methodID;
        MethodName = methodName;

        paymentMethods.Add(this);
    }
}
