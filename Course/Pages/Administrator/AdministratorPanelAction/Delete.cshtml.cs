using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Course.Data;
using Course.Model.DatabaseTables.Achievement;

namespace Course.Pages.Administrator.AdministratorPanelAction
{
    public class DeleteModel : PageModel
    {
        private readonly Course.Data.AchievementContext _context;

        public DeleteModel(Course.Data.AchievementContext context)
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
            else 
            {
                SportAchievement = sportachievement;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.SportAchievement == null)
            {
                return NotFound();
            }
            var sportachievement = await _context.SportAchievement.FindAsync(id);

            if (sportachievement != null)
            {
                SportAchievement = sportachievement;
                _context.SportAchievement.Remove(SportAchievement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
