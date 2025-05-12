using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminDashCore.Data;

namespace AdminDashCore.Pages.Admin.Products
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product? Product { get; set; }

        public SelectList? CategoryList { get; set; }

        public IActionResult OnGet(int id)
        {
            Product = _context.Products.Find(id);
            if (Product == null)
            {
                return NotFound();
            }

            CategoryList = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                CategoryList = new SelectList(_context.Categories, "Id", "Name");
                return Page();
            }

            _context.Attach(Product!).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}