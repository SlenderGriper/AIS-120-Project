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
      public Achievement Achievement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);

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

            if (achievement != null)
            {
                Achievement = achievement;
                
                var students = _context.StudentsAchievements.Where(p => p.AchievementID == achievement.ID).ToList();
                foreach(var student in students)
                {
                    _context.StudentsAchievements.Remove(student);
                }
                await _context.SaveChangesAsync();
                _context.Achievement.Remove(Achievement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToPage("/Administrator/AdministratorPanel");
        }
    }
}
