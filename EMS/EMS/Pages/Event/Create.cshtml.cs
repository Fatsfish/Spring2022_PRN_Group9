using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EMS.Pages.Event
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
                ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Bio");
                ViewData["StatusId"] = new SelectList(_context.EventStatuses, "Id", "Name");
                return Page();
            }
        }

        [BindProperty]
        public Models.Event Event { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Event.CreationDate = System.DateTime.Now;
            if (Event.RegistrationEndDate < Event.CreationDate || Event.StartDateTime < Event.CreationDate || Event.EndDateTime < Event.CreationDate) return Page();
            if (Event.EndDateTime < Event.StartDateTime) return Page();
            if (Event.EndDateTime < Event.RegistrationEndDate) return Page();
            if(Event.Capacity<0)Event.Capacity = 0;
            if(Event.Price < 0)Event.Price = 0;
            if (Event.Name == null) return Page();
            if (Event.Description == null) return Page();
            if (Event.Place == null) Event.Place = "Undefined";
            if (Event.Images == null) Event.Images = "/img/default.png";
            _context.Events.Add(Event);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
