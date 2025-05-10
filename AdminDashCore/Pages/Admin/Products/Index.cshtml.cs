using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Products
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Product>? Products { get; set; }

        public void OnGet()
        {
            Products = _context.Products
                               .Include(p => p.Category)
                               .ToList();
        }
    }
}
