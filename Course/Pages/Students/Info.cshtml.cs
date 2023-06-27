
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
        public bool isDraft { get; set; }
        private readonly AchievementContext _context;
        public InfoModel(AchievementContext context)
        {
            _context = context;
        }
        public IList<AchievementInformation> AchievementsInformation { get; set; } = default!;
        public IEnumerable<AchievementInformation> CreateAchievementInformation(IList<Achievement> achievement, IList<StudentsAchievements> studentsAchievements)
        {
            isDraft = false;
            if (achievement == null) return null;
            if (studentsAchievements == null) return null;
            int? id= _context.Student.Where(f => f.AccountID.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault()?.ID;
            IEnumerable<AchievementInformation> all = from sa in studentsAchievements
                                                      join a in achievement on sa.AchievementID equals a.ID
                                                      where sa.StudentID == id
                                                      where a.WhoСonfirmed != null
            select new AchievementInformation
                      {
                          AchievementType=a.AchievementType,
                          Point = (double)sa.Point,
                          FilePath = @"\Images\" + a.FilePath,
                          Description = a.Description,
                           Status = a.Status,
                          WhoСonfirmed = a.WhoСonfirmed
                      };
            Console.WriteLine(all.ToList().Count);
            return all;
        }
        public IEnumerable<AchievementInformation> CreateDraftAchievementInformation(IList<Achievement> achievement, IList<StudentsAchievements> studentsAchievements)
        {
            isDraft = true;
            if (achievement == null) return null;
            if (studentsAchievements == null) return null;
            int? id = _context.Student.Where(f => f.AccountID.ToString() == User.FindFirst(ClaimTypes.NameIdentifier).Value).FirstOrDefault()?.ID;
            IEnumerable<AchievementInformation> all = from sa in studentsAchievements
                                                      join a in achievement on sa.AchievementID equals a.ID
                                                      where sa.StudentID == id
                                                      where a.WhoСonfirmed == null
                                                      select new AchievementInformation
                                                      {
                                                          Id = a.ID,
                                                          AchievementType = a.AchievementType,
                                                          Description = a.Description,
                                                         Status = a.Status,
                                                         FailMessage=sa?.FailMessage
                                                      };
            Console.WriteLine(all.ToList().Count);
            return all;
        }
        public void OnGet(string param)
        {
            IEnumerable<AchievementInformation> achievementInformation=null;
            IList<Achievement> achievement;
            IList<StudentsAchievements> studentsAchievements;

            achievement = _context.Achievement.ToList<Achievement>();
            studentsAchievements = _context.StudentsAchievements.ToList<StudentsAchievements>();
            if (param == "Draft")
            {
                achievementInformation = CreateDraftAchievementInformation(achievement, studentsAchievements);
            }
            else
            {
                achievementInformation = CreateAchievementInformation(achievement, studentsAchievements);
            }
            AchievementsInformation = achievementInformation.ToList();

        }
    }
}
