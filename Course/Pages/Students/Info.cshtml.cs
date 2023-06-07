
using Course.Data;
using Course.Model.Database.Enum;
using Course.Model.DatabaseTables.Achievement;
using Course.Model.DatabaseTables.StudentAchivement;
using Course.Model.PageItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Security.Claims;

namespace Course.Pages.StudentOpportunities
{
    [Authorize]
    public class InfoModel : PageModel
    {
        private readonly AchievementContext _context;
        public InfoModel(AchievementContext context)
        {
            _context = context;
        }
        public IList<AchievementInformation> AchievementsInformation { get; set; } = default!;
        public IEnumerable<AchievementInformation> CreateAchievementInformation(IList<Achievement> achievement, IList<StudentsAchievements> studentsAchievements)
        {
            if (achievement == null) return null;
            if (studentsAchievements == null) return null;
            int? id= _context.Student.Where(f => f.AccountID.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault()?.ID;
            IEnumerable<AchievementInformation> all = from sa in studentsAchievements
                                                      join a in achievement on sa.AchievementID equals a.ID
                                                      where sa.StudentID == id
                                                      where a.WhoСonfirmed != null
            select new AchievementInformation
                      {
                          AchievementType=a.AchievementType(),
                          Point = sa.Point,
                          FilePath = sa.FilePath,
                          Description = a.Description,
                          WhoСonfirmed = a.WhoСonfirmed
                      };
            Console.WriteLine(all.ToList().Count);
            return all;
        }

        public void OnGet()
        {
            IEnumerable<AchievementInformation> achievementInformation=null;
            IList<Achievement> achievement;
            IList<StudentsAchievements> studentsAchievements;

            achievement = _context.SocialAchievement.ToList<Achievement>();
            studentsAchievements = _context.StudentsSocialAchievements.ToList<StudentsAchievements>();
            achievementInformation = CreateAchievementInformation(achievement, studentsAchievements);

            achievement = _context.ResearchAchievement.ToList<Achievement>();
            studentsAchievements = _context.StudentsResearchAchievements.ToList<StudentsAchievements>();
            achievementInformation = achievementInformation.Union(CreateAchievementInformation(achievement, studentsAchievements));

            achievement = _context.CulturalAchievement.ToList<Achievement>();
            studentsAchievements = _context.StudentsCulturalAchievements.ToList<StudentsAchievements>();
            achievementInformation = achievementInformation.Union(CreateAchievementInformation(achievement, studentsAchievements));

            achievement = _context.SportAchievement.ToList<Achievement>();
            studentsAchievements = _context.StudentsSportAchievements.ToList<StudentsAchievements>();
            achievementInformation = achievementInformation.Union(CreateAchievementInformation(achievement, studentsAchievements));
            AchievementsInformation = achievementInformation.ToList();

        }
    }
}
