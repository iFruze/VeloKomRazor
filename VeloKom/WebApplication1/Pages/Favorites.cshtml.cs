using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class FavoritesModel : PageModel
    {
        public static Users user; 
        public static List<Ads> favAds;
        public void OnGet()
        {
            var token = JWT.GetToken(Request);
            int userId = Convert.ToInt32(JWT.GetUserIdFromToken(token));
            user = new Database().Users.FirstOrDefault(u=> u.id == userId);
            var likes = new Database().Likes.Where(like => like.userId == user.id).ToList();
            var adsId = likes.Select(like => like.adId).ToList();
            favAds = new List<Ads>();
            foreach(var id in adsId)
            {
                Ads ad = new Database().Ads.FirstOrDefault(u => u.id == id);
                favAds.Add(ad);
            }
        }
        public IActionResult OnPost(string? like, string? cost)
        { 
            if (like == null)
            {
                if(cost == null)
                {
                    return Page();
                }
                var likes = new Database().Likes.Where(like => like.userId == user.id).ToList();
                var adsId = likes.Select(like => like.adId).ToList();
                favAds = new List<Ads>();
                foreach (var id in adsId)
                {
                    Ads ad = new Database().Ads.FirstOrDefault(u => u.id == id);
                    if(ad.cost <= Convert.ToDouble(cost))
                        favAds.Add(ad);
                }
                return Page();
            }
            else
            {
                using (var context = new Database())
                {
                    Likes likes = context.Likes.ToList().First(l => l.userId == user.id && l.adId == Convert.ToInt32(like));
                    context.Likes.Remove(likes);
                    context.SaveChanges();
                }
                return RedirectToPage();
            }
        }
    }
}
