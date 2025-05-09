using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Models;
using AdminDashCore.Data;

namespace AdminDashCore.Pages.Admin.Clients;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Client Client { get; set; } = new();

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Clients.Add(Client);
        await _context.SaveChangesAsync();
        return RedirectToPage("Index");
    }
}
