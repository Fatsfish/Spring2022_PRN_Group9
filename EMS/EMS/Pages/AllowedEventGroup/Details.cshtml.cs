using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.AllowedEventGroup
{
    public class DetailsModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DetailsModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public Models.AllowedEventGroup AllowedEventGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AllowedEventGroup = await _context.AllowedEventGroups
                .Include(a => a.Event)
                .Include(a => a.Group).FirstOrDefaultAsync(m => m.Id == id);

            if (AllowedEventGroup == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
