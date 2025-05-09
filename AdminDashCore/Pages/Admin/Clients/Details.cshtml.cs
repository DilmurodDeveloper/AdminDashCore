using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Data;
using AdminDashCore.Models;

namespace AdminDashCore.Pages.Admin.Clients;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public Client? Client { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Client = await _context.Clients.FindAsync(id);
        if (Client == null) return NotFound();

        return Page();
    }
}
