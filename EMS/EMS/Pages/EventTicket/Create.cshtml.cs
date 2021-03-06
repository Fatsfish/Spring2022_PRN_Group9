using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EMS.Pages.EventTicket
{
    public class CreateModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public CreateModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToPage("/Login");
            }
            if (HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role2") != null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ViewData["EventId"] = new SelectList(_context.Events, "Id", "Description");
                ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Bio");
                return Page();
            }
        }

        [BindProperty]
        public Models.EventTicket EventTicket { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            EventTicket.PaidDate = System.DateTime.Now;
            if (EventTicket.EventId == null) return Page();
            if (EventTicket.OwnerId == null) return Page();
            _context.EventTickets.Add(EventTicket);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
