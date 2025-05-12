using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Clients
{
    public class DeleteModelTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public DeleteModelTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Seed data
            using var context = new AppDbContext(_options);
            context.Clients.Add(new Client
            {
                Id = 1,
                Name = "Test Client",
                Position = "Dev",
                Office = "HQ",
                Age = 25,
                StartDate = new DateTime(2021, 1, 1),
                Salary = 1500
            });
            context.SaveChanges();
        }

        [Fact]
        public async Task OnGetAsync_ValidId_ReturnsPageResult()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var pageModel = new DeleteModel(context);

            // Act
            var result = await pageModel.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.Client);
            Assert.Equal("Test Client", pageModel.Client!.Name);
        }

        [Fact]
        public async Task OnGetAsync_InvalidId_ReturnsNotFound()
        {
            using var context = new AppDbContext(_options);
            var pageModel = new DeleteModel(context);

            var result = await pageModel.OnGetAsync(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ValidId_DeletesClientAndRedirects()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var pageModel = new DeleteModel(context);

            // Act
            var result = await pageModel.OnPostAsync(1);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            var deletedClient = await context.Clients.FindAsync(1);
            Assert.Null(deletedClient);
        }

        [Fact]
        public async Task OnPostAsync_InvalidId_ReturnsNotFound()
        {
            using var context = new AppDbContext(_options);
            var pageModel = new DeleteModel(context);

            var result = await pageModel.OnPostAsync(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
