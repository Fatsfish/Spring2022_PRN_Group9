using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.GroupUser
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.GroupUser> GroupUser { get;set; }

        public async Task OnGetAsync()
        {
            GroupUser = await _context.GroupUsers
                .Include(g => g.Group)
                .Include(g => g.User).ToListAsync();
        }
    }
}
