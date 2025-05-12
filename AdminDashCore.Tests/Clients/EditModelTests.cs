using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Clients
{
    public class EditModelTests
    {
        private readonly AppDbContext _context;
        private readonly EditModel _pageModel;
        private readonly DbContextOptions<AppDbContext> _options;

        public EditModelTests()
        {
                 _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(_options);

            _context.Clients.Add(new Client
            {
                Id = 1,
                Name = "Test Name",
                Position = "Developer",
                Office = "HQ",
                Age = 30,
                StartDate = new DateTime(2020, 1, 1),
                Salary = 1000m
            });
            _context.SaveChanges();

            _pageModel = new EditModel(_context);
        }

        [Fact]
        public async Task OnGetAsync_ValidId_ReturnsPageResult()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _pageModel.OnGetAsync(id);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(_pageModel.Client);
            Assert.Equal("Test Name", _pageModel.Client!.Name);
        }

        [Fact]
        public async Task OnGetAsync_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _pageModel.OnGetAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_InvalidModel_ReturnsPage()
        {
            // Arrange
            _pageModel.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ValidModel_UpdatesClientAndRedirects()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var existingClient = await context.Clients.FindAsync(1);

            var pageModel = new EditModel(context)
            {
                Client = existingClient!
            };

            pageModel.Client.Name = "Updated Name";
            pageModel.Client.Position = "Senior Dev";
            pageModel.Client.Office = "Remote";
            pageModel.Client.Age = 35;
            pageModel.Client.StartDate = new DateTime(2022, 5, 10);
            pageModel.Client.Salary = 2000m;

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);

            var updated = await context.Clients.FindAsync(1);
            Assert.Equal("Updated Name", updated!.Name);
            Assert.Equal("Senior Dev", updated.Position);
            Assert.Equal("Remote", updated.Office);
            Assert.Equal(35, updated.Age);
            Assert.Equal(new DateTime(2022, 5, 10), updated.StartDate);
            Assert.Equal(2000m, updated.Salary);
        }
    }
}