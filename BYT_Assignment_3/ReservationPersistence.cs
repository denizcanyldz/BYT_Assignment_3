public class ReservationPersistence : Persistence<Reservation>
{
    public static void SaveReservations(string filePath)
    {
        Save(filePath, Reservation.reservations);
    }

    public static void LoadReservations(string filePath)
    {
        Reservation.reservations = Load(filePath);
    }
}
