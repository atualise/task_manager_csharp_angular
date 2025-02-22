using MediatR;
using TaskService.Data;
using TaskService.Models;

namespace TaskService.Features.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<TodoTask?>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TodoTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int UserId { get; set; }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TodoTask?>
    {
        private readonly AppDbContext _context;

        public UpdateTaskCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TodoTask?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (task == null || task.UserId != request.UserId)
                return null;

            task.Title = request.Title;
            task.Description = request.Description ?? task.Description;
            task.Status = request.Status;
            task.Priority = request.Priority;

            await _context.SaveChangesAsync(cancellationToken);
            return task;
        }
    }
} 