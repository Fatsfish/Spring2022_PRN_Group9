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
    public class DeleteModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DeleteModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.GroupUser GroupUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GroupUser = await _context.GroupUsers
                .Include(g => g.Group)
                .Include(g => g.User).FirstOrDefaultAsync(m => m.Id == id);

            if (GroupUser == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GroupUser = await _context.GroupUsers.FindAsync(id);

            if (GroupUser != null)
            {
                _context.GroupUsers.Remove(GroupUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
