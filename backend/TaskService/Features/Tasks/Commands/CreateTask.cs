using MediatR;
using TaskService.Data;
using TaskService.Models;

namespace TaskService.Features.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TodoTask>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TodoTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int UserId { get; set; }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TodoTask>
    {
        private readonly AppDbContext _context;

        public CreateTaskCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TodoTask> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new TodoTask
            {
                Title = request.Title,
                Description = request.Description ?? "",
                Status = request.Status,
                Priority = request.Priority,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return task;
        }
    }
} 