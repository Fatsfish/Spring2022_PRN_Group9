using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.Role
{
    public class DeleteModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DeleteModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Role Role { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null)
            {
                return RedirectToPage("/Login");
            }
            if ((HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role2") != null) && (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null))
            {
                return RedirectToPage("/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            Role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);

            if (Role == null)
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

            Role = await _context.Roles.FindAsync(id);

            if (Role != null)
            {
                _context.Roles.Remove(Role);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
