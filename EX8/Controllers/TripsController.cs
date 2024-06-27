using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EX8.Models;
using System.Linq;
using System.Threading.Tasks;

namespace EX8.Controllers
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
            var tripsQuery = _context.Trips
                .OrderByDescending(t => t.DateFrom)
                .Include(t => t.ClientTrips)
                    .ThenInclude(ct => ct.IdClientNavigation)
                .Include(t => t.IdCountries);

            var totalTrips = await tripsQuery.CountAsync();
            var trips = await tripsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                pageNum = page,
                pageSize,
                allPages = (int)Math.Ceiling(totalTrips / (double)pageSize),
                trips
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip([FromBody] Trip trip)
        {
            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrips), new { id = trip.IdTrip }, trip);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientDto clientDto)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null || trip.DateFrom <= DateTime.Now)
            {
                return BadRequest("Trip does not exist or has already started");
            }

            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
            if (existingClient == null)
            {
                existingClient = new Client
                {
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    Email = clientDto.Email,
                    Telephone = clientDto.Telephone,
                    Pesel = clientDto.Pesel
                };
                _context.Clients.Add(existingClient);
                await _context.SaveChangesAsync();
            }

            var isClientRegistered = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);
            if (isClientRegistered)
            {
                return BadRequest("Client is already registered for this trip");
            }

            var clientTrip = new ClientTrip
            {
                IdClient = existingClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDto.PaymentDate
            };

            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTrips), new { id = existingClient.IdClient }, clientTrip);
        }
    }
}
