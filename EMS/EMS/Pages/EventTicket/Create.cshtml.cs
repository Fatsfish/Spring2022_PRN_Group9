using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Models;

namespace EMS.Pages.EventTicket
{
    public class CreateModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public CreateModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio");
            return Page();
        }

        [BindProperty]
        public Models.EventTicket EventTicket { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.EventTickets.Add(EventTicket);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
