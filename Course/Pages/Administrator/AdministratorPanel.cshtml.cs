using Course.Data;
using Course.Model.Database.Enum;
using Course.Model.DatabaseTables.Achievement;
using Course.Model.DatabaseTables.StudentAchivement;
using Course.Model.PageItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Course.Pages.Administrator
{
    [Authorize]
    public class AdministratorPanelModel : PageModel
    {
        private readonly AchievementContext _context;
        public AdministratorPanelModel(AchievementContext context)
        {
            _context = context;
        }
        public IList<AchievementInformation> AchievementsInformation { get; set; }
        public IEnumerable<AchievementInformation> CreateAchievementInformation(IList<Achievement> achievement, IList<StudentsAchievements> studentsAchievements)
        {
            if (achievement == null) return null;
            if (studentsAchievements == null) return null;
            IEnumerable<AchievementInformation> all = from a in achievement
                                                      join sa in studentsAchievements on a.ID equals sa.AchievementID
                                                      join s in _context.Student on sa.StudentID equals s.ID
                                                      join u in _context.Account on s.AccountID equals u.ID
                                                      where a.WhoСonfirmed == null
                                                      group u.FullName by new { a.ID, a.Description, a.WhoСonfirmed } into g
                                                      select new AchievementInformation
                                                      {
                                                          Id = g.Key.ID,
                                                          AchievementType = achievement[0].AchievementType(),
                                                          FullName = string.Join(", ", g),
                                                          Description = g.Key.Description,
                                                          WhoСonfirmed = g.Key.WhoСonfirmed
                                                      };
            return all;
        }

        public async void OnGetAsync()
        {
            IEnumerable<AchievementInformation> achievementInformation = null;
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
        public async void OnPostAsycn(string action,int id)
        {
            //switch (action)
            //{
            //    case"confirm":  
            //        break;
            //    case "delete":
            //        break;
            //}
            Console.WriteLine(action + id);
        }
    }
}
