using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Category>? Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync();
        }
    }
}
