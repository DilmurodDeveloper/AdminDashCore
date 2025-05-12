using AdminDashCore.Data;
using AdminDashCore.Models;
using AdminDashCore.Pages.Admin.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AdminDashCore.Tests.Messages
{
    public class DeleteModelTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public void OnGet_MessageExists_ReturnsPageResult()
        {
            // Arrange
            using var context = CreateContext();
            var client = new Client { Id = 1, Name = "Test Client" };
            var message = new Message { Id = 1, Content = "Test Message", ClientId = 1, Client = client };
            context.Clients.Add(client);
            context.Messages.Add(message);
            context.SaveChanges();

            var deleteModel = new DeleteModel(context);

            // Act
            var result = deleteModel.OnGet(1);

            // Assert
            Assert.IsType<PageResult>(result);
            Assert.NotNull(deleteModel.Message);
            Assert.Equal("Test Message", deleteModel.Message.Content);
        }

        [Fact]
        public void OnGet_MessageNotFound_ReturnsNotFound()
        {
            // Arrange
            using var context = CreateContext();
            var deleteModel = new DeleteModel(context);

            // Act
            var result = deleteModel.OnGet(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void OnPost_MessageExists_DeletesAndRedirects()
        {
            // Arrange
            using var context = CreateContext();
            var message = new Message { Id = 1, Content = "To Be Deleted" };
            context.Messages.Add(message);
            context.SaveChanges();

            var deleteModel = new DeleteModel(context)
            {
                Message = new Message { Id = 1 } 
            };

            // Act
            var result = deleteModel.OnPost();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Null(context.Messages.Find(1)); 
        }

        [Fact]
        public void OnPost_MessageNotFound_ReturnsNotFound()
        {
            // Arrange
            using var context = CreateContext();
            var deleteModel = new DeleteModel(context)
            {
                Message = new Message { Id = 999 }
            };

            // Act
            var result = deleteModel.OnPost();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
