using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Course.Data;
using Course.Model.DatabaseTables.Achievement;

namespace Course.Pages.Administrator.AdministratorPanelAction
{
    public class EditModel : PageModel
    {
        private readonly AchievementContext _context;

        public EditModel(AchievementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SportAchievement SportAchievement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SportAchievement == null)
            {
                return NotFound();
            }

            var sportachievement = await _context.SportAchievement.FirstOrDefaultAsync(m => m.ID == id);
            if (sportachievement == null)
            {
                return NotFound();
            }
            SportAchievement = sportachievement;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SportAchievement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SportAchievementExists(SportAchievement.ID))
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

        private bool SportAchievementExists(int id)
        {
            return (_context.SportAchievement?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
