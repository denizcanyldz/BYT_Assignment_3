

public class Reservation
{
    public static List<Reservation> reservations = new List<Reservation>();

    public int ReservationID {get;set;}
    public Customer Customer{get;set;}

    //public Table Table {get;set;}
    public DateTime ReservationTime{get;set;}
    public int NumberOfGuests{get;set;}
    public string Status{get;set;}
                                                        //Table table add
    public Reservation(int reservationID, Customer customer, DateTime reservationTime, int numberOfGuests, string status){
        if(numberOfGuests <=0){
            throw new ArgumentException("The number of guests should be at least 1.");
        }
        ReservationID = reservationID;
        Customer = customer;
        //Table = table;
        ReservationTime = reservationTime;
        NumberOfGuests = numberOfGuests;
        Status = status;

        reservations.Add(this);
    }

}