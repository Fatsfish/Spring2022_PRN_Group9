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
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.AllowedEventGroup
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

        public PaginatedList<Models.AllowedEventGroup> AllowedEventGroup { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            if (HttpContext.Session.GetString("role") == null)
            {
                return RedirectToPage("/Login");
            }
            if (HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role2") != null)
            {
                return RedirectToPage("/Index");
            }
            var allowedEventGroup = from m in _context.AllowedEventGroups
                .Include(p => p.Event)
                .Include(p => p.Group)
                       select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                allowedEventGroup = allowedEventGroup.Where(o => o.Event.Description.Contains(SearchString) || o.Group.Description.Contains(SearchString));
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
            AllowedEventGroup = await PaginatedList<Models.AllowedEventGroup>.CreateAsync(
                allowedEventGroup.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}


