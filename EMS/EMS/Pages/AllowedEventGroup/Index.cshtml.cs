using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.AllowedEventGroup
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.AllowedEventGroup> AllowedEventGroup { get;set; }

        public async Task OnGetAsync()
        {
            AllowedEventGroup = await _context.AllowedEventGroups
                .Include(a => a.Event)
                .Include(a => a.Group).ToListAsync();
        }
    }
}
