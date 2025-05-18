using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class UserCurrentAdModel : PageModel
    {
        public static Ads currentAd;
        public static Users currentUser;
        public void OnGet(int? id)
        {
            using (var context = new Database())
            {
                currentAd = context.Ads.FirstOrDefault(a => a.id == id);
                var token = JWT.GetToken(Request);
                if (token != null)
                {
                    currentUser = context.Users.ToList().FirstOrDefault(u => u.id == Convert.ToInt32(JWT.GetUserIdFromToken(token)));
                }
            }
        }
    }
}
