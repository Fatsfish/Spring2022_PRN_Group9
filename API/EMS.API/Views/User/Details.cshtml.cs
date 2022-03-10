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
    public class DetailsModel : PageModel
    {
        private readonly EMS.API.Models.EventMSContext _context;

        public DetailsModel(EMS.API.Models.EventMSContext context)
        {
            _context = context;
        }

        public Models.User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
