using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
using System.Text;
namespace VeloKom.Pages
{
    public class RepisterationModel : PageModel
    {
        public void OnGet()
        {}
        public IActionResult OnPost(string? login, string? pass, string? secondPass)
        {
            if(login == null ||  pass == null || secondPass == null)
            {
                ModelState.AddModelError(string.Empty, "Заполните все поля.");
                return Page();
            }
            Users user;
            using(var context = new Database())
            {
                user = context.Users.FirstOrDefault(u => u.login.ToString() == login);
            }
            if(user != null)
            {
                ModelState.AddModelError(string.Empty, "Данный логин уже занят.");
                return Page();
            }
            if(pass != secondPass)
            {
                ModelState.AddModelError(string.Empty, "Введённые пароли не совпадают.");
                return Page();
            }
            if(pass.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "Длина пароля должна составлять не менее 6 символов.");
                return Page();
            }
            user = new Users();
            user.login = login;
            user.password = BCrypt.Net.BCrypt.HashPassword(pass);
            using(var context = new Database())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
            return RedirectToPage("/Auth");
        }
    }
}
