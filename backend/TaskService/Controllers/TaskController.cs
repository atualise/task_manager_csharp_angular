using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        private readonly AppDbContext _context;
        private readonly TaskManager _taskManager;

        public TaskController(AppDbContext context, TaskManager taskManager)
        {
            _context = context;
            _taskManager = taskManager;
        }

        [HttpGet]
        public IActionResult GetTasks([FromQuery] TodoTaskStatus? status, [FromQuery] TaskPriority? priority)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var query = _context.Tasks.Where(t => t.UserId == userId);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);
            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            return Ok(query.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var task = new TodoTask
            {
                Title = request.Title,
                Description = request.Description ?? "",
                Status = request.Status,
                Priority = request.Priority,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            if (task.Title.Length < 5)
                return BadRequest("O título deve ter pelo menos 5 caracteres.");

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            if (task.UserId != userId)
                return Forbid();

            task.Title = request.Title;
            task.Description = request.Description ?? task.Description;
            task.Status = request.Status;
            task.Priority = request.Priority;

            await _context.SaveChangesAsync();
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            if (task.UserId != userId)
                return Forbid();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
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