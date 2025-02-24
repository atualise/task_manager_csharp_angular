using Xunit;
using TaskService.Data;
using TaskService.Models;
using TaskService.Features.Tasks.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskService.Tests.Features.Tasks.Queries
{
    public class GetTasksQueryTests
    {
        [Fact]
        public async Task Should_Return_All_Tasks_For_User()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            using var context = new AppDbContext(options);
            
            // Add test data
            context.Tasks.AddRange(
                new TodoTask { Title = "Task 1", UserId = 1, Status = TodoTaskStatus.Aberto, Priority = TaskPriority.Baixa },
                new TodoTask { Title = "Task 2", UserId = 1, Status = TodoTaskStatus.EmAndamento, Priority = TaskPriority.Media },
                new TodoTask { Title = "Task 3", UserId = 2, Status = TodoTaskStatus.Concluido, Priority = TaskPriority.Alta }
            );
            await context.SaveChangesAsync();

            var handler = new GetTasksQueryHandler(context);
            var query = new GetTasksQuery { UserId = 1 };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, task => Assert.Equal(1, task.UserId));
        }

        [Fact]
        public async Task Should_Filter_Tasks_By_Status_And_Priority()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            using var context = new AppDbContext(options);
            
            context.Tasks.AddRange(
                new TodoTask { Title = "Task 1", UserId = 1, Status = TodoTaskStatus.Aberto, Priority = TaskPriority.Baixa },
                new TodoTask { Title = "Task 2", UserId = 1, Status = TodoTaskStatus.Aberto, Priority = TaskPriority.Alta },
                new TodoTask { Title = "Task 3", UserId = 1, Status = TodoTaskStatus.EmAndamento, Priority = TaskPriority.Baixa }
            );
            await context.SaveChangesAsync();

            var handler = new GetTasksQueryHandler(context);
            var query = new GetTasksQuery 
            { 
                UserId = 1,
                Status = TodoTaskStatus.Aberto,
                Priority = TaskPriority.Baixa
            };

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Single(result);
            var task = result.First();
            Assert.Equal(TodoTaskStatus.Aberto, task.Status);
            Assert.Equal(TaskPriority.Baixa, task.Priority);
        }
    }
} 