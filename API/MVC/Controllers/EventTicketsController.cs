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
    public class EventTicketsController : Controller
    {
        private readonly EventMSContext _context;

        public EventTicketsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: EventTickets
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            var eventMSContext = _context.EventTickets.Include(e => e.Event).Include(e => e.Owner);
            return View(await eventMSContext.ToListAsync());
        }

        // GET: EventTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var eventTicket = await _context.EventTickets
                .Include(e => e.Event)
                .Include(e => e.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventTicket == null)
            {
                return NotFound();
            }

            return View(eventTicket);
        }

        // GET: EventTickets/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio");
            return View();
        }

        // POST: EventTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventId,OwnerId,IsPaid,PaidDate")] EventTicket eventTicket)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (ModelState.IsValid)
            {
                _context.Add(eventTicket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", eventTicket.EventId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio", eventTicket.OwnerId);
            return View(eventTicket);
        }

        // GET: EventTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var eventTicket = await _context.EventTickets.FindAsync(id);
            if (eventTicket == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", eventTicket.EventId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio", eventTicket.OwnerId);
            return View(eventTicket);
        }

        // POST: EventTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventId,OwnerId,IsPaid,PaidDate")] EventTicket eventTicket)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id != eventTicket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventTicketExists(eventTicket.Id))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", eventTicket.EventId);
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio", eventTicket.OwnerId);
            return View(eventTicket);
        }

        // GET: EventTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var eventTicket = await _context.EventTickets
                .Include(e => e.Event)
                .Include(e => e.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventTicket == null)
            {
                return NotFound();
            }

            return View(eventTicket);
        }

        // POST: EventTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            var eventTicket = await _context.EventTickets.FindAsync(id);
            _context.EventTickets.Remove(eventTicket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventTicketExists(int id)
        {
            return _context.EventTickets.Any(e => e.Id == id);
        }
    }
}
