using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Orders
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
            model.Order = new Order
            {
                ClientId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 500.00m,
                Status = "Pending"
            };

            model.ClientList = new SelectList(new List<Client>
            {
                new Client { 
                    Id = 1, 
                    Name = "Client A" }
            }, "Id", "Name");

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("Index", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPost_InvalidModel_ReturnsPage()
        {
            var model = GetCreateModelWithContext();
            model.Order = new Order
            {
                ClientId = 1,  
                TotalAmount = -100, 
                Status = "Invalid" 
            };

            // Act
            var result = await model.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);

        }

    }
}
