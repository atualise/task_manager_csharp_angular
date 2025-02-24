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
    /// <summary>
    /// Controller para gerenciamento de tarefas
    /// </summary>
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

        /// <summary>
        /// Retorna a lista de tarefas do usuário
        /// </summary>
        /// <param name="status">Filtro opcional por status</param>
        /// <param name="priority">Filtro opcional por prioridade</param>
        /// <returns>Lista de tarefas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoTask>), StatusCodes.Status200OK)]
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

        /// <summary>
        /// Cria uma nova tarefa
        /// </summary>
        /// <param name="request">Dados da tarefa</param>
        /// <returns>Tarefa criada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TodoTask), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Atualiza uma tarefa existente
        /// </summary>
        /// <param name="id">ID da tarefa</param>
        /// <param name="dto">Dados atualizados da tarefa</param>
        /// <returns>Tarefa atualizada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TodoTask), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var command = new UpdateTaskCommand
            {
                Id = id,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                CompletionDate = dto.CompletionDate,
                UserId = userId
            };

            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Remove uma tarefa
        /// </summary>
        /// <param name="id">ID da tarefa</param>
        /// <returns>Nenhum conteúdo</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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