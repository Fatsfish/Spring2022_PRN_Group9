using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly EventMSContext _context;

        public UsersController(EventMSContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password,FirstName,LastName,Email,Bio,IsActive")] User user)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (ModelState.IsValid)
            {
                bool err = false;

                if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.FirstName) ||
                        string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.Email))
                {
                    ViewBag.Message = "Password, First Name, Last Name, Email are required. Please fill all fields!";
                    err = true;
                }
                else
                {
                    if (user.Password.Trim().Length < 3 || user.Password.Trim().Length > 31)
                    {
                        ViewBag.PasswordMessage = "Password from 4 - 30 characters.";
                        err = true;
                    }

                    if (user.FirstName.Trim().Length < 71)
                    {
                        ViewBag.NameMessage = "First name from 1 - 70 characters.";
                        err = true;
                    }

                    if (user.LastName.Trim().Length > 71)
                    {
                        ViewBag.LastNameMessage = "Last name from 1 - 70 characters.";
                    }

                    Regex rg = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                    if (!rg.IsMatch(user.Email.Trim()))
                    {
                        ViewBag.EmailMessage = "Invalid email.";
                        err = true;
                    }
                }

                if (err)
                {
                    return View(user);
                }

                if (string.IsNullOrEmpty(user.Bio)) user.Bio = "";
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Password,FirstName,LastName,Email,Bio,IsActive")] User user)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool err = false;

                    if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.FirstName) ||
                        string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.Email))
                    {
                        ViewBag.Message = "Password, First Name, Last Name, Email are required. Please fill all fields!";
                        err = true;
                    }
                    else
                    {
                        if (user.Password.Trim().Length < 3 || user.Password.Trim().Length > 31)
                        {
                            ViewBag.PasswordMessage = "Password from 4 - 30 characters.";
                            err = true;
                        }

                        if (user.FirstName.Trim().Length > 71)
                        {
                            ViewBag.NameMessage = "First name from 1 - 70 characters.";
                            err = true;
                        }

                        if (user.LastName.Trim().Length > 71)
                        {
                            ViewBag.LastNameMessage = "Last name from 1 - 70 characters.";
                        }

                        Regex rg = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                        if (!rg.IsMatch(user.Email.Trim()))
                        {
                            ViewBag.EmailMessage = "Invalid email.";
                            err = true;
                        }
                    }


                    if (err)
                    {
                        return View(user);
                    }

                    if (string.IsNullOrEmpty(user.Bio.Trim())) user.Bio = "";

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            var user = await _context.Users.FindAsync(id);
            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
