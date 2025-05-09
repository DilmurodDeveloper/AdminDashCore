using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public DashboardModel(AppDbContext context)
        {
            _context = context;
        }

        public int ProductCount { get; set; }
        public int OrderCount { get; set; }
        public int ClientCount { get; set; }
        public int MessageCount { get; set; }

        public List<Client>? LatestClients { get; set; }

        public async Task OnGetAsync()
        {
            ProductCount = await _context.Products.CountAsync();
            OrderCount = await _context.Orders.CountAsync();
            ClientCount = await _context.Clients.CountAsync();
            MessageCount = await _context.Messages.CountAsync();

            LatestClients = await _context.Clients
                .OrderByDescending(c => c.StartDate)
                .Take(5)
                .ToListAsync();
        }
    }
}
