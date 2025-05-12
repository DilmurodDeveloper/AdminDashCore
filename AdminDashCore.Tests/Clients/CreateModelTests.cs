using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Clients
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
            model.Client = new Client
            {
                Name = "Test", 
                Position = "Manager",
                Office = "HQ",
                Age = 30,
                StartDate = DateTime.Today,
                Salary = 50000
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var redirect = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirect.PageName);
        }

        [Fact]
        public async Task OnPostAsync_InvalidModel_ReturnsPage()
        {
            // Arrange
            var model = GetCreateModelWithContext();

            model.Client = new Client
            {
                Name = "", 
                Position = "Manager",
                Office = "HQ",
                Age = 30,
                StartDate = DateTime.Today,
                Salary = 50000
            };

            model.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result); 
        }
    }
}
