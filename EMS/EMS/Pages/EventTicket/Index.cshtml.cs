using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;
using Microsoft.Extensions.Configuration;
using EMS.Services;

namespace EMS.Pages.EventTicket
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;
        private readonly IConfiguration Configuration;
        public IndexModel(EventMSContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public PaginatedList<Models.EventTicket> EventTicket { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            var eventTicket = from m in _context.EventTickets
                .Include(p => p.Event)
                .Include(p => p.Owner)
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                eventTicket = eventTicket.Where(o => o.Event.Description.Contains(SearchString) || o.Owner.Bio.Contains(SearchString));
            }
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var pageSize = Configuration.GetValue("PageSize", 4);
            EventTicket = await PaginatedList<Models.EventTicket>.CreateAsync(
                eventTicket.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}

