/**
 * @file IReservationService.cs
 * @brief Interface for Reservation service
 */
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public interface IReservationService
    {
        List<Reservation> Get();
        Reservation Get(string id);
        Reservation Create(Reservation reservation);
        void Update(string id, Reservation reservation);
        void Remove(string id);
        void Cancel(string id);
    }
}
