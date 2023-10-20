/**
 * @file TrainController.cs
 * @brief Controller for Train
 */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystemAPI.Models;
using TicketReservationSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketReservationSystemAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrainController : ControllerBase
    {
        private readonly ITrainService trainService;

        public TrainController(ITrainService trainService)
        {
            this.trainService = trainService;
        }

        // GET: api/<TrainController>
        [HttpGet]
        public ActionResult<List<Train>> Get()
        {
            return trainService.Get();
        }

        // GET api/<TrainController>/5
        [HttpGet("{id}")]
        public ActionResult<Train> Get(string id)
        {
            var train = trainService.Get(id);

            if (train == null)
            {
                return NotFound($"Train with id {id} not found");
            }

            return train;
        }

        // POST api/<TrainController>
        [HttpPost]
        public ActionResult<Train> Post([FromBody] Train train)
        {
            trainService.Create(train);

            return CreatedAtAction(nameof(Get), new { id = train.Id }, train);
        }

        // PUT api/<TrainController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Train train)
        {
            var existingTrain = trainService.Get(id);

            if (existingTrain == null)
            {
                return NotFound($"Train with id {id} not found");
            }

            trainService.Update(id, train);

            return NoContent();
        }

        // DELETE api/<TrainController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var train = trainService.Get(id);

            if (train == null)
            {
                return NotFound($"Train with id {id} not found");
            }

            trainService.Remove(train.Id);

            return Ok($"Train with id {id} deleted");
        }

        // Activate api/<TrainController>/activate/5
        [HttpPut("activate/{id}")]
        public ActionResult Activate(string id)
        {
            var train = trainService.Get(id);

            if (train == null)
            {
                return NotFound($"Train with id {id} not found");
            }

            trainService.Activate(train.Id);

            return Ok($"Train with id {id} activated");
        }

        // Cancel api/<TrainController>/cancel/5
        [HttpPut("cancel/{id}")]
        public ActionResult Cancel(string id)
        {
            var train = trainService.Get(id);

            if (train == null)
            {
                return NotFound($"Train with id {id} not found");
            }

            trainService.Cancel(train.Id);

            return Ok($"Train with id {id} cancelled");
        }
    }
}
