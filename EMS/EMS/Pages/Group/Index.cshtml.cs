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

namespace EMS.Pages.Group
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

        public PaginatedList<Models.Group> Group { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
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
                var group = from m in _context.Groups
                    .Include(p => p.GroupUsers)
                    .Include(p => p.AllowedEventGroups)
                            select m;
                if (!string.IsNullOrEmpty(SearchString))
                {
                    group = group.Where(o => o.Name.Contains(SearchString));
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
                Group = await PaginatedList<Models.Group>.CreateAsync(
                    group.AsNoTracking(), pageIndex ?? 1, pageSize);
                if (HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role1") != null)
                {
                    return Page();
                }
                return Page();
            }
        }
    }
}

