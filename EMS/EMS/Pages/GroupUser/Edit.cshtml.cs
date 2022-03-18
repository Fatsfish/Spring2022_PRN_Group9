using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Pages.GroupUser
{
    public class EditModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public EditModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.GroupUser GroupUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
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
                if (id == null)
                {
                    return NotFound();
                }

                GroupUser = await _context.GroupUsers
                    .Include(g => g.Group)
                    .Include(g => g.User).FirstOrDefaultAsync(m => m.Id == id);

                if (GroupUser == null)
                {
                    return NotFound();
                }
                ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Description");
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Bio");
                return Page();
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (GroupUser.UserId == null) return Page();
            if (GroupUser.GroupId == null) return Page();
            _context.Attach(GroupUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupUserExists(GroupUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool GroupUserExists(int id)
        {
            return _context.GroupUsers.Any(e => e.Id == id);
        }
    }
}
