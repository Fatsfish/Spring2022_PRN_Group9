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
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.EventTicket> EventTicket { get;set; }

        public async Task OnGetAsync()
        {
            EventTicket = await _context.EventTickets
                .Include(e => e.Event)
                .Include(e => e.Owner).ToListAsync();
        }
    }
}
