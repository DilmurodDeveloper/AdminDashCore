using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        public Order? Order { get; set; }

        public IActionResult OnGet(int id)
        {
            Order = _context.Orders.Include(o => o.Client).FirstOrDefault(o => o.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
