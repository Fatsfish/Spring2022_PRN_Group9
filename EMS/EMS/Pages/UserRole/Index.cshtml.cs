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

namespace EMS.Pages.UserRole
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

        public PaginatedList<Models.UserRole> UserRole { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            var userRole = from m in _context.UserRoles
                .Include(p => p.User)
                .Include(p => p.Role)
                        select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                userRole = userRole.Where(o => o.Role.Description.Contains(SearchString) || o.User.Bio.Contains(SearchString));
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
            UserRole = await PaginatedList<Models.UserRole>.CreateAsync(
                userRole.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
