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
    public class EventStatusController : Controller
    {
        private readonly EventMSContext _context;

        public EventStatusController(EventMSContext context)
        {
            _context = context;
        }

        // GET: EventStatus
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");
            return View(await _context.EventStatuses.ToListAsync());
        }

        // GET: EventStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var eventStatus = await _context.EventStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventStatus == null)
            {
                return NotFound();
            }

            return View(eventStatus);
        }

        // GET: EventStatus/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            return View();
        }

        // POST: EventStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] EventStatus eventStatus)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (ModelState.IsValid)
            {
                _context.Add(eventStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventStatus);
        }

        // GET: EventStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var eventStatus = await _context.EventStatuses.FindAsync(id);
            if (eventStatus == null)
            {
                return NotFound();
            }
            return View(eventStatus);
        }

        // POST: EventStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] EventStatus eventStatus)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id != eventStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventStatusExists(eventStatus.Id))
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
            return View(eventStatus);
        }

        // GET: EventStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var eventStatus = await _context.EventStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventStatus == null)
            {
                return NotFound();
            }

            return View(eventStatus);
        }

        // POST: EventStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            var eventStatus = await _context.EventStatuses.FindAsync(id);
            _context.EventStatuses.Remove(eventStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventStatusExists(int id)
        {
            return _context.EventStatuses.Any(e => e.Id == id);
        }
    }
}
