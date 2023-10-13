using Microsoft.AspNetCore.Mvc;
using TicketReservationSystemAPI.Models;
using TicketReservationSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketReservationSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        // GET: api/<ReservationController>
        [HttpGet]
        public ActionResult<List<Reservation>> Get()
        {
            return reservationService.Get();
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(string id)
        {
            var reservation = reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound($"Reservation with id {id} not found");
            }

            return reservation;
        }

        // POST api/<ReservationController>
        [HttpPost]
        public ActionResult<Reservation> Post([FromBody] Reservation train)
        {
            reservationService.Create(train);

            return CreatedAtAction(nameof(Get), new { id = train.Id }, train);
        }

        // PUT api/<ReservationController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Reservation train)
        {
            var existingReservation = reservationService.Get(id);

            if (existingReservation == null)
            {
                return NotFound($"Reservation with id {id} not found");
            }

            reservationService.Update(id, train);

            return NoContent();
        }

        // DELETE api/<ReservationController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var reservation = reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound($"Reservation with id {id} not found");
            }

            reservationService.Remove(reservation.Id);

            return Ok($"Reservation with id {id} deleted");
        }

        // Cancel api/<ReservationController>/cancel/5
        [HttpPut("cancel/{id}")]
        public ActionResult Cancel(string id)
        {
            var reservation = reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound($"Reservation with id {id} not found");
            }

            reservationService.Cancel(reservation.Id);

            return Ok($"Reservation with id {id} cancelled");
        }
    }
}
