using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminDashCore.Data;
using AdminDashCore.Models;

namespace AdminDashCore.Pages.Admin.Clients;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Client> Clients { get; set; } = new();

    public async Task OnGetAsync()
    {
        Clients = await _context.Clients.ToListAsync();
    }
}
