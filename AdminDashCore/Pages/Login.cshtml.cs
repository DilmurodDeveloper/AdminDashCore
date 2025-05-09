using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminDashCore.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public User User { get; set; }

    public string ErrorMessage { get; set; }

    public void OnGet()
    {
        HttpContext.Session.Clear();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (User.Username == "admin" && User.Password == "123456")
        {
            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("Username", User.Username);

            return RedirectToPage("/Admin/Dashboard");
        }

        ErrorMessage = "The username or password is incorrect.";
        return Page();
    }
}
