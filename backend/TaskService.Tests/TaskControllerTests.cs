using Xunit;
using Moq;
using MediatR;
using TaskService.Controllers;
using TaskService.Data;
using TaskService.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TaskService.Tests
{
    public class TaskControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AppDbContext _context;
        private readonly Mock<TaskManager> _taskManagerMock;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;
            _context = new AppDbContext(options);
            
            _taskManagerMock = new Mock<TaskManager>(_context);
            
            _controller = new TaskController(
                _mediatorMock.Object,
                _context,
                _taskManagerMock.Object
            );
        }

        [Fact]
        public async Task GetTasks_ReturnsOkResult()
        {
            // Arrange & Act & Assert
            // Adicione seus testes aqui
        }
    }
}