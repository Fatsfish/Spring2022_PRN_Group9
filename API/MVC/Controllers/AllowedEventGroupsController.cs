using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class AllowedEventGroupsController : Controller
    {
        private readonly EventMSContext _context;

        public AllowedEventGroupsController(EventMSContext context)
        {
            _context = context;
        }

        // GET: AllowedEventGroups
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("id") == null || HttpContext.Session.GetString("role") == null || HttpContext.Session.GetString("role1") == null ) return Redirect("/Home/Login");
            var eventMSContext = _context.AllowedEventGroups.Include(a => a.Event).Include(a => a.Group);
            return View(await eventMSContext.ToListAsync());
        }

        // GET: AllowedEventGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");
            if (id == null)
            {
                return NotFound();
            }

            var allowedEventGroup = await _context.AllowedEventGroups
                .Include(a => a.Event)
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allowedEventGroup == null)
            {
                return NotFound();
            }

            return View(allowedEventGroup);
        }

        // GET: AllowedEventGroups/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Description");
            return View();
        }

        // POST: AllowedEventGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,GroupId,Id")] AllowedEventGroup allowedEventGroup)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (ModelState.IsValid)
            {
                _context.Add(allowedEventGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", allowedEventGroup.EventId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Description", allowedEventGroup.GroupId);
            return View(allowedEventGroup);
        }

        // GET: AllowedEventGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var allowedEventGroup = await _context.AllowedEventGroups.FindAsync(id);
            if (allowedEventGroup == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", allowedEventGroup.EventId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Description", allowedEventGroup.GroupId);
            return View(allowedEventGroup);
        }

        // POST: AllowedEventGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,GroupId,Id")] AllowedEventGroup allowedEventGroup)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id != allowedEventGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allowedEventGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllowedEventGroupExists(allowedEventGroup.Id))
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
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description", allowedEventGroup.EventId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Description", allowedEventGroup.GroupId);
            return View(allowedEventGroup);
        }

        // GET: AllowedEventGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            if (id == null)
            {
                return NotFound();
            }

            var allowedEventGroup = await _context.AllowedEventGroups
                .Include(a => a.Event)
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (allowedEventGroup == null)
            {
                return NotFound();
            }

            return View(allowedEventGroup);
        }

        // POST: AllowedEventGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("id") == null || (HttpContext.Session.GetString("role") == null && HttpContext.Session.GetString("role1") == null)) return Redirect("/Home/Login");

            var allowedEventGroup = await _context.AllowedEventGroups.FindAsync(id);
            _context.AllowedEventGroups.Remove(allowedEventGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllowedEventGroupExists(int id)
        {
            return _context.AllowedEventGroups.Any(e => e.Id == id);
        }
    }
}
