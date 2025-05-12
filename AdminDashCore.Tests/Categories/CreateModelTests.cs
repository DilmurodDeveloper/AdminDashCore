using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace AdminDashCore.Tests.Categories
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
        public async Task OnPostAsync_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var model = GetCreateModelWithContext();
            model.Category = new Category
            {
                Name = "New Category" 
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var redirectResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirectResult.PageName); 
        }

        [Fact]
        public async Task OnPostAsync_InvalidModel_ReturnsPage()
        {
            //Arrange
            var model = GetCreateModelWithContext();
            model.Category = new Category
            {
                Name = "" 
            };

            //Act
            model.ModelState.AddModelError("Name", "Required");
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
