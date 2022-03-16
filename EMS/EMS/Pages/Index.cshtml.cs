using EMS.Models;
using EMS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly EMS.Models.EventMSContext _context;
        private readonly IConfiguration Configuration;
        public IndexModel(ILogger<IndexModel> logger, EventMSContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;
        }

        public List<Models.Event> Event { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public async Task<IActionResult> OnGetAsync()
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
                .Include(p => p.EventTickets).OrderByDescending(p => p.CreationDate).OrderByDescending(p => p.Name)
                             select m;
                int i = 0;
                var list = new List<Models.Event>();
                foreach (var e in _event)
                {
                    if (e.IsPublic == true)
                    {
                        if (i < 4) { list.Add(e); i++; }
                    }
                }
                Event = list;
                return Page();
            }
        }
    }
}
