using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class ProfileModel : PageModel
    {
        public static Users user;
        public void OnGet()
        {
            using(var context = new Database())
            {
                user = context.Users.ToList().FirstOrDefault(u=> u.id == Convert.ToInt32(JWT.GetUserIdFromToken(JWT.GetToken(Request))));
            }
        }
        public IActionResult OnPost(string action)
        {
            switch (action)
            {
                case "addAdver":
                    return RedirectToPage("/AddAdver");
                    break;
                case "myAds":
                    return RedirectToPage("/UserAds");
                    break;
                case "likes":
                    return RedirectToPage("/Favorites");
                    break;
                case "exit":
                    JWT.DeleteToken(Response);
                    return RedirectToPage("/Index");
                    break;
                default:
                    return Page();
                    break;
            }
        }
    }
}
