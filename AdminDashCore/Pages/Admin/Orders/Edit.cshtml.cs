using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Orders
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Order? Order { get; set; }
        public SelectList? ClientList { get; set; }

        public IActionResult OnGet(int id)
        {
            Order = _context.Orders.Include(o => o.Client).FirstOrDefault(o => o.Id == id);

            if (Order == null)
            {
                return NotFound();
            }

            ClientList = new SelectList(_context.Clients, "Id", "Name", Order.ClientId);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ClientList = new SelectList(_context.Clients, "Id", "Name", Order?.ClientId);
                return Page();
            }

            _context.Orders.Update(Order!);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
