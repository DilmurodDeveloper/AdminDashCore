using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Messages;
using Microsoft.AspNetCore.Mvc;
using AdminDashCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminDashCore.Tests.Messages
{
    public class CreateModelTests
    {
        private readonly CreateModel _pageModel;
        private readonly AppDbContext _context;

        public CreateModelTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbContext(options);
            _pageModel = new CreateModel(_context);

            if (!_context.Clients.Any())
            {
                _context.Clients.Add(new Client { Id = 1, Name = "Client 1" });
                _context.SaveChanges();
            }
        }

        [Fact]
        public async Task OnPost_InvalidModel_ReturnsPage()
        {
            // Arrange
            _pageModel.Message = new Message
            {
                Content = "",
                ClientId = 1
            };
            _pageModel.ModelState.AddModelError("Content", "Content is required");

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPost_ValidModel_RedirectsToIndex()
        {
            // Arrange
            _pageModel.Message = new Message
            {
                Content = "This is a valid message",
                ClientId = 1
            };

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}