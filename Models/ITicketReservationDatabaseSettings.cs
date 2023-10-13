/**
 * @file TicketReservationDatabaseSettings.cs
 * @brief This file contains the TicketReservationDatabaseSettings interface.
 */
namespace TicketReservationSystemAPI.Models
{
    public interface ITicketReservationDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
