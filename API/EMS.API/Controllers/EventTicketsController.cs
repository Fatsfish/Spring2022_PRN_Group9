using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.API.Models;

namespace EMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventTicketsController : ControllerBase
    {
        private readonly EventMSContext _context;

        public EventTicketsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: api/EventTickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventTicket>>> GetEventTickets()
        {
            return await _context.EventTickets.ToListAsync();
        }

        // GET: api/EventTickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventTicket>> GetEventTicket(int id)
        {
            var eventTicket = await _context.EventTickets.FindAsync(id);

            if (eventTicket == null)
            {
                return NotFound();
            }

            return eventTicket;
        }

        // PUT: api/EventTickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventTicket(int id, EventTicket eventTicket)
        {
            if (id != eventTicket.Id)
            {
                return BadRequest();
            }

            _context.Entry(eventTicket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventTicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EventTickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventTicket>> PostEventTicket(EventTicket eventTicket)
        {
            _context.EventTickets.Add(eventTicket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventTicket", new { id = eventTicket.Id }, eventTicket);
        }

        // DELETE: api/EventTickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventTicket(int id)
        {
            var eventTicket = await _context.EventTickets.FindAsync(id);
            if (eventTicket == null)
            {
                return NotFound();
            }

            _context.EventTickets.Remove(eventTicket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventTicketExists(int id)
        {
            return _context.EventTickets.Any(e => e.Id == id);
        }
    }
}
