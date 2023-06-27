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
        private readonly AchievementContext _context;

        public DeleteModel(AchievementContext context)
        {
            _context = context;
        }
        [BindProperty]
        public string FullName { get; set; }
        public Achievement Achievement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);
            var StudentsAchievements = await _context.StudentsAchievements.FirstOrDefaultAsync(m => m.AchievementID == achievement.ID);
            var idName = await _context.Student.FirstOrDefaultAsync(m => m.ID == StudentsAchievements.StudentID);
            var account = await _context.Account.FirstOrDefaultAsync(m => m.ID == idName.AccountID);
            FullName = account.FullName;
            if (achievement == null)
            {
                return NotFound();
            }
            else 
            {
                Achievement = achievement;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }
            var achievement = await _context.Achievement.FindAsync(id);
            var studentsAchievements = await _context.StudentsAchievements.FirstOrDefaultAsync(m => m.AchievementID == achievement.ID);

            if (achievement != null&& studentsAchievements!=null)
            {
                _context.StudentsAchievements.Remove(studentsAchievements);
                await _context.SaveChangesAsync();
                _context.Achievement.Remove(achievement);
                await _context.SaveChangesAsync();
            }
            
           
            return RedirectToPage("/Administrator/AdministratorPanel");
        }
    }
}
