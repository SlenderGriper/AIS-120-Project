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
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Course.Pages.StudentOpportunities
{
    [Authorize]
    public class RequestModel : PageModel
    {
        public record class Person(string Name, int Point, string? FilePath);
        private readonly AchievementContext _context;
        
        
        public RequestModel(AchievementContext context)
        {
            _context = context;
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
            }
            int AchievementID;
            Achievement achievement = null;
            StudentsAchievements[] studentsAchievements = null;
            switch (Input.AchievementType)
            {
                case (AchiveType.Sport):
                    achievement = new SportAchievement();
                    achievement.ID = AchievementID = _context.SportAchievement.Max(u => u.ID) + 1;
                    achievement.Description = Input.Description;
                    _context.SportAchievement.Add((SportAchievement)achievement);

                    studentsAchievements = new StudentsSportAchievements[student.Length];
                    for (int i=0;i<student.Length;i++)
                    {
                        if (id[i] == null) continue;
                        studentsAchievements[i] = new StudentsSportAchievements();
                        studentsAchievements[i].AchievementID = AchievementID;
                        studentsAchievements[i].StudentID = (int)id[i];
                        studentsAchievements[i].Point = student[i].Point;
                        studentsAchievements[i].FilePath = student[i].FilePath;
                        _context.StudentsSportAchievements.Add((StudentsSportAchievements)studentsAchievements[i]);

                    }
                    break;
                case (AchiveType.Research):
                    achievement = new ResearchAchievement();
                    achievement.ID = AchievementID = _context.ResearchAchievement.Max(u => u.ID) + 1;
                    achievement.Description = Input.Description;
                    _context.ResearchAchievement.Add((ResearchAchievement)achievement);

                    studentsAchievements = new StudentsResearchAchievements[student.Length];
                    for (int i = 0; i < student.Length; i++)
                    {
                        if (id[i] == null) continue;
                        studentsAchievements[i] = new StudentsResearchAchievements();
                        studentsAchievements[i].AchievementID = AchievementID;
                        studentsAchievements[i].StudentID = (int)id[i];
                        studentsAchievements[i].Point = student[i].Point;
                        studentsAchievements[i].FilePath = student[i].FilePath;
                        _context.StudentsResearchAchievements.Add((StudentsResearchAchievements)studentsAchievements[i]);

                    }
                    break;
                case (AchiveType.Social):
                    achievement = new SocialAchievement();
                    achievement.ID = AchievementID = _context.SocialAchievement.Max(u => u.ID) + 1;
                    achievement.Description = Input.Description;
                    _context.SocialAchievement.Add((SocialAchievement)achievement);

                    studentsAchievements = new StudentsSocialAchievements[student.Length];
                    for (int i = 0; i < student.Length; i++)
                    {
                        if (id[i] == null) continue;
                        studentsAchievements[i] = new StudentsSocialAchievements();
                        studentsAchievements[i].AchievementID = AchievementID;
                        studentsAchievements[i].StudentID = (int)id[i];
                        studentsAchievements[i].Point = student[i].Point;
                        studentsAchievements[i].FilePath = student[i].FilePath;
                        _context.StudentsSocialAchievements.Add((StudentsSocialAchievements)studentsAchievements[i]);

                    }
                    break;
                case (AchiveType.Cultural):
                    achievement = new CulturalAchievement();
                    achievement.ID = AchievementID = _context.CulturalAchievement.Max(u => u.ID) + 1;
                    achievement.Description = Input.Description;
                    _context.CulturalAchievement.Add((CulturalAchievement)achievement);

                    studentsAchievements = new StudentsCulturalAchievements[student.Length];
                    for (int i = 0; i < student.Length; i++)
                    {
                        if (id[i] == null) continue;
                        studentsAchievements[i] = new StudentsCulturalAchievements();
                        studentsAchievements[i].AchievementID = AchievementID;
                        studentsAchievements[i].StudentID = (int)id[i];
                        studentsAchievements[i].Point = student[i].Point;
                        studentsAchievements[i].FilePath = student[i].FilePath;
                        _context.StudentsCulturalAchievements.Add((StudentsCulturalAchievements)studentsAchievements[i]);

                    }
                    break;

            }
            await _context.SaveChangesAsync();
            return Page();
        }

        public class InputItem
        {

            [Required]
            [Display(Name = "Вид работы")]
            public AchiveType AchievementType { get; set; }


            [Required]
            [Display(Name = "Информация")]
            public string? Description { get; set; }
        }
    }
}