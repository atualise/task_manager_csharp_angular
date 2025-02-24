using Xunit;
using TaskService.Data;
using TaskService.Models;
using TaskService.Features.Tasks.Commands;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace TaskService.Tests.Features.Tasks.Commands
{
    public class CreateTaskCommandTests
    {
        [Fact]
        public async Task Should_Create_New_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            using var context = new AppDbContext(options);
            var handler = new CreateTaskCommandHandler(context);
            
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Test Description",
                Status = "Aberto",
                Priority = "Baixa",
                UserId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(TodoTaskStatus.Aberto, result.Status);
            Assert.Equal(TaskPriority.Baixa, result.Priority);
            Assert.Equal(command.UserId, result.UserId);
        }

        [Fact]
        public async Task Should_Create_Task_With_EmAndamento_Status()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            using var context = new AppDbContext(options);
            var handler = new CreateTaskCommandHandler(context);
            
            var command = new CreateTaskCommand
            {
                Title = "Test Task",
                Description = "Test Description",
                Status = "EmAndamento",
                Priority = "Media",
                UserId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(TodoTaskStatus.EmAndamento, result.Status);
            Assert.Equal(TaskPriority.Media, result.Priority);
        }
    }
} 