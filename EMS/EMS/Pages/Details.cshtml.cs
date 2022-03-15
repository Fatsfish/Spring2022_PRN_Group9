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

        public Models.Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            /*if (HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToPage("/Login");
            }
            if (HttpContext.Session.GetString("role2") == "member" || HttpContext.Session.GetString("role2") != null)
            {
                return RedirectToPage("/Index");
            }
            else*/
            {
                if (id == null)
                {
                    return NotFound();
                }

                Event = await _context.Events
                    .Include(e => e.CreationUser)
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
