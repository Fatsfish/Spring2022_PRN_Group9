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
    public class AllowedEventGroupsController : ControllerBase
    {
        private readonly EventMSContext _context;

        public AllowedEventGroupsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: api/AllowedEventGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllowedEventGroup>>> GetAllowedEventGroups()
        {
            return await _context.AllowedEventGroups.ToListAsync();
        }

        // GET: api/AllowedEventGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AllowedEventGroup>> GetAllowedEventGroup(int id)
        {
            var allowedEventGroup = await _context.AllowedEventGroups.FindAsync(id);

            if (allowedEventGroup == null)
            {
                return NotFound();
            }

            return allowedEventGroup;
        }

        // PUT: api/AllowedEventGroups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllowedEventGroup(int id, AllowedEventGroup allowedEventGroup)
        {
            if (id != allowedEventGroup.Id)
            {
                return BadRequest();
            }

            _context.Entry(allowedEventGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllowedEventGroupExists(id))
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

        // POST: api/AllowedEventGroups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AllowedEventGroup>> PostAllowedEventGroup(AllowedEventGroup allowedEventGroup)
        {
            _context.AllowedEventGroups.Add(allowedEventGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllowedEventGroup", new { id = allowedEventGroup.Id }, allowedEventGroup);
        }

        // DELETE: api/AllowedEventGroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllowedEventGroup(int id)
        {
            var allowedEventGroup = await _context.AllowedEventGroups.FindAsync(id);
            if (allowedEventGroup == null)
            {
                return NotFound();
            }

            _context.AllowedEventGroups.Remove(allowedEventGroup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AllowedEventGroupExists(int id)
        {
            return _context.AllowedEventGroups.Any(e => e.Id == id);
        }
    }
}
