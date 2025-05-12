using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AdminDashCore.Data;

namespace AdminDashCore.Tests.Messages
{
    public class EditModelTests
    {
        private readonly EditModel _editModel;
        private readonly AppDbContext _context;

        public EditModelTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                          .UseInMemoryDatabase(databaseName: "TestDb")
                          .Options;
            _context = new AppDbContext(options);

            _editModel = new EditModel(_context);
        }

        [Fact]
        public async Task OnGetAsync_MessageNotFound_ReturnsNotFound()
        {
            // Arrange
            using var context = CreateContext();
            var editModel = new EditModel(context);

            // Act
            var result = await editModel.OnGetAsync(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ValidMessage_ReturnsRedirectToPage()
        {
            // Arrange
            using var context = CreateContext();
            var editModel = new EditModel(context);

            var message = new Message { Id = 1, Content = "Initial", IsRead = false };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            context.Entry(message).State = EntityState.Detached;

            editModel.Message = new Message { Id = 1, Content = "Updated", IsRead = true };

            // Act
            var result = await editModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }


        [Fact]
        public async Task OnPostAsync_InvalidModelState_ReturnsPage()
        {
            // Arrange
            _editModel.ModelState.AddModelError("Content", "Content is required.");
            _editModel.Message = new Message();

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

    }
}
