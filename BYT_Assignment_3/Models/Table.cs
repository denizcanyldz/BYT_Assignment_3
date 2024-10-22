namespace BYT_Assignment_3;

public class Table
{
    public static List<Table> Tables = new List<Table>();

    public int TableNumber { get; set; }
    public int MaxSeats { get; set; }
    public bool IsReserved { get; set; }

    public Reservation CurrentReservation { get; set; }

    public Table(int tableNumber, int maxSeats, bool isReserved = false)
    {
        if (maxSeats <= 0)
        {
            throw new ArgumentException("Max seats must be greater than zero.");
        }

        TableNumber = tableNumber;
        MaxSeats = maxSeats;
        IsReserved = isReserved;

        Tables.Add(this);
    }
}
