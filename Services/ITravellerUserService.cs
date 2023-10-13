/**
 * @file ITravellerUserService.cs
 * @brief Interface for TravellerUser service
 */
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public interface ITravellerUserService
    {
        List<TravellerUser> Get();
        TravellerUser Get(string id);
        TravellerUser GetByUsername(string username);
        TravellerUser Create(TravellerUser travellerUser);
        void Update(string id, TravellerUser travellerUser);
        void Remove(string id);
        void Activate(string id);
        void Deactivate(string id);
    }
}
