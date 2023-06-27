using Course.Data;
using Course.Model.Database.Enum;
using Course.Model.DatabaseTables;
using Course.Model.DatabaseTables.Achievement;
using Course.Model.DatabaseTables.StudentAchivement;
using Course.Model.PageItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Course.Pages.Administrator.AdministratorPanelAction
{
    public class EditModel : PageModel
    {
        private readonly AchievementContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public EditModel(AchievementContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public InputItem Input { get; set; }

        public string Massage { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Achievement == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == id);
            Input = new InputItem();
            Input.Description = achievement.Description;
            Input.AchievementType = achievement.AchievementType;
            Input.FilePath = achievement.FilePath;
            Input.ID = achievement.ID;

            if (achievement == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Input.Point <= 0)
            {
                Massage = "Неправильные баллы";
                return Page();
            }
            if (Input.Image == null && Input.FilePath == null)
            {
                Massage = "Файл пустой";
                return Page();
            }
            if (Input.Image != null)
            {
                if (Input.Image.Length == 0)
                {
                    Massage = "Файл пустой";
                    return Page();
                }
                if (!IsImage(Input.Image))
                {
                    Massage = "Файл не картинка";
                    return Page();
                }
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", Input.Image.FileName);
                if (FileSystem.FileExists(filePath))
                {
                    Massage = "Файл с таким именем уже существует";
                    return Page();
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.Image.CopyToAsync(stream);
                }
                Input.FilePath = Input.Image.FileName;
            }
            var achievement = await _context.Achievement.FirstOrDefaultAsync(m => m.ID == Input.ID);
            var studentsAchievements = await _context.StudentsAchievements.FirstOrDefaultAsync(m => m.AchievementID == achievement.ID);
            achievement.Description=Input.Description;
            achievement.AchievementType = Input.AchievementType;
            achievement.FilePath= Input.FilePath;
            studentsAchievements.Point = Input.Point;
            achievement.Status = AchiveStatus.Accepted;
            achievement.WhoСonfirmed = User.FindFirst(ClaimTypes.Name)?.Value;
            _context.Attach(achievement).State = EntityState.Modified;
            _context.Attach(studentsAchievements).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AchievementExists(achievement.ID))
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

        private bool IsImage(IFormFile file)
        {
            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/png" &&
                file.ContentType.ToLower() != "image/jpeg")
            {
                return false;
            }

            return true;
        }

        private bool AchievementExists(int id)
        {
            return (_context.Achievement?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        public class InputItem
        {
            [Display(Name = "Описание")]
            [Required]
            public string Description { get; set; }
            [Display(Name = "Вид достижения")]
            [Required]
            public AchiveType AchievementType { get; set; }

            
            [Display(Name = "Заменить картинку(необязательно)")]
            public IFormFile? Image { get; set; }
            [Display(Name = "Баллы")]
            [Required]
            public int Point { get; set; }

            public string FilePath { get; set; }
            public int ID { get; set; }
        }
    }
}
