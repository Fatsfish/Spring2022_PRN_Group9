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
    public class InvitationResponseTypesController : Controller
    {
        private readonly EventMSContext _context;

        public InvitationResponseTypesController(EventMSContext context)
        {
            _context = context;
        }

        // GET: InvitationResponseTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.InvitationResponseTypes.ToListAsync());
        }

        // GET: InvitationResponseTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitationResponseType = await _context.InvitationResponseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitationResponseType == null)
            {
                return NotFound();
            }

            return View(invitationResponseType);
        }

        // GET: InvitationResponseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InvitationResponseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] InvitationResponseType invitationResponseType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invitationResponseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invitationResponseType);
        }

        // GET: InvitationResponseTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitationResponseType = await _context.InvitationResponseTypes.FindAsync(id);
            if (invitationResponseType == null)
            {
                return NotFound();
            }
            return View(invitationResponseType);
        }

        // POST: InvitationResponseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] InvitationResponseType invitationResponseType)
        {
            if (id != invitationResponseType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invitationResponseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvitationResponseTypeExists(invitationResponseType.Id))
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
            return View(invitationResponseType);
        }

        // GET: InvitationResponseTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitationResponseType = await _context.InvitationResponseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invitationResponseType == null)
            {
                return NotFound();
            }

            return View(invitationResponseType);
        }

        // POST: InvitationResponseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitationResponseType = await _context.InvitationResponseTypes.FindAsync(id);
            _context.InvitationResponseTypes.Remove(invitationResponseType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvitationResponseTypeExists(int id)
        {
            return _context.InvitationResponseTypes.Any(e => e.Id == id);
        }
    }
}
