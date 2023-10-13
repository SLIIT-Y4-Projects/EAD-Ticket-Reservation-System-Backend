using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketReservationSystemAPI.Identity;
using TicketReservationSystemAPI.Models;
using TicketReservationSystemAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TicketReservationSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelAgentUserController : ControllerBase
    {
        private readonly ITravelAgentUserService travelAgentUserService;
        private readonly IConfiguration _configuration;

        public TravelAgentUserController(ITravelAgentUserService travelAgentUserService, IConfiguration configuration)
        {
            this.travelAgentUserService = travelAgentUserService;
            _configuration = configuration;
        }

        // Generate JWT token
        private string CreateToken(TravelAgentUser travelAgentUser)
        {
            // ID and role add to claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, travelAgentUser.Id)
            };

            // Add TRAVEL_AGENT role to claims
            claims.Add(new Claim(IdentityData.TravelAgentClaimName, "true"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                               _configuration.GetSection("JwtSettings:Key").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration.GetSection("JwtSettings:Issuer").Value!,
                _configuration.GetSection("JwtSettings:Audience").Value!,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        // GET: api/<TravelAgentUserController>
        [HttpGet]
        public ActionResult<List<TravelAgentUser>> Get()
        {
            return travelAgentUserService.Get();
        }

        // GET api/<TravelAgentUserController>/5
        [HttpGet("{id}")]
        public ActionResult<TravelAgentUser> Get(string id)
        {
            var travelAgentUser = travelAgentUserService.Get(id);

            if (travelAgentUser == null)
            {
                return NotFound($"TravelAgentUser with Id = {id} not found");
            }

            return travelAgentUser;
        }

        // POST api/<TravelAgentUserController>/register
        [HttpPost("register")]
        public ActionResult<TravelAgentUser> Register([FromBody] TravelAgentUser request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = passwordHash;

            travelAgentUserService.Create(request);

            return CreatedAtAction(nameof(Get), new { id = request.Id }, request);
        }

        // POST api/<TravelAgentUserController>/login
        [HttpPost("login")]
        public ActionResult<TravelAgentUser> Login([FromBody] TravelAgentUser request)
        {
            var travelAgentUser = travelAgentUserService.GetByUsername(request.Username);

            if (travelAgentUser == null)
            {
                return BadRequest("Invalid Credentials");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, travelAgentUser.Password);

            if (!verified)
            {
                return BadRequest("Invalid Credentials");
            }

            string jwt = CreateToken(travelAgentUser);

            return Ok(new
            {
                Id = travelAgentUser.Id,
                Username = travelAgentUser.Username,
                FullName = travelAgentUser.FullName,
                Token = jwt,
                Role = IdentityData.TravelAgentClaimName
            });
        }

        // PUT api/<TravelAgentUserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] TravelAgentUser travelAgentUser)
        {
            var existingTravelAgentUser = travelAgentUserService.Get(id);

            if (existingTravelAgentUser == null)
            {
                return NotFound($"TravelAgentUser with Id = {id} not found");
            }

            travelAgentUserService.Update(id, travelAgentUser);

            return NoContent();
        }

        // DELETE api/<TravelAgentUserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var travelAgentUser = travelAgentUserService.Get(id);

            if (travelAgentUser == null)
            {
                return NotFound($"TravelAgentUser with Id = {id} not found");
            }

            travelAgentUserService.Remove(travelAgentUser.Id);

            return Ok($"TravelAgentUser with Id = {id} deleted");
        }
    }
}
