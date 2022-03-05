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
    public class GroupUsersController : ControllerBase
    {
        private readonly EventMSContext _context;

        public GroupUsersController(EventMSContext context)
        {
            _context = context;
        }

        // GET: api/GroupUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupUser>>> GetGroupUsers()
        {
            return await _context.GroupUsers.ToListAsync();
        }

        // GET: api/GroupUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupUser>> GetGroupUser(int id)
        {
            var groupUser = await _context.GroupUsers.FindAsync(id);

            if (groupUser == null)
            {
                return NotFound();
            }

            return groupUser;
        }

        // PUT: api/GroupUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupUser(int id, GroupUser groupUser)
        {
            if (id != groupUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(groupUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupUserExists(id))
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

        // POST: api/GroupUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GroupUser>> PostGroupUser(GroupUser groupUser)
        {
            _context.GroupUsers.Add(groupUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupUser", new { id = groupUser.Id }, groupUser);
        }

        // DELETE: api/GroupUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupUser(int id)
        {
            var groupUser = await _context.GroupUsers.FindAsync(id);
            if (groupUser == null)
            {
                return NotFound();
            }

            _context.GroupUsers.Remove(groupUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupUserExists(int id)
        {
            return _context.GroupUsers.Any(e => e.Id == id);
        }
    }
}
