using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EMS.Models;
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.UserRole
{
    public class EditModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public EditModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.UserRole UserRole { get; set; }

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

            UserRole = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User).FirstOrDefaultAsync(m => m.Id == id);

            if (UserRole == null)
            {
                return NotFound();
            }
           ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Description");
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (UserRole.RoleId == null) return Page();
            if (UserRole.UserId == null) return Page();
            _context.Attach(UserRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(UserRole.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserRoleExists(int id)
        {
            return _context.UserRoles.Any(e => e.Id == id);
        }
    }
}
