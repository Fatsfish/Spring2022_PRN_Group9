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

namespace EMS.Pages.User
{
    public class EditModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public EditModel(EMS.Models.EventMSContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null)
            {
                return RedirectToPage("/Login");
            }
            if ((HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role2") != null) && (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null))
            {
                return RedirectToPage("/Index");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (User.Password == null) return Page();
            if (User.FirstName == null) return Page();
            if (User.LastName == null) return Page();
            if (User.Email == null) return Page();
            var list = _context.Users;
            foreach(var i in list)
            {
                if (User.Email.ToLower() == i.Email.ToLower())
                {
                   if( User.Id != i.Id)
                    {
                        return Page();
                    }
                }
            }
            if (User.Bio == null) User.Bio = "This is User " + User.LastName;
            _context.Attach(User).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(User.Id))
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
