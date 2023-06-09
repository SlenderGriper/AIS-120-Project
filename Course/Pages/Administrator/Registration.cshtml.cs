using Course.Model.DatabaseTables;
using Course.Model.Password;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Course.Pages.Administrator
{
    [Authorize]
    public class RegistrationModel : PageModel
    {
        private readonly Course.Data.AchievementContext _context;
        public string Massage { get; set; }
        public RegistrationModel(Course.Data.AchievementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Account Input{ get; set; }
        public async Task OnGetAsync()
        {

        }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine(ModelState);
            if (ModelState.IsValid)
            {
                var user = _context.Account.Where(f => f.FullName == Input.FullName).FirstOrDefault();
                if (user != null)
                {
                    Massage= Input.FullName + " Alrready exists";
                }
                else
                {
                    Input.HashPassword = HashPassword.CreatePasswordHash(Input.HashPassword);
                    Input.ID=_context.Account.Max(u => u.ID)+1;
                    Console.WriteLine(Input.ID);
                     _context.Account.Add(Input);
                    await _context.SaveChangesAsync();

                    if (Input.AccessRights == Model.Database.Enum.Roles.Student)
                    {
                        var stud = new Student();
                        stud.ID = _context.Student.Max(u => u.ID + 1);
                        stud.AccountID = Input.ID;
                        _context.Student.Add(stud);
                        await _context.SaveChangesAsync();
                    }
                    Massage = Input.FullName + "Create";
                }

            }
            return Page();
        }
    }
}
