using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection.Metadata.Ecma335;

namespace VeloKom.Pages
{
    public class AddAdverModel : PageModel
    {
        public static List<Categories> categories;
        public static Users user;
        public void OnGet()
        {
            categories = new List<Categories>();
            using(var context = new Database())
            {
                foreach(var cat in context.Categories.ToList())
                {
                    if(cat.id == 7)
                    {
                        continue;
                    }
                    categories.Add(cat);
                }
                var token = JWT.GetToken(Request);
                if (token != null)
                {
                    user = context.Users.ToList().FirstOrDefault(u => u.id == Convert.ToInt32(JWT.GetUserIdFromToken(token)));
                }
            }
        }
        [BindProperty]
        public IFormFile Img { get;set; }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public int SelectedCatId { get; set; }
        [BindProperty]
        public string Cost { get; set; }
        [BindProperty]
        public string Contact { get; set; }
        public string ImagePath {  get; set; }
        public IActionResult OnPost()
        {
            string newFileName = "";
            if(Img != null && Img.Length > 0)
            {
                newFileName = $"{Guid.NewGuid()}_{Path.GetFileName(Img.FileName)}";
                var filePath = Path.Combine("wwwroot/img", newFileName);
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    Img.CopyTo(stream);
                }
                ImagePath = $"/img/{newFileName}";
            }
            string regex = @"^\+375\s?(29|33|44|25)\s?\d{3}\s?\d{2}\s?\d{2}$";
            if(Title.Length <= 2)
            {
                ModelState.AddModelError(string.Empty, "Некорректное название.");
            }
            if (Description.Length <= 5)
            {
                ModelState.AddModelError(string.Empty, "Некорректное описание.");
            }
            if (!Regex.IsMatch(Contact, regex) || Contact.Length > 13)
            {
                ModelState.AddModelError(string.Empty, "Некорректный номер телефона.");
            }
            if (!ViewData.ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                Ads ad = new Ads();
                ad.title = Title;
                ad.description = Description;
                ad.contacts = Contact;
                ad.categoryId = SelectedCatId;
                Cost = Cost.Replace('.', ',');
                ad.cost = Convert.ToDouble(Cost);
                ad.image = newFileName;
                ad.userId = user.id;
                ad.moderate = false;
                using (var context = new Database())
                {
                    context.Ads.Add(ad);
                    context.SaveChanges();
                }
                TempData["Message"] = Encoding.Unicode.GetString(Encoding.Unicode.GetBytes("Объявление успешно добавлено!"));
                return RedirectToPage();
            }
        }
    }
}
