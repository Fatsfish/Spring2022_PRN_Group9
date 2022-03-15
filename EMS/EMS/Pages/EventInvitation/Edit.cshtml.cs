using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.Models;
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.EventInvitation
{
    public class EditModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public EditModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.EventInvitation EventInvitation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToPage("/Login");
            }
            if (HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role2") != null)
            {
                return RedirectToPage("/Index");
            }
            else
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
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
                ViewData["InvitationResponseId"] = new SelectList(_context.InvitationResponseTypes, "Id", "Name");
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio");
                return Page();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(EventInvitation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventInvitationExists(EventInvitation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool EventInvitationExists(int id)
        {
            return _context.EventInvitations.Any(e => e.Id == id);
        }
    }
}
