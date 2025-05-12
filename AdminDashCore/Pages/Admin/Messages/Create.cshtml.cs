using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdminDashCore.Pages.Admin.Messages
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Message? Message { get; set; }
        public SelectList? ClientList { get; set; }

        public void OnGet()
        {
            ClientList = new SelectList(_context.Clients, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid)
            {
                ClientList = new SelectList(_context.Clients, "Id", "Name");
                return Page();
            }

            _context.Messages.Add(Message!);
            await _context.SaveChangesAsync(); 
            return RedirectToPage("Index");
        }

    }
}
