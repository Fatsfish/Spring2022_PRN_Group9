using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class EventInvitationsController : Controller
    {
        private readonly EventMSContext _context;

        public EventInvitationsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: EventInvitations
        public async Task<IActionResult> Index()
        {
            var eventMSContext = _context.EventInvitations.Include(e => e.Event).Include(e => e.InvitationResponse).Include(e => e.User);
            return View(await eventMSContext.ToListAsync());
        }

        // GET: EventInvitations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInvitation = await _context.EventInvitations
                .Include(e => e.Event)
                .Include(e => e.InvitationResponse)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventInvitation == null)
            {
                return NotFound();
            }

            return View(eventInvitation);
        }

        // GET: EventInvitations/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
            ViewData["InvitationResponseId"] = new SelectList(_context.InvitationResponseTypes, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio");
            return View();
        }

        // POST: EventInvitations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,UserId,SentDate,InvitationResponseId,TextResponse,ResponseDate,Id")] EventInvitation eventInvitation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventInvitation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", eventInvitation.EventId);
            ViewData["InvitationResponseId"] = new SelectList(_context.InvitationResponseTypes, "Id", "Name", eventInvitation.InvitationResponseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio", eventInvitation.UserId);
            return View(eventInvitation);
        }

        // GET: EventInvitations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInvitation = await _context.EventInvitations.FindAsync(id);
            if (eventInvitation == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", eventInvitation.EventId);
            ViewData["InvitationResponseId"] = new SelectList(_context.InvitationResponseTypes, "Id", "Name", eventInvitation.InvitationResponseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio", eventInvitation.UserId);
            return View(eventInvitation);
        }

        // POST: EventInvitations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,UserId,SentDate,InvitationResponseId,TextResponse,ResponseDate,Id")] EventInvitation eventInvitation)
        {
            if (id != eventInvitation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventInvitation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventInvitationExists(eventInvitation.Id))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", eventInvitation.EventId);
            ViewData["InvitationResponseId"] = new SelectList(_context.InvitationResponseTypes, "Id", "Name", eventInvitation.InvitationResponseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio", eventInvitation.UserId);
            return View(eventInvitation);
        }

        // GET: EventInvitations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventInvitation = await _context.EventInvitations
                .Include(e => e.Event)
                .Include(e => e.InvitationResponse)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventInvitation == null)
            {
                return NotFound();
            }

            return View(eventInvitation);
        }

        // POST: EventInvitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventInvitation = await _context.EventInvitations.FindAsync(id);
            _context.EventInvitations.Remove(eventInvitation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventInvitationExists(int id)
        {
            return _context.EventInvitations.Any(e => e.Id == id);
        }
    }
}
