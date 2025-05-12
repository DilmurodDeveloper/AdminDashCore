using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Products
{
    public class EditModelTests
    {
        private DbContextOptions<AppDbContext> _options;

        public EditModelTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void OnGet_ProductExists_ReturnsPageResult()
        {
            using var context = new AppDbContext(_options);
            var category = new Category { Id = 1, Name = "Category 1" };
            var product = new Product { Id = 1, Name = "Product 1", CategoryId = 1 };

            context.Categories.Add(category);
            context.Products.Add(product);
            context.SaveChanges();

            var model = new EditModel(context);
            var result = model.OnGet(1);

            Assert.IsType<PageResult>(result);
            Assert.NotNull(model.Product);
            Assert.Equal("Product 1", model.Product.Name);
        }

        [Fact]
        public void OnGet_ProductNotFound_ReturnsNotFound()
        {
            using var context = new AppDbContext(_options);
            var model = new EditModel(context);

            var result = model.OnGet(999); 

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void OnPost_ValidModel_UpdatesProductAndRedirects()
        {
            using (var seedContext = new AppDbContext(_options))
            {
                seedContext.Categories.Add(new Category { Id = 1, Name = "Category 1" });
                seedContext.Products.Add(new Product { Id = 1, Name = "Old Product", CategoryId = 1 });
                seedContext.SaveChanges();
            }

            using (var testContext = new AppDbContext(_options))
            {
                var model = new EditModel(testContext)
                {
                    Product = new Product { Id = 1, Name = "Updated Product", CategoryId = 1 }
                };

                var result = model.OnPost();

                Assert.IsType<RedirectToPageResult>(result);

                var updated = testContext.Products.Find(1);
                Assert.Equal("Updated Product", updated!.Name);
            }
        }

        [Fact]
        public void OnPost_InvalidModel_ReturnsPageResult()
        {
            using var context = new AppDbContext(_options);
            var model = new EditModel(context)
            {
                Product = new Product { Id = 1, Name = "", CategoryId = 1 } 
            };

            model.ModelState.AddModelError("Product.Name", "Required");

            var result = model.OnPost();

            Assert.IsType<PageResult>(result);
            Assert.NotNull(model.CategoryList); 
        }
    }
}
