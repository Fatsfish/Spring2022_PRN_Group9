using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EMS.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DetailsModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }
        public Models.EventTicket EventTicket { get; set; }
        public async Task<IActionResult> OnGetApply(int? id)
        {
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null || HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role1") != null)
            {
                return RedirectToPage("Event/index");
            }
            else
            {
                var ticket = _context.EventTickets.AnyAsync(p => p.OwnerId == HttpContext.Session.GetInt32("id"));
                if (ticket != null) return Page();
                if (HttpContext.Session.GetInt32("id") == null) { return Page(); }
                EventTicket = new Models.EventTicket();
                EventTicket.IsPaid = true;
                EventTicket.EventId = id;
                EventTicket.PaidDate = System.DateTime.Now;
                EventTicket.OwnerId = HttpContext.Session.GetInt32("id");
                _context.EventTickets.Add(EventTicket);
                await _context.SaveChangesAsync();
                Event = await _context.Events
                        .Include(e => e.CreationUser).Include(e => e.EventTickets)
                        .Include(e => e.Status).FirstOrDefaultAsync(m => m.Id == id);
                return Page();
            }
        }

        public Models.Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("role") == "admin" || HttpContext.Session.GetString("role") != null || HttpContext.Session.GetString("role1") == "host" || HttpContext.Session.GetString("role1") != null)
            {
                return RedirectToPage("Event/index");
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                Event = await _context.Events
                    .Include(e => e.CreationUser).Include(e => e.EventTickets)
                    .Include(e => e.Status).FirstOrDefaultAsync(m => m.Id == id);

                if (Event == null)
                {
                    return NotFound();
                }
                return Page();
            }
        }
    }
}
