using Xunit;
using Moq;
using TaskService.Models;
using TaskService.Data;
using TaskService.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskService.Tests
{
    public class TaskControllerTests
    {
        [Fact]
        public void CreateTask_ValidInput_ReturnsCreatedResult()
        {
            // Arrange
            var mockDbContext = new Mock<AppDbContext>();
            var controller = new TaskController(mockDbContext.Object);
            
            // Act
            var result = controller.CreateTask(new TaskCreateDto { Title = "Tarefa Válida" });

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void TestMethod()
        {
            // Seu código de teste aqui
        }
    }
}