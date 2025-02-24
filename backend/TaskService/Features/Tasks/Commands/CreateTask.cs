using MediatR;
using TaskService.Data;
using TaskService.Models;

namespace TaskService.Features.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TodoTask>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
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
                Description = request.Description,
                Status = Enum.Parse<TodoTaskStatus>(request.Status),
                Priority = Enum.Parse<TaskPriority>(request.Priority),
                UserId = request.UserId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync(cancellationToken);

            return task;
        }
    }
} 