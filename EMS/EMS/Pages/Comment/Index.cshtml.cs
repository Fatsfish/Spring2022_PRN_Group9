using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.Comment
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.Comment> Comment { get;set; }

        public async Task OnGetAsync()
        {
            Comment = await _context.Comments
                .Include(c => c.CreationUser)
                .Include(c => c.Event).ToListAsync();
        }
    }
}
