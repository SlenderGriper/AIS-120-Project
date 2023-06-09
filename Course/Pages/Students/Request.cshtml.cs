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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Course.Pages.StudentOpportunities
{
    [Authorize]
    public class RequestModel : PageModel
    {
        public record class Person(string Name, int Point, string? FilePath);
        private readonly AchievementContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public string Massage { get; set; }
       
        public RequestModel(AchievementContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }
     
        [BindProperty]
        public InputItem Input { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostAsync(Person[] student)
        {
            int?[] id = new int?[student.Length];
            int index = 0;
            foreach (var item in student)
            {
                id[index] = _context.Account.Where(f => f.FullName == item.Name).FirstOrDefault()?.ID;
                id[index] = _context.Student.Where(f => f.AccountID == id[index]).FirstOrDefault()?.ID;
                if (id[index] == null)
                {
                    Massage = item.Name + " не существует";
                    return Page();
                };
            }
            if (Input.Image == null || Input.Image.Length == 0)
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
            int AchievementID;
            Achievement achievement = null;
            StudentsAchievements[] studentsAchievements = null;
            achievement = new Achievement();
            achievement.ID = AchievementID = _context.Achievement.Max(u => u.ID) + 1;
            achievement.Description = Input.Description;
            achievement.AchievementType = Input.AchievementType;
            achievement.FilePath = Input.Image.FileName;
            _context.Achievement.Add(achievement);

            studentsAchievements = new StudentsAchievements[student.Length];
            for (int i = 0; i < student.Length; i++)
            {

                studentsAchievements[i] = new StudentsAchievements();
                studentsAchievements[i].AchievementID = AchievementID;
                studentsAchievements[i].StudentID = (int)id[i];
                studentsAchievements[i].Point = student[i].Point;
                
                _context.StudentsAchievements.Add(studentsAchievements[i]);

            }
            await _context.SaveChangesAsync();
            Massage = "Запрос на"+Input.Description+" был отправлен";
            return Page();
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
        public class InputItem
        {

            [Required]
            [Display(Name = "Вид работы")]
            public AchiveType AchievementType { get; set; }


            [Required]
            [Display(Name = "Информация")]
            public string? Description { get; set; }
            [Required]
            [Display(Name = "Картинка")]
            public IFormFile? Image { get; set; }
        }
    }
}