using TaskService.Models;
using TaskService.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskService.Services
{
    public class TaskManager
    {
        private readonly AppDbContext _context;

        public TaskManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TodoTask> CreateTask(TaskCreateDto taskDto)
        {
            var task = new TodoTask
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TodoTask?> UpdateTask(int id, TaskCreateDto taskDto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description ?? task.Description;
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<byte[]> ExportToExcel()
        {
            var tasks = await _context.Tasks.ToListAsync();
            // Por enquanto, retornando um array vazio
            // Você pode implementar a geração real do Excel posteriormente
            return new byte[0];
        }
    }
} 