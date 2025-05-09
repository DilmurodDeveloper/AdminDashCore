using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Models;
using AdminDashCore.Data;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Clients;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Client> Clients { get; set; } = [];

    public async Task OnGetAsync()
    {
        Clients = await _context.Clients.ToListAsync();
    }
}
