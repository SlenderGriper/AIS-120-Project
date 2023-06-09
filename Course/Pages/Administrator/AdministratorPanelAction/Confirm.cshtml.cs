using Course.Data;
using Course.Model.DatabaseTables.Achievement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Course.Pages.Administrator.AdministratorPanelAction
{
    public class ConfirmModel : PageModel
    {
        private readonly AchievementContext _context;

        public ConfirmModel(AchievementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Achievement SportAchievement { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var sportachievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);
            if (sportachievement == null)
            {
                return NotFound();
            }
            SportAchievement = sportachievement;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            SportAchievement.WhoСonfirmed = User.FindFirst(ClaimTypes.Name)?.Value;

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

            return RedirectToPage("/Administrator/AdministratorPanel");
        }

        private bool SportAchievementExists(int id)
        {
            return (_context.Achievement?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
