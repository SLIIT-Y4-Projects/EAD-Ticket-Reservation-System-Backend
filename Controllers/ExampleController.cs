using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystemAPI.Identity;
using TicketReservationSystemAPI.Models;
using TicketReservationSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketReservationSystemAPI.Controllers
{
    [Authorize(Policy = IdentityData.BackOfficePolicyName)] // Only users with the BACK_OFFICE claim can access this controller
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly IExampleService exampleService;

        public ExampleController(IExampleService exampleService)
        {
            this.exampleService = exampleService;
        }

        // GET: api/<ExampleController>
        [AllowAnonymous] // Allow anonymous access to this endpoint (no JWT required)
        [HttpGet]
        public ActionResult<List<Example>> Get()
        {
            return exampleService.Get();
        }

        // GET api/<ExampleController>/5
        [HttpGet("{id}")]
        public ActionResult<Example> Get(string id)
        {
            var example = exampleService.Get(id);

            if (example == null)
            {
                return NotFound($"Example with id {id} not found");
            }

            return example;
        }

        // POST api/<ExampleController>
        [HttpPost]
        public ActionResult<Example> Post([FromBody] Example example)
        {
            exampleService.Create(example);

            return CreatedAtAction(nameof(Get), new { id = example.Id }, example);
        }

        // PUT api/<ExampleController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Example example)
        {
            var existingStudent = exampleService.Get(id);

            if (existingStudent == null)
            {
                return NotFound($"Example with id {id} not found");
            }

            exampleService.Update(id, example);

            return NoContent();
        }

        // DELETE api/<ExampleController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var student = exampleService.Get(id);

            if (student == null)
            {
                return NotFound($"Example with id {id} not found");
            }

            exampleService.Remove(student.Id);

            return Ok($"Example with id {id} deleted");
        }
    }
}
