using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
namespace VeloKom.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public static List<Ads> ads { get; set; }
        public static List<Categories> categories { get; set; }
        public static Users user;
        public IndexModel(ILogger<IndexModel> logger)
        {
            var context = new Database();
            _logger = logger;
            categories = context.Categories.ToList();
            ads = context.Ads.ToList();
        }
        public void OnGet(int? cat)
        {
            using (var context = new Database())
            {
                var token = JWT.GetToken(Request);
                if (token != null)
                {
                    user = context.Users.ToList().FirstOrDefault(u => u.id == Convert.ToInt32(JWT.GetUserIdFromToken(token)));
                }
                else
                    user = null;
            }
            if (cat == null)
            {
                cat = 7;
            }
            using(var context = new Database())
            {
                if (cat == 7 || cat == null)
                {
                    if (user == null)
                    {
                        ads = context.Ads.Where(ad=>ad.moderate == true).ToList();
                    }
                    else
                    {
                        ads = context.Ads.Where(ad => ad.userId != user.id && ad.moderate == true).ToList();
                    }
                }
                else
                {
                    if(user == null)
                    {
                        ads = context.Ads.Where(c => c.categoryId == cat && c.moderate == true).ToList();
                    }
                    else
                    {
                        ads = context.Ads.Where(c => c.categoryId == cat && user.id != c.userId && c.moderate == true).ToList();
                    }
                }
            }
            
        }
        public IActionResult OnPost(string like)
        {
            var dat = new Database();
            Ads ad = dat.Ads.ToList().FirstOrDefault(a => a.id == Convert.ToInt32(like));
            if (user == null)
            {
                return RedirectToPage("/Auth");
            }
            if (IsLiked(ad) == true)
            {
                using (var context = new Database())
                {
                    Likes likes = context.Likes.ToList().First(l => l.userId == user.id && l.adId == ad.id);
                    context.Likes.Remove(likes);
                    context.SaveChanges();
                }
            }
            else
            {
                Likes favor = new Likes();
                favor.adId = ad.id;
                favor.userId = user.id;
                using (var context = new Database())
                {
                    context.Likes.Add(favor);
                    context.SaveChanges();
                }
            }
            return RedirectToPage();
        }
        public static bool IsLiked(Ads ad)
        {
            bool flag = false;
            var database = new Database();
            Likes like = database.Likes.FirstOrDefault(l => l.adId == ad.id && l.userId == user.id);
            if(like != null)
            {
                flag = true;
            }
            return flag;
        }
    }
}
