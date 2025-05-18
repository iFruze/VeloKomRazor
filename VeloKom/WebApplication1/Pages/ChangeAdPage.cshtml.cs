using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace VeloKom.Pages
{
    public class ChangeAdPageModel : PageModel
    {
        public static Ads currentAd;
        public static Users currentUser;
        public static List<Categories> categories;
        public static List<string> errors;
        public void OnGet(int? id, List<string>? er)
        {
            if(er != null)
            {
                errors = er;
            }
            else
            {
                errors = new List<string>();
            }
            using (var context = new Database())
            {
                categories = new List<Categories>();
                foreach (var cat in context.Categories.ToList())
                {
                    if (cat.id == 7)
                    {
                        continue;
                    }
                    categories.Add(cat);
                }
                currentAd = context.Ads.FirstOrDefault(a => a.id == id);
                var token = JWT.GetToken(Request);
                if (token != null)
                {
                    currentUser = context.Users.ToList().FirstOrDefault(u => u.id == Convert.ToInt32(JWT.GetUserIdFromToken(token)));
                }
            }
            if (currentAd != null)
            {
                Title = currentAd.title;
                Description = currentAd.description;
                SelectedCatId = currentAd.categoryId;
                Cost = currentAd.cost.ToString().Replace(',', '.');
                Contact = currentAd.contacts;
                ImagePath = $"img/{currentAd.image}";
            }
        }
        [BindProperty]
        public IFormFile? Img { get; set; }
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
        public string ImagePath { get; set; }
        public IActionResult OnPost(string action)
        {
            switch (action)
            {
                case "saveAd":
                    errors = new List<string>();
                    string newFileName = "";
                    if (Img != null && Img.Length > 0)
                    {
                        newFileName = $"{Guid.NewGuid()}_{Path.GetFileName(Img.FileName)}";
                        var filePath = Path.Combine("wwwroot/img", newFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            Img.CopyTo(stream);
                        }
                        ImagePath = $"/img/{newFileName}";
                    }
                    string regex = @"^\+375\s?(29|33|44|25)\s?\d{3}\s?\d{2}\s?\d{2}$";
                    if (Title.Length <= 2)
                    {
                        errors.Add("Некорректное название.");
                    }
                    if (Description.Length <= 5)
                    {
                        errors.Add("Некорректное описание.");
                    }
                    if (!Regex.IsMatch(Contact, regex) || Contact.Length > 13)
                    {
                        errors.Add("Некорректный номер телефона.");
                    }
                    Cost = Cost.Replace('.', ',');
                    var arr = Cost.Split(',');
                    if(arr.Length > 1)
                    {
                        if (arr[0].Length > 6 || arr[1].Length > 2)
                        {
                            errors.Add("Некорректная цена.");
                        }
                    }
                    if (errors.Count > 0)
                    {
                        return RedirectToPage("/ChangeAdPage", new { id = currentAd.id, er=errors });
                    }
                    else
                    {
                        using (var context = new Database())
                        {
                            var ad = context.Ads.FirstOrDefault(a => a.id == currentAd.id);
                            ad.title = Title;
                            ad.description = Description;
                            ad.contacts = Contact;
                            ad.categoryId = SelectedCatId;
                            ad.cost = Convert.ToDouble(Cost);
                            if(newFileName != "")
                            {
                                ad.image = newFileName;
                            }
                            context.SaveChanges();
                        }
                        TempData["Message"] = "Успешно";
                        return RedirectToPage("/UserAds");
                    }
                    break;
                case "delAd":
                    using(var context = new Database())
                    {
                        context.Ads.Remove(currentAd);
                        context.SaveChanges();
                    }
                    return RedirectToPage("/UserAds");
                    break;
                default:
                    return Page();
            }
        }
    }
}
