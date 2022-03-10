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

namespace EMS.Pages.InvitationResponseType
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

        public PaginatedList<Models.InvitationResponseType> InvitationResponseType { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            var invitationResponseType = from m in _context.InvitationResponseTypes
                .Include(p => p.EventInvitations)
                .Include(p => p.Name)
                           select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                invitationResponseType = invitationResponseType.Where(o => o.Name.Contains(SearchString));
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
            InvitationResponseType = await PaginatedList<Models.InvitationResponseType>.CreateAsync(
                invitationResponseType.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
