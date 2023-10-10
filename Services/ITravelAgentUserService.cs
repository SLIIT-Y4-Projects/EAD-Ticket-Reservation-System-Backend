using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public interface ITravelAgentUserService
    {
        List<TravelAgentUser> Get();
        TravelAgentUser Get(string id);
        TravelAgentUser GetByUsername(string username);
        TravelAgentUser Create(TravelAgentUser travelAgentUser);
        void Update(string id, TravelAgentUser travelAgentUser);
        void Remove(string id);
    }
}
