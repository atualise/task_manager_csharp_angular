using MediatR;
using TaskService.Data;

namespace TaskService.Features.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteTaskCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(new object[] { request.Id }, cancellationToken);

            if (task == null || task.UserId != request.UserId)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
} 