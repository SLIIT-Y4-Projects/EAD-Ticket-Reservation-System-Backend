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
    public class BackOfficeUserController : ControllerBase
    {
        private readonly IBackOfficeUserService backOfficeUserService;
        private readonly IConfiguration _configuration;

        public BackOfficeUserController(IBackOfficeUserService backOfficeUserService, IConfiguration configuration)
        {
            this.backOfficeUserService = backOfficeUserService;
            _configuration = configuration;
        }

        // Generate JWT token
        private string CreateToken(BackOfficeUser backOfficeUser)
        {
            // ID and role add to claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, backOfficeUser.Id)
            };

            // Add BACK_OFFICE role to claims
            claims.Add(new Claim(IdentityData.BackOfficeClaimName, "true"));

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

        // GET: api/<BackOfficeUserController>
        [HttpGet]
        public ActionResult<List<BackOfficeUser>> Get()
        {
            return backOfficeUserService.Get();
        }

        // GET api/<BackOfficeUserController>/5
        [HttpGet("{id}")]
        public ActionResult<BackOfficeUser> Get(string id)
        {
            var backOfficeUser = backOfficeUserService.Get(id);

            if (backOfficeUser == null)
            {
                return NotFound($"BackOfficeUser with Id = {id} not found");
            }

            return backOfficeUser;
        }

        // POST api/<BackOfficeUserController>/register
        [HttpPost("register")]
        public ActionResult<BackOfficeUser> Register([FromBody] BackOfficeUser request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            request.Password = passwordHash;

            backOfficeUserService.Create(request);

            return CreatedAtAction(nameof(Get), new { id = request.Id }, request);
        }

        // POST api/<BackOfficeUserController>/login
        [HttpPost("login")]
        public ActionResult<BackOfficeUser> Login([FromBody] BackOfficeUser request)
        {
            var backOfficeUser = backOfficeUserService.GetByUsername(request.Username);

            if (backOfficeUser == null)
            {
                return BadRequest("Invalid Credentials");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, backOfficeUser.Password);

            if (!verified)
            {
                return BadRequest("Invalid Credentials");
            }

            string jwt = CreateToken(backOfficeUser);

            return Ok(jwt);
        }

        // PUT api/<BackOfficeUserController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] BackOfficeUser backOfficeUser)
        {
            var existingBackOfficeUser = backOfficeUserService.Get(id);

            if (existingBackOfficeUser == null)
            {
                return NotFound($"BackOfficeUser with Id = {id} not found");
            }

            backOfficeUserService.Update(id, backOfficeUser);

            return NoContent();
        }

        // DELETE api/<BackOfficeUserController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var backOfficeUser = backOfficeUserService.Get(id);

            if (backOfficeUser == null)
            {
                return NotFound($"BackOfficeUser with Id = {id} not found");
            }

            backOfficeUserService.Remove(backOfficeUser.Id);

            return Ok($"BackOfficeUser with Id = {id} deleted");
        }
    }
}
