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
    public class EventsController : Controller
    {
        private readonly EventMSContext _context;

        public EventsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");
            var eventMSContext = _context.Events.Include(o => o.CreationUser).Include(o => o.Status);
            return View(await eventMSContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var oevent = await _context.Events
                .Include(o => o.CreationUser)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oevent == null)
            {
                return NotFound();
            }

            return View(oevent);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            ViewData["StatusId"] = new SelectList(_context.EventStatuses, "Id", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreationDate,CreationUserId,RegistrationEndDate,StartDateTime,EndDateTime,Place,IsPublic,Capacity,Price,Images")] Event oevent)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (ModelState.IsValid)
            {
                bool err = false;

                oevent.CreationDate = DateTime.Now;
                if (oevent.RegistrationEndDate < oevent.CreationDate || oevent.StartDateTime < oevent.CreationDate || oevent.EndDateTime < oevent.CreationDate)
                {
                    ViewBag.Message = "Registration End Date, Start Date, End Date should come after now";
                    return View(oevent);
                }
                if (oevent.EndDateTime < oevent.StartDateTime)
                {
                    ViewBag.Message = "End date should come after start date";
                    return View(oevent);
                }
                if (oevent.EndDateTime < oevent.RegistrationEndDate)
                {
                    ViewBag.Message = "Registration date should come before end date";
                    return View(oevent);
                }

                if (oevent.Capacity < 2)
                {
                    ViewBag.CapacityMessage = "Capacity must >= 3";
                    err = true;
                }
                if (oevent.Price < 0)
                {
                    ViewBag.PriceMessage = "Price must >= 0";
                    err = true;
                }
                if (string.IsNullOrEmpty(oevent.Name))
                {
                    ViewBag.NameMessage = "Event name is required";
                    err = true;
                }

                if (err) return View(oevent);

                if (oevent.Description == null) oevent.Description = "";
                if (oevent.Place == null) oevent.Place = "Undefined";
                if (oevent.Images == null) oevent.Images = "https://playorlandonorth.com/assets/images/placeholders/placeholder-event.png";

                oevent.Status = new EventStatus() { Name = "Available" };
                oevent.CreationUserId = HttpContext.Session.GetInt32("id");

                _context.Add(oevent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Bio", oevent.CreationUserId);
            ViewData["StatusId"] = new SelectList(_context.EventStatuses, "Id", "Name", oevent.StatusId);
            return View(oevent);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var oevent = await _context.Events.FindAsync(id);
            if (oevent == null)
            {
                return NotFound();
            }
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Bio", oevent.CreationUserId);
            ViewData["StatusId"] = new SelectList(_context.EventStatuses, "Id", "Name", oevent.StatusId);
            return View(oevent);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreationDate,CreationUserId,RegistrationEndDate,StartDateTime,EndDateTime,Place,IsPublic,Capacity,Price,StatusId,Images")] Event oevent)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id != oevent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oevent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(oevent.Id))
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
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Bio", oevent.CreationUserId);
            ViewData["StatusId"] = new SelectList(_context.EventStatuses, "Id", "Name", oevent.StatusId);
            return View(oevent);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var oevent = await _context.Events
                .Include(o => o.CreationUser)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oevent == null)
            {
                return NotFound();
            }

            return View(oevent);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            var oevent = await _context.Events.FindAsync(id);
            _context.Events.Remove(oevent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
