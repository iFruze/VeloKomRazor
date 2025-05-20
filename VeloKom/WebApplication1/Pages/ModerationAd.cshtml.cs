using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;

namespace VeloKom.Pages
{
    public class ModerationAdModel : PageModel
    {
        public static Ads ad {  get; set; }
        public void OnGet(int? id)
        {
            ad = new Database().Ads.FirstOrDefault(a => a.id == id);

        }
        public static string GetUserById(int id) => new Database().Users.FirstOrDefault(u => u.id == id).login;

        public IActionResult OnPost(string? button)
        {
            if(button == "plus")
            {
                using(var context = new Database())
                {
                    var curAd = context.Ads.FirstOrDefault(a => a.id == ad.id);
                    curAd.moderate = true;
                    context.SaveChanges();
                }
                return RedirectToPage("/Moderation");
            }
            else
            {
                using (var context = new Database())
                {
                    var curAd = new Database().Ads.FirstOrDefault(a => a.id == ad.id);
                    context.Remove(curAd);
                    context.SaveChanges();
                }
                return RedirectToPage("/Moderation");
            }
        }
    }
}
