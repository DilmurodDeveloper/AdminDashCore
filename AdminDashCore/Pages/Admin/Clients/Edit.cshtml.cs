using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Clients;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Client? Client { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Client = await _context.Clients.FindAsync(id);

        if (Client == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        _context.Attach(Client).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Clients.Any(e => e.Id == Client.Id))
                return NotFound();
            else
                throw;
        }

        return RedirectToPage("Index");
    }
}
