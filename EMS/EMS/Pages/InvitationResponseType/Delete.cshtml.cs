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
    public class DeleteModel : PageModel
    {
        private readonly EMS.Models.EventMSContext _context;

        public DeleteModel(EMS.Models.EventMSContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            InvitationResponseType = await _context.InvitationResponseTypes.FindAsync(id);

            if (InvitationResponseType != null)
            {
                _context.InvitationResponseTypes.Remove(InvitationResponseType);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}