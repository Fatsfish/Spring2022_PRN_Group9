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
    public class EventStatusController : ControllerBase
    {
        private readonly EventMSContext _context;

        public EventStatusController(EventMSContext context)
        {
            _context = context;
        }

        // GET: api/EventStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventStatus>>> GetEventStatuses()
        {
            return await _context.EventStatuses.ToListAsync();
        }

        // GET: api/EventStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventStatus>> GetEventStatus(int id)
        {
            var eventStatus = await _context.EventStatuses.FindAsync(id);

            if (eventStatus == null)
            {
                return NotFound();
            }

            return eventStatus;
        }

        // PUT: api/EventStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventStatus(int id, EventStatus eventStatus)
        {
            if (id != eventStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(eventStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventStatusExists(id))
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

        // POST: api/EventStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventStatus>> PostEventStatus(EventStatus eventStatus)
        {
            _context.EventStatuses.Add(eventStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventStatus", new { id = eventStatus.Id }, eventStatus);
        }

        // DELETE: api/EventStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventStatus(int id)
        {
            var eventStatus = await _context.EventStatuses.FindAsync(id);
            if (eventStatus == null)
            {
                return NotFound();
            }

            _context.EventStatuses.Remove(eventStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventStatusExists(int id)
        {
            return _context.EventStatuses.Any(e => e.Id == id);
        }
    }
}
