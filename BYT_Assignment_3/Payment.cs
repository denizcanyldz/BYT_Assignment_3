public class Payment
{
    public static List<Payment> payments = new List<Payment>();

    public int PaymentID { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public Payment(int paymentID, double amount, DateTime date, PaymentMethod paymentMethod)
    {
        PaymentID = paymentID;
        Amount = amount;
        Date = date;
        PaymentMethod = paymentMethod;

        payments.Add(this);
    }
}

