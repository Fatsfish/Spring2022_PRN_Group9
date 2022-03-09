﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EMS.Models;

namespace EMS.Pages.InvitationResponseType
{
    public class DetailsModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DetailsModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        public Models.InvitationResponseType InvitationResponseType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvitationResponseType = await _context.InvitationResponseTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (InvitationResponseType == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}