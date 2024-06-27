using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EX8.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EX8.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly TripsDbContext _context;

        public ClientsController(TripsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients);
        }

        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var client = await _context.Clients
                .Include(c => c.ClientTrips)
                .FirstOrDefaultAsync(c => c.IdClient == idClient);

            if (client == null)
            {
                return NotFound("Client not found");
            }

            if (client.ClientTrips.Any())
            {
                return BadRequest("Client has assigned trips and cannot be deleted");
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto)
        {
            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
            if (existingClient != null)
            {
                return BadRequest("Client with this PESEL already exists");
            }

            var client = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClients), new { id = client.IdClient }, client);
        }
    }
}
