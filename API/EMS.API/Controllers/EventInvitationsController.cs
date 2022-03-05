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
    public class EventInvitationsController : ControllerBase
    {
        private readonly EventMSContext _context;

        public EventInvitationsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: api/EventInvitations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventInvitation>>> GetEventInvitations()
        {
            return await _context.EventInvitations.ToListAsync();
        }

        // GET: api/EventInvitations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventInvitation>> GetEventInvitation(int id)
        {
            var eventInvitation = await _context.EventInvitations.FindAsync(id);

            if (eventInvitation == null)
            {
                return NotFound();
            }

            return eventInvitation;
        }

        // PUT: api/EventInvitations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventInvitation(int id, EventInvitation eventInvitation)
        {
            if (id != eventInvitation.Id)
            {
                return BadRequest();
            }

            _context.Entry(eventInvitation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventInvitationExists(id))
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

        // POST: api/EventInvitations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventInvitation>> PostEventInvitation(EventInvitation eventInvitation)
        {
            _context.EventInvitations.Add(eventInvitation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventInvitation", new { id = eventInvitation.Id }, eventInvitation);
        }

        // DELETE: api/EventInvitations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventInvitation(int id)
        {
            var eventInvitation = await _context.EventInvitations.FindAsync(id);
            if (eventInvitation == null)
            {
                return NotFound();
            }

            _context.EventInvitations.Remove(eventInvitation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventInvitationExists(int id)
        {
            return _context.EventInvitations.Any(e => e.Id == id);
        }
    }
}
