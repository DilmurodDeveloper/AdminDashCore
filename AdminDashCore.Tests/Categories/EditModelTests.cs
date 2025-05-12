using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Categories
{
    public class EditModelTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public EditModelTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(_options);
            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Test Category"
            });
            context.SaveChanges();
        }

        [Fact]
        public async Task OnGetAsync_ValidId_ReturnsPageResult()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var model = new EditModel(context);

            // Act
            var result = await model.OnGetAsync(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(model.Category);
            Assert.Equal("Test Category", model.Category!.Name);
        }

        [Fact]
        public async Task OnGetAsync_InvalidId_ReturnsNotFound()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var model = new EditModel(context);

            // Act
            var result = await model.OnGetAsync(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_InvalidModel_ReturnsPage()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var model = new EditModel(context);
            model.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostAsync_ValidModel_UpdatesCategoryAndRedirects()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var model = new EditModel(context)
            {
                Category = new Category
                {
                    Id = 1,
                    Name = "Updated Category"
                }
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);

            var updated = await context.Categories.FindAsync(1);
            Assert.NotNull(updated);
            Assert.Equal("Updated Category", updated!.Name);
        }

        [Fact]
        public async Task OnPostAsync_CategoryNotFound_ReturnsNotFound()
        {
            // Arrange
            using var context = new AppDbContext(_options);
            var model = new EditModel(context)
            {
                Category = new Category
                {
                    Id = 999,
                    Name = "Nonexistent"
                }
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
