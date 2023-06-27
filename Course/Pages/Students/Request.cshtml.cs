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

namespace Course.Pages.StudentOpportunities
{
    [Authorize]
    public class RequestModel : PageModel
    {
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
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Input = new InputItem();
            Input.IsDraft = false;
            if (id!=0)
            {
                Input.IsDraft = true;
                var achievement =await _context.Achievement.FirstOrDefaultAsync(m=>m.ID==id);
                if (achievement == null)
                {
                    return NotFound();
                }
                if(achievement.Status!=AchiveStatus.Draft&&achievement.Status!=AchiveStatus.Rejected) {
                    return NotFound();
                }
                
                Input.AchievementType = achievement.AchievementType;
                Input.FilePath = achievement.FilePath;
                Input.ID = id;
                Input.Description= achievement.Description;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string param)
        {
            int? id = _context.Student.Where(f => f.AccountID.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault()?.ID;
            if (Input.Image == null && Input.FilePath == null)
            {
                Massage = "Файл пустой";
                return Page();
            }
            if (Input.Image != null)
            {
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
                Input.FilePath = Input.Image.FileName;
            }
            Achievement achievement = null;
            StudentsAchievements studentsAchievements = null;
            if (Input.IsDraft)
            {
                achievement = await _context.Achievement.FirstOrDefaultAsync(m=>m.ID==Input.ID);
                studentsAchievements = await _context.StudentsAchievements.FirstOrDefaultAsync(m=>m.AchievementID==achievement.ID);
            }
            else {
                achievement = new Achievement();
                studentsAchievements = new StudentsAchievements();
            }
            
            
            achievement.ID  =  (Input.ID==0)  ?  ( _context.Achievement.Max(u => u.ID) + 1) : Input.ID;
            achievement.Description = Input.Description;
            achievement.AchievementType = Input.AchievementType;
            achievement.FilePath = Input.FilePath;
            if (param == "Sent") achievement.Status = AchiveStatus.Sent;
            else achievement.Status = AchiveStatus.Draft;


           
            studentsAchievements.AchievementID = achievement.ID;
            studentsAchievements.StudentID = (int)id;

            if (Input.IsDraft) {
                _context.Attach(achievement).State = EntityState.Modified;
                _context.Attach(studentsAchievements).State = EntityState.Modified;
            }
            else { 
            _context.Achievement.Add(achievement);
            _context.StudentsAchievements.Add(studentsAchievements);
        }
            await _context.SaveChangesAsync();
            Massage = "Запрос на"+Input.Description+" был отправлен";
            if (param == "Sent") return RedirectToPage(@"/Students/Info",new {param= "MyAcheiv" });
            else return RedirectToPage(@"/Students/Info", new { param = "Draft" });
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
            public int ID {  get; set; }

            [Required]
            [Display(Name = "Вид работы")]
            public AchiveType AchievementType { get; set; }


            [Required]
            [Display(Name = "Информация")]
            public string? Description { get; set; }

            [Display(Name = "Картинка")]
            public IFormFile? Image { get; set; }
            public string FilePath { get; set; }
            public bool IsDraft { get; set; }
        }
    }
}