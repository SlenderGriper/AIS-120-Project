using Course.Data;
using Course.Model.Database.Enum;
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
        public string FailMassage { get; set; }
        public Achievement Achievement { get; set; } 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            Achievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);
            if (Achievement == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Achievement= await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);
            var StudentAchievement = await _context.StudentsAchievements.FirstOrDefaultAsync(m => m.AchievementID == Achievement.ID);
            StudentAchievement.FailMessage = FailMassage;
            Achievement.Status = AchiveStatus.Rejected;
            _context.Attach(Achievement).State = EntityState.Modified;
            _context.Attach(StudentAchievement).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AchievementExists(Achievement.ID))
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

        private bool AchievementExists(int id)
        {
            return (_context.Achievement?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
