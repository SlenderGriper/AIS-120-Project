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

        public StudentsInfoModel(Data.AchievementContext context)
        {
            _context = context;
        }
        public bool IsStudentAchivTableCreated { get; set; } = false;
        public bool IsTopStudentTableCreated { get; set; } = false;
        public IList<AchievementInformation> AchievementsInformation { get; set; } = default!;
        public async void CreateAchievementInformation(IList<Achievement> achievement, IList<StudentsAchievements> studentsAchievements)
        {
            if (achievement == null) return;
            if (studentsAchievements == null) return;
            var all = (from a in achievement
                       join sa in studentsAchievements on a.ID equals sa.AchievementID
                       join s in _context.Student on sa.StudentID equals s.ID
                       join u in _context.Account on s.AccountID equals u.ID
                       orderby sa.Point descending
                       select new AchievementInformation
                       {
                           FullName = u.FullName,
                           Point = sa.Point,
                           FilePath = sa.FilePath,
                           Description = a.Description,
                       });
            AchievementsInformation = all.ToList();
        }
        public async void CreateBestStudentInformation(IList<StudentsAchievements> studentsAchievements)
        {
            if (studentsAchievements == null) return;
            var achiv = from sa in studentsAchievements
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
                List<StudentsAchievements> studentsAchievements = null;
                studentsAchievements = AchievementContextHelper.StudentsAchievements(typeAchiv, _context);
                CreateBestStudentInformation(studentsAchievements);
                IsStudentAchivTableCreated = false;
                IsTopStudentTableCreated = true;
            }
            else if (typeGet == "t2")
            {
                List<Achievement> achievement = null;
                List<StudentsAchievements> studentsAchievements = null;
                if (typeAchiv == AchiveType.Social)
                {
                    achievement = _context.SocialAchievement.ToList<Achievement>();
                    studentsAchievements = _context.StudentsSocialAchievements.ToList<StudentsAchievements>();
                }
                else if (typeAchiv == AchiveType.Research)
                {
                    achievement = _context.ResearchAchievement.ToList<Achievement>();
                    studentsAchievements = _context.StudentsResearchAchievements.ToList<StudentsAchievements>();
                }
                else if (typeAchiv == AchiveType.Cultural)
                {
                    achievement = _context.CulturalAchievement.ToList<Achievement>();
                    studentsAchievements = _context.StudentsCulturalAchievements.ToList<StudentsAchievements>();
                }
                else if (typeAchiv == AchiveType.Sport)
                {
                    achievement = _context.SportAchievement.ToList<Achievement>();
                    studentsAchievements = _context.StudentsSportAchievements.ToList<StudentsAchievements>();
                }

                CreateAchievementInformation(achievement, studentsAchievements);
                IsStudentAchivTableCreated = true;
                IsTopStudentTableCreated = false;
            }
            return Page();

        }
    }
}
