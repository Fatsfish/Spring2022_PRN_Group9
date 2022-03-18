using EMS.Models;
using EMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EMS.Pages
{
    public class EventHistoryModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly EMS.Models.EventMSContext _context;
        private readonly IConfiguration Configuration;
        public EventHistoryModel(ILogger<IndexModel> logger, EventMSContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;
        }

        public PaginatedList<Models.Event> Event { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null || HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role1") != null)
            {
                return RedirectToPage("Event/index");
            }
            else
            {
                var _event = from m in _context.Events
                .Include(p => p.CreationUser)
                .Include(p => p.AllowedEventGroups)
                .Include(p => p.Status)
                .Include(p => p.Comments)
                .Include(p => p.EventInvitations)
                .Include(p => p.EventTickets)
                             select m;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    _event = _event.Where(o => o.Name.Contains(SearchString));
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
                Event = await PaginatedList<Models.Event>.CreateAsync(
                    _event.AsNoTracking(), pageIndex ?? 1, pageSize);
                return Page();
            }
        }
    }
}
