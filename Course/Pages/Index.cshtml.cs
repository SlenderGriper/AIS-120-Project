using Course.Model.DatabaseTables;
using Course.Model.Password;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Course.Model.Database.Enum;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Course.Pages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        private readonly Data.AchievementContext _context;
        [BindProperty]
        public InputItem Input { get; set; }

        public class InputItem
        {
            [Required]
            [Display(Name = "Имя")]
            public string? FullName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string? HashPassword { get; set; }
        }
        public IndexModel(Data.AchievementContext context)
        {
            _context=context;
        }
        
       
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/AccountPage/AccountCookie");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            if (ModelState.IsValid)
            {
                var user = _context.Account.Where(f => f.FullName == Input.FullName && f.HashPassword == HashPassword.CreatePasswordHash(Input.HashPassword)).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                    return Page();
                }
                

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Role,user.AccessRights.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties { IsPersistent = true });


                return RedirectToPage("/AccountPage/AccountCookie");
            }
            return Page();
        }
    }
}