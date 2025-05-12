using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Orders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Orders
{
    public class DeleteModelTests
    {
        private DbContextOptions<AppDbContext> _options;

        public DeleteModelTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
        }

        [Fact]
        public void OnGet_OrderExists_ReturnsPageResult()
        {
            using var context = new AppDbContext(_options);
            var client = new Client { Id = 1, Name = "Test Client" };
            var order = new Order { Id = 1, Client = client, ClientId = client.Id };

            context.Clients.Add(client);
            context.Orders.Add(order);
            context.SaveChanges();

            var model = new DeleteModel(context);
            var result = model.OnGet(1);

            Assert.IsType<PageResult>(result);
            Assert.NotNull(model.Order);
        }

        [Fact]
        public void OnGet_OrderNotFound_ReturnsNotFound()
        {
            using var context = new AppDbContext(_options);
            var model = new DeleteModel(context);

            var result = model.OnGet(999); 

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void OnPost_OrderExists_DeletesOrderAndRedirects()
        {
            using var context = new AppDbContext(_options);
            var client = new Client { Id = 1, Name = "Test Client" };
            var order = new Order { Id = 1, ClientId = 1, Client = client };

            context.Clients.Add(client);
            context.Orders.Add(order);
            context.SaveChanges();

            var model = new DeleteModel(context)
            {
                Order = new Order { Id = 1 } 
            };

            var result = model.OnPost();

            Assert.IsType<RedirectToPageResult>(result);
            Assert.Null(context.Orders.Find(1)); 
        }

        [Fact]
        public void OnPost_OrderNotFound_ReturnsNotFound()
        {
            using var context = new AppDbContext(_options);
            var model = new DeleteModel(context)
            {
                Order = new Order { Id = 999 } 
            };

            var result = model.OnPost();

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
