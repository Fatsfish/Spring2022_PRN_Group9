using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Pages
{
    public class Index_GuestModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Index_GuestModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null || HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role1") != null)
            {
                return RedirectToPage("Event/index");
            }
            return Page();
        }
    }
}
