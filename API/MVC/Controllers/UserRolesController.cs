using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly EventMSContext _context;

        public UserRolesController(EventMSContext context)
        {
            _context = context;
        }

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            var eventMSContext = _context.UserRoles.Include(u => u.Role).Include(u => u.User);
            return View(await eventMSContext.ToListAsync());
        }

        // GET: UserRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // GET: UserRoles/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId,Id")] UserRole userRole)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (ModelState.IsValid)
            {
                _context.Add(userRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Description", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Description", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRole.UserId);
            return View(userRole);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RoleId,Id")] UserRole userRole)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id != userRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRoleExists(userRole.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Description", userRole.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRole.UserId);
            return View(userRole);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var userRole = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRole == null)
            {
                return NotFound();
            }

            return View(userRole);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null) return Redirect("/Home/Login");

            var userRole = await _context.UserRoles.FindAsync(id);
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRoleExists(int id)
        {
            return _context.UserRoles.Any(e => e.Id == id);
        }
    }
}
