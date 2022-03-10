using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EMS.Models;

namespace EMS.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public RegisterModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null)
            {
                return RedirectToPage("/User");
            }
            else if (HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role") != null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
        [BindProperty]
        public Models.User Account { get; set; }
        public Models.UserRole Role { get; set; }
        public string Msg { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (AccountExists(Account.Email)) { Msg = "Email has been used, please choose another!"; return Page(); }
            
            Account.FirstName= Account.Email;
            Account.LastName= Account.Email;
            Account.Bio= Account.Email;
            Account.IsActive= false;
            _context.Users.Add(Account);
            Role.UserId = _context.Users.FirstOrDefault(o => o.Email == Account.Email).Id;
            Role.RoleId = 3;
            _context.UserRoles.Add(Role);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Login");
        }

        private bool AccountExists(string id)
        {
            return _context.Users.Any(e => e.Email == id);
        }
    }
}
