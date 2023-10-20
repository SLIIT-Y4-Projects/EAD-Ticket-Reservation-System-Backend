/**
 * @file TravellerUserController.cs
 * @brief Controller for TravellerUser
 */
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
    public class TravellerUserController : ControllerBase
    {
        private readonly ITravellerUserService travellerUserService;
        private readonly IConfiguration _configuration;

        public TravellerUserController(ITravellerUserService travellerUserService, IConfiguration configuration)
        {
            this.travellerUserService = travellerUserService;
            _configuration = configuration;
        }

        // Generate JWT token
        private string CreateToken(TravellerUser travellerUser)
        {
            // ID and role add to claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, travellerUser.Id)
            };

            // Add TRAVELLER role to claims
            claims.Add(new Claim(IdentityData.TravellerClaimName, "true"));

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

        // GET: api/<TravellerUserController>
        [HttpGet]
        public ActionResult<List<TravellerUser>> Get()
        {
            return travellerUserService.Get();
        }

        // GET api/<TravellerUserController>/5
        [HttpGet("{id}")]
        public ActionResult<TravellerUser> Get(string id)
        {
            var travellerUser = travellerUserService.Get(id);

            if (travellerUser == null)
            {
                return NotFound($"TravellerUser with Id = {id} not found");
            }

            return travellerUser;
        }

        // POST api/<TravellerUserController>/register
        [HttpPost("register")]
        public ActionResult<TravellerUser> Register([FromBody] TravellerUser request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = passwordHash;

            travellerUserService.Create(request);

            return CreatedAtAction(nameof(Get), new { id = request.Id }, request);
        }

        // POST api/<TravellerUserController>/login
        [HttpPost("login")]
        public ActionResult<TravellerUser> Login([FromBody] TravellerUser request)
        {
            var travellerUser = travellerUserService.GetByUsername(request.Username);

            if (travellerUser == null)
            {
                return BadRequest("Invalid Credentials");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, travellerUser.Password);

            if (!verified)
            {
                return BadRequest("Invalid Credentials");
            }

            if (travellerUser.Status == "INACTIVE")
            {
                return BadRequest("Account is inactive please contact back office");
            }

            string jwt = CreateToken(travellerUser);

            return Ok(new
            {
                Id = travellerUser.Id,
                Username = travellerUser.Username,
                FullName = travellerUser.FullName,
                Token = jwt,
                Role = IdentityData.TravellerClaimName
            });
        }

        // PUT api/<TravellerUserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] TravellerUser travellerUser)
        {
            var existingTravellerUser = travellerUserService.Get(id);

            if (existingTravellerUser == null)
            {
                return NotFound($"TravellerUser with Id = {id} not found");
            }

            travellerUserService.Update(id, travellerUser);

            return NoContent();
        }

        // DELETE api/<TravellerUserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var travellerUser = travellerUserService.Get(id);

            if (travellerUser == null)
            {
                return NotFound($"TravellerUser with Id = {id} not found");
            }

            travellerUserService.Remove(travellerUser.Id);

            return Ok($"TravellerUser with Id = {id} deleted");
        }

        [HttpPut("activate/{id}")]
        public ActionResult Activate(string id)
        {
            var travellerUser = travellerUserService.Get(id);

            if (travellerUser == null)
            {
                return NotFound($"TravellerUser with Id = {id} not found");
            }

            travellerUserService.Activate(travellerUser.Id);

            return NoContent();
        }

        [HttpPut("deactivate/{id}")]
        public ActionResult Deactivate(string id)
        {
            var travellerUser = travellerUserService.Get(id);

            if (travellerUser == null)
            {
                return NotFound($"TravellerUser with Id = {id} not found");
            }

            travellerUserService.Deactivate(travellerUser.Id);

            return NoContent();
        }
    }
}
