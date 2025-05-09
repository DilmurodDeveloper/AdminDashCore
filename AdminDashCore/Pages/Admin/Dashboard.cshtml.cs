using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToPage("/Login");
            }

            ProductCount = _context.Products.Count();
            OrderCount = _context.Orders.Count();
            ClientCount = _context.Clients.Count();
            MessageCount = _context.Messages.Count();

            LatestClients = _context.Clients
                .OrderByDescending(c => c.StartDate)
                .Take(5)
                .ToList();

            return Page();
        }
    }
}
