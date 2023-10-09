using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public interface IBackOfficeUserService
    {
        List<BackOfficeUser> Get();
        BackOfficeUser Get(string id);
        BackOfficeUser Create(BackOfficeUser backOfficeUser);
        void Update(string id, BackOfficeUser backOfficeUser);
        void Remove(string id);
    }
}
