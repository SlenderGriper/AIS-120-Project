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
                                                      where a.Status == AchiveStatus.Sent
                                                      group u.FullName by new { a.ID, a.Description, a.WhoСonfirmed,a.AchievementType } into g
                                                      select new AchievementInformation
                                                      {
                                                          Id = g.Key.ID,
                                                          AchievementType = g.Key.AchievementType,
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

            achievement = _context.Achievement.ToList();
            studentsAchievements = _context.StudentsAchievements.ToList();
            achievementInformation = CreateAchievementInformation(achievement, studentsAchievements);

            AchievementsInformation = achievementInformation.ToList();

        }
    }
}
