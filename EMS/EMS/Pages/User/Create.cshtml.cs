using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EMS.Models;
using Microsoft.AspNetCore.Http;

namespace EMS.Pages.User
{
    public class CreateModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public CreateModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null)
            {
                return RedirectToPage("/Login");
            }
            if ((HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role2") != null) && (HttpContext.Session.GetString("role") != "admin" || HttpContext.Session.GetString("role") == null))
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        [BindProperty]
        public Models.User User { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (User.Password == null) return Page();
            if (User.FirstName == null) return Page();
            if (User.LastName == null) return Page();
            if (User.Email == null) return Page(); 
            if (UserExists(User.Email)) return Page();
            if (User.Bio == null) User.Bio = "This is User " + User.LastName;
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }
    }
}
