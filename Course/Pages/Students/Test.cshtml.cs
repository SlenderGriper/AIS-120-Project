using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic.FileIO;

namespace Course.Pages.Students
{
   
    public class TestModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TestModel(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public string Massage { get; set; }

        [BindProperty]
        public IFormFile? Image { get; set; }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (Image == null || Image.Length == 0)
            {
                Massage = "Файл пустой";
                return Page();
            }
            if (!IsImage(Image))
            {
                Massage = "Файл не картинка";
                return Page();
            }
              var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", Image.FileName);
            if (FileSystem.FileExists(filePath))
            {
                Massage = "Файл с таким именем уже существует";
                return Page();
            }
            Massage = "норм";
            using (var stream = new FileStream(filePath, FileMode.Create))
                {
                   // await Image.CopyToAsync(stream);
                }
            return Page();
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
    }
}
