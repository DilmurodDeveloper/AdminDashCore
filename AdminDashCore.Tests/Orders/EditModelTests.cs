using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Orders
{
    public class EditModelTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void OnGet_OrderExists_ReturnsPageResult()
        {
            // Arrange
            using var context = CreateContext();
            var client = new Client { Id = 1, Name = "Client 1" };
            var order = new Order { Id = 1, ClientId = 1, Client = client, Status = "Pending" };
            context.Clients.Add(client);
            context.Orders.Add(order);
            context.SaveChanges();

            var pageModel = new EditModel(context);

            // Act
            var result = pageModel.OnGet(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(pageModel.Order);
            Assert.Equal("Pending", pageModel.Order.Status);
        }

        [Fact]
        public void OnGet_OrderNotFound_ReturnsNotFound()
        {
            // Arrange
            using var context = CreateContext();
            var pageModel = new EditModel(context);

            // Act
            var result = pageModel.OnGet(999); 

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void OnPost_ValidModel_RedirectsToIndex()
        {
            // Arrange
            using var context = CreateContext();
            var client = new Client { Id = 1, Name = "Client 1" };
            var order = new Order { Id = 1, ClientId = 1, Client = client, Status = "Pending" };
            context.Clients.Add(client);
            context.Orders.Add(order);
            context.SaveChanges();

            var existingOrder = context.Orders.First(o => o.Id == 1);
            existingOrder.Status = "Shipped";

            var pageModel = new EditModel(context)
            {
                Order = existingOrder
            };

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }


        [Fact]
        public void OnPost_InvalidModel_ReturnsPage()
        {
            // Arrange
            using var context = CreateContext();
            var pageModel = new EditModel(context)
            {
                Order = new Order { Id = 1, ClientId = 1, Status = "" } 
            };

            pageModel.ModelState.AddModelError("Status", "Required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
