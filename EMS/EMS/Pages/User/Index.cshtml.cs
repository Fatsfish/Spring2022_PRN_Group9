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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.User
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

        public PaginatedList<Models.User> User { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        
        public async Task<IActionResult> OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            if (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null)
            {
                return RedirectToPage("/Login");
            }
            if (HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role2") != null)
            {
                return RedirectToPage("/Index");
            }
            var users = from m in _context.Users
                .Include(p => p.UserRoles)
                .Include(p => p.GroupUsers)
                           select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(o => o.FirstName.Contains(SearchString) || o.LastName.Contains(SearchString));
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
            User = await PaginatedList<Models.User>.CreateAsync(
                users.AsNoTracking(), pageIndex ?? 1, pageSize);
            return Page();
        }
    }
}
