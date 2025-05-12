using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Categories
{
    public class DeleteModelTests
    {
        private readonly AppDbContext _context;
        private readonly DeleteModel _pageModel;

        public DeleteModelTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Test Category"
            });
            _context.SaveChanges();

            _pageModel = new DeleteModel(_context);
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
            Assert.NotNull(_pageModel.Category);
            Assert.Equal("Test Category", _pageModel.Category!.Name);
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
        public async Task OnPostAsync_ValidCategory_RemovesCategoryAndRedirects()
        {
            // Arrange
            var id = 1;
            _pageModel.Category = await _context.Categories.FindAsync(id);

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);

            var deletedCategory = await _context.Categories.FindAsync(id);
            Assert.Null(deletedCategory);
        }

        [Fact]
        public async Task OnPostAsync_InvalidCategory_ReturnsRedirectToIndex()
        {
            // Arrange
            var id = 999; 
            _pageModel.Category = new Category { Id = id };

            // Act
            var result = await _pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);

            var deletedCategory = await _context.Categories.FindAsync(id);
            Assert.Null(deletedCategory);  
        }
    }
}
