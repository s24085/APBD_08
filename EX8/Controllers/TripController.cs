using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EX8.Models;


namespace EX8.Controllers // Zmień na rzeczywiste namespace Twoich kontrolerów
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly TripsDbContext _context;

        public TripsController(TripsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var trips = await _context.Trips
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation) // Ładowanie klientów
                .Include(t => t.IdCountries) // Ładowanie krajów
                .ToListAsync();

            var totalTrips = await _context.Trips.CountAsync();

            return Ok(new
            {
                pageNum = page,
                pageSize,
                allPages = (int)Math.Ceiling(totalTrips / (double)pageSize),
                trips
            });
        }

        [HttpDelete("/api/clients/{idClient}")]
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

        [HttpPost("/api/trips/{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientDto clientDto)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now)
            {
                return BadRequest("Trip does not exist or has already started");
            }

            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
            if (existingClient != null)
            {
                return BadRequest("Client with this PESEL already exists");
            }

            var isClientRegistered = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);
            if (isClientRegistered)
            {
                return BadRequest("Client is already registered for this trip");
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

            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrips), new { id = client.IdClient }, client);
        }
    }
}
