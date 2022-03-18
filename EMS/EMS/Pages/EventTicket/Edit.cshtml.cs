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

namespace EMS.Pages.EventTicket
{
    public class EditModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public EditModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.EventTicket EventTicket { get; set; }

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

                EventTicket = await _context.EventTickets
                    .Include(e => e.Event)
                    .Include(e => e.Owner).FirstOrDefaultAsync(m => m.Id == id);

                if (EventTicket == null)
                {
                    return NotFound();
                }
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
                ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio");
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
            EventTicket.PaidDate = System.DateTime.Now;
            if (EventTicket.EventId == null) return Page();
            if (EventTicket.OwnerId == null) return Page();
            _context.Attach(EventTicket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventTicketExists(EventTicket.Id))
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

        private bool EventTicketExists(int id)
        {
            return _context.EventTickets.Any(e => e.Id == id);
        }
    }
}
