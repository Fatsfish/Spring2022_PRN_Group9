using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.Event
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.Event> Event { get;set; }

        public async Task OnGetAsync()
        {
            Event = await _context.Events
                .Include(e => e.CreationUser)
                .Include(e => e.Status).ToListAsync();
        }
    }
}
