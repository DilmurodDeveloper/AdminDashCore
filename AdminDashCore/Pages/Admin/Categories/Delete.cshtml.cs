using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminDashCore.Pages.Admin.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category? Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _context.Categories.FindAsync(id);
            if (Category == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var category = await _context.Categories.FindAsync(Category?.Id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}
