using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EMS.Pages
{
    public class LoginModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public LoginModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null)
            {
                return RedirectToPage("User/index");
            }
            else if (HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1") != null || HttpContext.Session.GetString("role") != null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("role");
            HttpContext.Session.Remove("id");
            HttpContext.Session.Remove("username");
            return Page();
        }

        [BindProperty]
        public Models.User Account { get; set; }
        public String Msg { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            if (string.IsNullOrEmpty(Account.Email) || string.IsNullOrEmpty(Account.Password))
            {
                Msg = "Please enter your email and password and try again.";
                return Page();
            }
            else
            {
                Account = await _context.Users.FirstOrDefaultAsync(m => m.Email.ToLower()== Account.Email.ToLower() && m.Password == Account.Password);
                if (Account == null)
                {
                    Msg = "The account doesn't exist!";
                    return Page();
                }
                if (Account.IsActive==false)
                {
                    Msg = "The account is not active!";
                    return Page();
                }
                foreach (var i in _context.UserRoles.ToList())
                {
                    if ( i.UserId== Account.Id && i.RoleId == 1)
                    {
                        HttpContext.Session.SetString("role", "admin");
                        return RedirectToPage("User/index");
                    }
                    else if (i.UserId == Account.Id && i.RoleId == 2)
                    {
                        HttpContext.Session.SetString("role1", "host");
                        return RedirectToPage("Index");

                    }
                    else if (i.UserId == Account.Id && i.RoleId == 3)
                    {
                        HttpContext.Session.SetString("role2", "member");
                        return RedirectToPage("Index");
                    }
                    else { Msg = "This account doesn't have role, contact admin for details!"; }
                }
                HttpContext.Session.SetInt32("id", Account.Id);
                HttpContext.Session.SetString("username", Account.Email);
            }
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null)
            {
                return RedirectToPage("User/index");
            }
            else if (HttpContext.Session.GetString("role1") == "host"|| HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role1")!=null|| HttpContext.Session.GetString("role") !=null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                return Page();
            }
        }
    }
}
