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

namespace Course.Pages.Administrator
{
    [Authorize]
    public class StudentsInfoModel : PageModel
    {
        private readonly Data.AchievementContext _context;

        public StudentsInfoModel(Data.AchievementContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
        }
        public bool IsStudentAchivTableCreated { get; set; } = false;
        public bool IsTopStudentTableCreated { get; set; } = false;
        public IList<AchievementInformation> AchievementsInformation { get; set; } = default!;
        public async void CreateAchievementInformation(AchiveType achive)
        {
            if (_context.Achievement == null) return;
            if (_context.StudentsAchievements == null) return;
            var all = (from a in _context.Achievement
                       join sa in _context.StudentsAchievements on a.ID equals sa.AchievementID
                       join s in _context.Student on sa.StudentID equals s.ID
                       join u in _context.Account on s.AccountID equals u.ID
                       orderby sa.Point descending
                       where achive==a.AchievementType
                       select new AchievementInformation
                       {
                           FullName = u.FullName,
                           Point = sa.Point,
                           FilePath = @"\Images\" + a.FilePath,
                           Description = a.Description,
                       });
            AchievementsInformation = all.ToList();
        }
        public async void CreateBestStudentInformation( AchiveType achive)
        {
            if (_context.StudentsAchievements == null) return;
            var achiv = from sa in _context.StudentsAchievements
                        join a in _context.Achievement on sa.AchievementID equals a.ID
                        where a.AchievementType == achive
                        group sa by sa.StudentID into stud
                        select new
                        {
                            stud.Key,
                            Point = stud.Sum(x => x.Point)

                        };
            var bestStudent = from a in achiv
                              join s in _context.Student on a.Key equals s.ID
                              join u in _context.Account on s.AccountID equals u.ID
                              orderby a.Point descending    
                              select new AchievementInformation
                              {
                                  FullName = u.FullName,
                                  Point = a.Point
                              };
            AchievementsInformation = bestStudent.ToList();
        }
        public async Task<IActionResult> OnPostAsync(AchiveType typeAchiv, string typeGet)
        {

            if (typeGet == "t1")
            {
                CreateBestStudentInformation(typeAchiv);
                IsStudentAchivTableCreated = false;
                IsTopStudentTableCreated = true;
            }
            else if (typeGet == "t2")
            {
                List<Achievement> achievement = null;
                List<StudentsAchievements> studentsAchievements = null;


                CreateAchievementInformation(typeAchiv);
                IsStudentAchivTableCreated = true;
                IsTopStudentTableCreated = false;
            }
            return Page();

        }
    }
}
