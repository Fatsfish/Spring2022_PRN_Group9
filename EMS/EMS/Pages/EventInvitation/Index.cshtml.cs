using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.EventInvitation
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.EventInvitation> EventInvitation { get;set; }

        public async Task OnGetAsync()
        {
            EventInvitation = await _context.EventInvitations
                .Include(e => e.Event)
                .Include(e => e.InvitationResponse)
                .Include(e => e.User).ToListAsync();
        }
    }
}
