using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class UserAdsModel : PageModel
    {
        Users user;
        public static List<Ads> ads;
        public void OnGet()
        {
            var token = JWT.GetToken(Request);
            if(token != null)
            {
                using(var context = new Database())
                {
                    user = context.Users.FirstOrDefault(u => u.id == Convert.ToInt32(JWT.GetUserIdFromToken(token)));
                    ads = context.Ads.Where(ad => ad.userId == user.id && ad.moderate == true).ToList();
                }
            }
        }
    }
}
