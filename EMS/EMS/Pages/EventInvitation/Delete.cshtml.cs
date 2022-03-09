using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.EventInvitation
{
    public class DeleteModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DeleteModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.EventInvitation EventInvitation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EventInvitation = await _context.EventInvitations
                .Include(e => e.Event)
                .Include(e => e.InvitationResponse)
                .Include(e => e.User).FirstOrDefaultAsync(m => m.Id == id);

            if (EventInvitation == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EventInvitation = await _context.EventInvitations.FindAsync(id);

            if (EventInvitation != null)
            {
                _context.EventInvitations.Remove(EventInvitation);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
