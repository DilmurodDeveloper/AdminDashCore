using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Products
{
    public class DeleteModelTests
    {
        private AppDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            var context = new AppDbContext(options);

            context.Categories.Add(new Category { Id = 1, Name = "Category 1" });
            context.Products.Add(new Product { Id = 1, Name = "Product 1", CategoryId = 1 });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public void OnGet_ValidProductId_ReturnsPageResult()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var deleteModel = new DeleteModel(context);

            var productId = 1;

            // Act
            var result = deleteModel.OnGet(productId);

            // Assert
            var pageResult = Assert.IsType<PageResult>(result);
            Assert.Equal("Product 1", deleteModel.Product?.Name);
        }

        [Fact]
        public void OnGet_InvalidProductId_ReturnsNotFound()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var deleteModel = new DeleteModel(context);

            var invalidProductId = 999;

            // Act
            var result = deleteModel.OnGet(invalidProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void OnPost_ValidProductId_DeletesProductAndRedirects()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var deleteModel = new DeleteModel(context);

            var productId = 1;
            deleteModel.OnGet(productId);

            // Act
            var result = deleteModel.OnPost();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Null(context.Products.Find(productId)); 
        }

        [Fact]
        public void OnPost_InvalidProductId_ReturnsNotFound()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var deleteModel = new DeleteModel(context);

            var invalidProductId = 999;
            deleteModel.Product = new Product { Id = invalidProductId };

            // Act
            var result = deleteModel.OnPost();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}