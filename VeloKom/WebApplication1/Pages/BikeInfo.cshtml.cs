using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class BikeInfoModel : PageModel
    {
        private readonly ILogger<BikeInfoModel> _logger;
        public static Ads currentAd;
        public static Users currentUser;
        public static bool inLikes = false;
        public BikeInfoModel(ILogger<BikeInfoModel> logger)
        {
            _logger = logger;
        }
        public void OnGet(int? id)
        {
            using (var context = new Database())
            {
                currentAd = context.Ads.FirstOrDefault(a => a.id == id);
                var token = JWT.GetToken(Request);
                if (token != null)
                {
                    currentUser = context.Users.ToList().FirstOrDefault(u => u.id == Convert.ToInt32(JWT.GetUserIdFromToken(token)));
                    if(context.Likes.ToList().FirstOrDefault(like => like.userId == currentUser.id && like.adId == currentAd.id) != null)
                    {
                        inLikes = true;
                    }
                }
                else
                {
                    currentUser = null;
                }
            }
        }
        public IActionResult OnPost(string like)
        {
            if(currentUser == null)
            {
                return RedirectToPage("/Auth");
            }
            if(inLikes == true)
            {
                using(var context = new Database())
                {
                    Likes likes = context.Likes.ToList().First(l => l.userId == currentUser.id && l.adId == currentAd.id);
                    context.Likes.Remove(likes);
                    context.SaveChanges();
                    inLikes = false;
                }
            }
            else
            {
                Likes favor = new Likes();
                favor.adId = currentAd.id;
                favor.userId = currentUser.id;
                using(var context = new Database())
                {
                    context.Likes.Add(favor);
                    context.SaveChanges();
                }
                inLikes = true;
            }
            return RedirectToPage("/BikeInfo", new {id = currentAd.id});
        }
    }
}
