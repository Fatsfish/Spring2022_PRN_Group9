using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.API.Models;

namespace EMS.API.Views.User
{
    public class IndexModel : PageModel
    {
        private readonly EMS.API.Models.EventMSContext _context;

        public IndexModel(EMS.API.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.User> User { get;set; }

        public async Task OnGetAsync()
        {
            User = await _context.Users.ToListAsync();
        }
    }
}
