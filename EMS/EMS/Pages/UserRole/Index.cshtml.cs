using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.UserRole
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.UserRole> UserRole { get;set; }

        public async Task OnGetAsync()
        {
            UserRole = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User).ToListAsync();
        }
    }
}
