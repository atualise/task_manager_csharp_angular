using Xunit;
using TaskService.Data;
using TaskService.Models;
using TaskService.Features.Tasks.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskService.Tests.Features.Tasks.Commands
{
    public class UpdateTaskCommandTests
    {
        [Fact]
        public async Task Should_Update_Existing_Task()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            using var context = new AppDbContext(options);
            
            var task = new TodoTask
            {
                Title = "Original Title",
                Description = "Original Description",
                Status = TodoTaskStatus.Aberto,
                Priority = TaskPriority.Baixa,
                UserId = 1
            };
            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            var handler = new UpdateTaskCommandHandler(context);
            var command = new UpdateTaskCommand
            {
                Id = task.Id,
                Title = "Updated Title",
                Description = "Updated Description",
                Status = TodoTaskStatus.EmAndamento.ToString(),
                Priority = TaskPriority.Alta.ToString(),
                UserId = 1,
                CompletionDate = DateTime.Now
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(TodoTaskStatus.EmAndamento.ToString(), result.Status.ToString());
            Assert.Equal(TaskPriority.Alta.ToString(), result.Priority.ToString());
            Assert.Equal(command.CompletionDate, result.CompletionDate);
        }

        [Fact]
        public async Task Should_Return_Null_When_Task_Not_Found()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            using var context = new AppDbContext(options);
            var handler = new UpdateTaskCommandHandler(context);
            
            var command = new UpdateTaskCommand
            {
                Id = 999,
                Title = "Updated Title",
                UserId = 1
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
} 