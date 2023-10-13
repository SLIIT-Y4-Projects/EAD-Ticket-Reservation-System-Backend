/**
 * @file TicketReservationDatabaseSettings.cs
 * @brief This file contains the TicketReservationDatabaseSettings class.
 */
namespace TicketReservationSystemAPI.Models
{
    public class TicketReservationDatabaseSettings : ITicketReservationDatabaseSettings
    {
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
    }
}
