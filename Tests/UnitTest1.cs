

[TestFixture]
public class Tests
{
    private const string TestFilePath = "testData.json";

    [SetUp]
    public void SetUp()
    {
        
        Customer.customers.Clear();
        Feedback.feedbacks.Clear();
        Payment.payments.Clear();
        Reservation.reservations.Clear();
        PaymentMethod.paymentMethods.Clear();
    }

    [Test]
    public void Create_Customer_With_Valid_Data_Should_Add_To_CustomerList()
    {
        var customer = new Customer(1, "John Dark", "1234567839", "johnd@example.com");

        Assert.That(Customer.customers.Count, Is.EqualTo(1));
        Assert.That(customer.Name, Is.EqualTo("John Dark"));
    }

    [Test]
    public void CreateCustomer_With_Missing_Name_Should_Throw_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Customer(2, "", "123456789"));
    }

    [Test]
    public void CreateFeedback_Should_Add_To_FeedbackList()
    {
        var customer = new Customer(1, "John Doe", "123456789", "john@example.com");
        var feedback = new Feedback(1, customer, 5, "Great service", DateTime.Now);

        Assert.That(Feedback.feedbacks.Count, Is.EqualTo(1));
        Assert.That(feedback.Rating, Is.EqualTo(5));
    }

    [Test]
    public void AverageRating_When_No_Feedback_Should_Return_Zero()
    {
        var averageRating = Feedback.AverageRating();

        Assert.That(averageRating, Is.EqualTo(0));
    }

    [Test]
    public void AverageRating_With_Multiple_Feedback_Should_Return_Correct_Average()
    {
        var customer = new Customer(1, "John Doe", "123456789", "john@example.com");
        new Feedback(1, customer, 5, "Great service", DateTime.Now);
        new Feedback(2, customer, 3, "Good service", DateTime.Now);

        var averageRating = Feedback.AverageRating();

        Assert.That(averageRating, Is.EqualTo(4));
    }

    [Test]
    public void CreateReservation_With_Valid_Data_Should_Add_To_ReservationList()
    {
        var customer = new Customer(1, "John Doe", "123456789", "john@example.com");
        var reservation = new Reservation(1, customer, DateTime.Now, 4, "Confirmed");

        Assert.That(Reservation.reservations.Count, Is.EqualTo(1));
        Assert.That(reservation.NumberOfGuests, Is.EqualTo(4));
    }

    [Test]
    public void CreateReservation_With_Zero_Guests_Should_Throw_ArgumentException()
    {
        var customer = new Customer(1, "John Doe", "123456789", "john@example.com");

        Assert.Throws<ArgumentException>(() => new Reservation(1, customer, DateTime.Now, 0, "Pending"));
    }

    [Test]
    public void CreatePayment_With_Valid_Data_Should_Add_To_PaymentList()
    {
        var paymentMethod = new PaymentMethod(1, "Credit Card");
        var payment = new Payment(1, 100.50, DateTime.Now, paymentMethod);

        Assert.That(Payment.payments.Count, Is.EqualTo(1));
        Assert.That(payment.Amount, Is.EqualTo(100.50));
    }

    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }

    [Test]
    public void Save_And_Load_Customers_Should_Preserve_Data()
    {
        _ = new Customer(1, "Alice", "1234567890");
        _ = new Customer(2, "Bob", "0987654321");
        CustomerPersistence.SaveCustomers(TestFilePath);

        Customer.customers.Clear(); 
        CustomerPersistence.LoadCustomers(TestFilePath);

        Assert.That(Customer.customers.Count, Is.EqualTo(2));
        Assert.That(Customer.customers[0].Name, Is.EqualTo("Alice"));
        Assert.That(Customer.customers[1].Name, Is.EqualTo("Bob"));
    }

    [Test]
    public void Save_And_Load_Payments_Should_Preserve_Data()
    {
        var paymentMethod = new PaymentMethod(1, "Credit Card");
        _ = new Payment(1, 100.0, DateTime.Now, paymentMethod);
        PaymentPersistence.SavePayments(TestFilePath);

        Payment.payments.Clear(); 
        PaymentPersistence.LoadPayments(TestFilePath);

        Assert.That(Payment.payments.Count, Is.EqualTo(1));
        Assert.That(Payment.payments[0].Amount, Is.EqualTo(100.0));
    }

    [Test]
    public void Save_And_Load_Reservations_Should_Preserve_Data()
    {
        var customer = new Customer(1, "Charlie", "1234567890");
        _ = new Reservation(1, customer, DateTime.Now, 2, "Reserved");
        ReservationPersistence.SaveReservations(TestFilePath);

        Reservation.reservations.Clear(); 
        ReservationPersistence.LoadReservations(TestFilePath);

        Assert.That(Reservation.reservations.Count, Is.EqualTo(1));
        Assert.That(Reservation.reservations[0].Status, Is.EqualTo("Reserved"));
    }

    [Test]
    public void Save_And_Load_PaymentMethods_Should_Preserve_Data()
    {
        _ = new PaymentMethod(1, "PayPal");
        PaymentMethodPersistence.SavePaymentMethods(TestFilePath);

        PaymentMethod.paymentMethods.Clear();
        PaymentMethodPersistence.LoadPaymentMethods(TestFilePath);

        Assert.That(PaymentMethod.paymentMethods.Count, Is.EqualTo(1));
        Assert.That(PaymentMethod.paymentMethods[0].MethodName, Is.EqualTo("PayPal"));
    }
}
