using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.User
{
    public class DeleteModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DeleteModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null)
            {
                return RedirectToPage("/Login");
            }
            if (HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role2") != null)
            {
                return RedirectToPage("/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            User = await _context.Users.FindAsync(id);

            if (User != null)
            {
                if (User.IsActive == true) return Page();
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
