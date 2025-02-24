using MediatR;
using TaskService.Data;
using TaskService.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskService.Features.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<TodoTask?>
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime? CompletionDate { get; set; }
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
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == request.UserId, cancellationToken);

            if (task == null)
                return null;

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = Enum.Parse<TodoTaskStatus>(request.Status);
            task.Priority = Enum.Parse<TaskPriority>(request.Priority);
            task.CompletionDate = request.CompletionDate;

            await _context.SaveChangesAsync(cancellationToken);
            return task;
        }
    }
} 