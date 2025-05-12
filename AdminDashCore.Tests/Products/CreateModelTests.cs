using AdminDashCore.Models;
using AdminDashCore.Data;
using AdminDashCore.Pages.Admin.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminDashCore.Tests.Products
{
    public class CreateModelTests
    {
        private CreateModel GetCreateModelWithContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new AppDbContext(options);
            return new CreateModel(context);
        }

        [Fact]
        public async Task OnPost_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = GetCreateModelWithContext();
            model.Product = new Product
            {
                Name = "Test Product",
                Price = 100,
                Quantity = 10,
                CategoryId = 1  
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPost_InvalidModel_ReturnsPage()
        {
            // Arrange
            var model = GetCreateModelWithContext();
            model.Product = new Product
            {
                Name = "",  
                Price = 100,
                Quantity = 10,
                CategoryId = 1
            };

            model.ModelState.AddModelError("Product.Name", 
                "The Name field is required.");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPost_InvalidPrice_ReturnsPage()
        {
            // Arrange
            var model = GetCreateModelWithContext();
            model.Product = new Product
            {
                Name = "Valid Product",
                Price = -1,  
                Quantity = 10,
                CategoryId = 1
            };

            model.ModelState.AddModelError("Product.Price", 
                "The field Price must be between 0 and 1.7976931348623157E+308.");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPost_MissingCategoryId_ReturnsPage()
        {
            // Arrange
            var model = GetCreateModelWithContext();
            model.Product = new Product
            {
                Name = "Valid Product",
                Price = 100,
                Quantity = 10,
                CategoryId = 0 
            };

            model.ModelState.AddModelError("Product.CategoryId", 
                "The CategoryId field is required.");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
        }
    }
}
