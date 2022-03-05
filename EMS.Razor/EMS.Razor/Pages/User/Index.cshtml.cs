using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Razor.Models.User;

namespace EMS.Razor.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Razor.Models.EventMSContext _context;

        public IndexModel(EMS.Razor.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<User> User { get;set; }

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
