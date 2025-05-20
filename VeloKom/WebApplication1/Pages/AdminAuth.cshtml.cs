using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class AdminAuthModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost(string login, string password)
        {
            
            if (login == "Admin" && password == "admin111")
            {
                return RedirectToPage("/Panel");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                return Page();
            }
        }
    }
}
