using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeloKom.Pages
{
    public class PanelModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnPost(string button)
        {
            if(button == "moderate")
            {
                return RedirectToPage("/Moderation");
            }
            else if(button == "users")
            {
                return RedirectToPage("/UserView");
            }
            else
            {
                return Page();
            }
        }
    }
}
