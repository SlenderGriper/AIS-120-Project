using Course.Data;
using Course.Model.DatabaseTables.Achievement;
using Course.Model.DatabaseTables.StudentAchivement;
using Course.Model.PageItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Course.Pages.Administrator.AdministratorPanelAction
{
    public class AboutModel : PageModel
    {
        private readonly AchievementContext _context;
        [BindProperty]
        public AchievementInformation Input { get; set; }
        public Achievement Achievement { get; set; }
        public string FullName { get; set; }
        public AboutModel(AchievementContext context) {
            _context = context;

        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            Achievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);
            var StudentsAchievements = await _context.StudentsAchievements.FirstOrDefaultAsync(m => m.AchievementID == Achievement.ID);
            var idName=await _context.Student.FirstOrDefaultAsync(m=>m.ID == StudentsAchievements.StudentID);
            var account = await _context.Account.FirstOrDefaultAsync(m => m.ID == idName.AccountID);
            FullName = account.FullName;
            if (Achievement == null|| StudentsAchievements == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
