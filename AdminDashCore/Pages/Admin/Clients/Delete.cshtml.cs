using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Data;
using AdminDashCore.Models;

namespace AdminDashCore.Pages.Admin.Clients;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Client? Client { get; set; }  

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Client = await _context.Clients.FindAsync(id);
        if (Client == null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
