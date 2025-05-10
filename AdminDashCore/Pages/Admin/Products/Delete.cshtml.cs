using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using AdminDashCore.Data;
using Microsoft.EntityFrameworkCore;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product? Product { get; set; }

    public IActionResult OnGet(int id)
    {
        Product = _context.Products
            .Include(p => p.Category)
            .FirstOrDefault(p => p.Id == id);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        var productToDelete = _context.Products.Find(Product?.Id);
        if (productToDelete == null)
        {
            return NotFound();
        }

        _context.Products.Remove(productToDelete);
        _context.SaveChanges();
        return RedirectToPage("Index");
    }
}
