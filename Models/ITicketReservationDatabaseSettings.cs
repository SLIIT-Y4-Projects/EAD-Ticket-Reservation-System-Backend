namespace TicketReservationSystemAPI.Models
{
    public interface ITicketReservationDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
