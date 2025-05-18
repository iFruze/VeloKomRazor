using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
namespace VeloKom.Pages
{
    public class AuthModel : PageModel
    {
        static Users user;
        public IActionResult OnGet()
        {
            ModelState.Clear();
            var token = JWT.GetToken(Request);
            if(token != null)
            {
                return RedirectToPage("/Profile");
            }
            return Page();
        }
        public IActionResult OnPost(string login, string password)
        {
            var hashPass = BCrypt.Net.BCrypt.HashPassword(password);
            using (var context = new Database())
            {
                user = context.Users.ToList().FirstOrDefault(u=> u.login == login && BCrypt.Net.BCrypt.Verify(password, u.password));
            }
            if (user != null)
            {
                var token = JWT.GenerateToken(user);
                JWT.SaveToken(Response, token);
                return RedirectToPage("/Profile");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
                return Page();
            }
        }
    }
}
