using AdminDashCore.Data;
using AdminDashCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Pages.Admin.Messages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Message? Message { get; set; }
        public SelectList? ClientList { get; set; }

        public IActionResult OnGet(int id)
        {
            Message = _context.Messages.Include(m => m.Client).FirstOrDefault(m => m.Id == id);

            if (Message == null)
            {
                return NotFound();
            }

            ClientList = new SelectList(_context.Clients, "Id", "Name", Message.ClientId);
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ClientList = new SelectList(_context.Clients, "Id", "Name", Message?.ClientId);
                return Page();
            }

            _context.Messages.Update(Message!);
            _context.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
