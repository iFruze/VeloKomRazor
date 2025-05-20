using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class ModerationModel : PageModel
    {
        public static List<Ads> ads = new List<Ads>();
        public void OnGet()
        {
            using(var context = new Database())
            {
                ads = context.Ads.Where(ad => ad.moderate == false).ToList();
            }
        }
        public static string GetUserById(int id) => new Database().Users.FirstOrDefault(u => u.id == id).login;
        
        public IActionResult OnPost(string exit)
        {
            return RedirectToPage("/Panel");
        }
    }
}
