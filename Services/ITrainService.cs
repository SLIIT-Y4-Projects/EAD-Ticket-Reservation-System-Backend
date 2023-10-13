/**
 * @file ITrainService.cs
 * @brief Interface for Train service
 */
using TicketReservationSystemAPI.Models;

namespace TicketReservationSystemAPI.Services
{
    public interface ITrainService
    {
        List<Train> Get();
        Train Get(string id);
        Train Create(Train train);
        void Update(string id, Train train);
        void Remove(string id);
        void Activate(string id);
        void Cancel(string id);
    }
}
