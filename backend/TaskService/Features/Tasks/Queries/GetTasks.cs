using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskService.Data;
using TaskService.Models;

namespace TaskService.Features.Tasks.Queries
{
    public class GetTasksQuery : IRequest<List<TodoTask>>
    {
        public int UserId { get; set; }
        public TodoTaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
    }

    public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, List<TodoTask>>
    {
        private readonly AppDbContext _context;

        public GetTasksQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoTask>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Tasks.Where(t => t.UserId == request.UserId);

            if (request.Status.HasValue)
                query = query.Where(t => t.Status == request.Status.Value);
            if (request.Priority.HasValue)
                query = query.Where(t => t.Priority == request.Priority.Value);

            return await query.ToListAsync(cancellationToken);
        }
    }
} 