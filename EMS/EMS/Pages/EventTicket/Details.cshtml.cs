using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.EventTicket
{
    public class DetailsModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DetailsModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public Models.EventTicket EventTicket { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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
            return Page();
        }
    }
}
