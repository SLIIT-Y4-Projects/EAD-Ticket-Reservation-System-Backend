/**
 * @file IExampleService.cs
 * @brief Interface for Example service
 */
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public interface IExampleService
    {
        List<Example> Get();
        Example Get(string id);
        Example Create(Example example);
        void Update(string id, Example example);
        void Remove(string id);
    }
}
