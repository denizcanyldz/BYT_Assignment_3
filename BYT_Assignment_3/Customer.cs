public class Customer
{
    public static List<Customer> customers = new List<Customer>();

    public int CustomerID { get; set; }
    public required string Name { get; set; }
    public required string ContactNumber { get; set; }
    public string? Email { get; set; }
    public List<Reservation> Reservations{get;set;} = new List<Reservation>();

    public Customer(int customerID, string name, string contactNumber, string? email = null)
    {
        if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(contactNumber)){
            throw new ArgumentException("Name and contact number should be provided.");
        }
        CustomerID = customerID;
        Name = name;
        ContactNumber = contactNumber;
        Email = email;

        customers.Add(this);

    }


}