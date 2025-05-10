using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using AdminDashCore.Data;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Product Product { get; set; } = new();

    public SelectList? CategoryList { get; set; }

    public void OnGet()
    {
        CategoryList = new SelectList(_context.Categories, "Id", "Name");
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            CategoryList = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        _context.Products.Add(Product);
        _context.SaveChanges();
        return RedirectToPage("Index");
    }
}
