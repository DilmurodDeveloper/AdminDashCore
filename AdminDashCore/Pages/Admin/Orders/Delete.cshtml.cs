using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Orders
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public IActionResult OnPost()
        {
            var order = _context.Orders.Find(Order?.Id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
