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
    public class InvitationResponseTypesController : ControllerBase
    {
        private readonly EventMSContext _context;

        public InvitationResponseTypesController(EventMSContext context)
        {
            _context = context;
        }

        // GET: api/InvitationResponseTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvitationResponseType>>> GetInvitationResponseTypes()
        {
            return await _context.InvitationResponseTypes.ToListAsync();
        }

        // GET: api/InvitationResponseTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvitationResponseType>> GetInvitationResponseType(int id)
        {
            var invitationResponseType = await _context.InvitationResponseTypes.FindAsync(id);

            if (invitationResponseType == null)
            {
                return NotFound();
            }

            return invitationResponseType;
        }

        // PUT: api/InvitationResponseTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvitationResponseType(int id, InvitationResponseType invitationResponseType)
        {
            if (id != invitationResponseType.Id)
            {
                return BadRequest();
            }

            _context.Entry(invitationResponseType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvitationResponseTypeExists(id))
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

        // POST: api/InvitationResponseTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InvitationResponseType>> PostInvitationResponseType(InvitationResponseType invitationResponseType)
        {
            _context.InvitationResponseTypes.Add(invitationResponseType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvitationResponseType", new { id = invitationResponseType.Id }, invitationResponseType);
        }

        // DELETE: api/InvitationResponseTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvitationResponseType(int id)
        {
            var invitationResponseType = await _context.InvitationResponseTypes.FindAsync(id);
            if (invitationResponseType == null)
            {
                return NotFound();
            }

            _context.InvitationResponseTypes.Remove(invitationResponseType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InvitationResponseTypeExists(int id)
        {
            return _context.InvitationResponseTypes.Any(e => e.Id == id);
        }
    }
}
