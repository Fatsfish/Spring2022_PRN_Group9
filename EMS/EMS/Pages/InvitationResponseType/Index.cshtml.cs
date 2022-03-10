using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.InvitationResponseType
{
    public class IndexModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public IndexModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public IList<Models.InvitationResponseType> InvitationResponseType { get;set; }

        public async Task OnGetAsync()
        {
            InvitationResponseType = await _context.InvitationResponseTypes.ToListAsync();
        }
    }
}
