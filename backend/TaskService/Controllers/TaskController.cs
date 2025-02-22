using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TaskService.Features.Tasks.Commands;
using TaskService.Features.Tasks.Queries;
using TaskService.Models;
using TaskService.Data;
using TaskService.Services;
using ClosedXML.Excel;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;
        private readonly TaskManager _taskManager;

        public TaskController(IMediator mediator, AppDbContext context, TaskManager taskManager)
        {
            _mediator = mediator;
            _context = context;
            _taskManager = taskManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TodoTaskStatus? status, [FromQuery] TaskPriority? priority)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = new GetTasksQuery
            {
                UserId = userId,
                Status = status,
                Priority = priority
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTasks), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new UpdateTaskCommand
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var command = new DeleteTaskCommand
            {
                Id = id,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportTasks()
        {
            var excelBytes = await _taskManager.ExportToExcel();
            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "tarefas.xlsx"
            );
        }
    }
}