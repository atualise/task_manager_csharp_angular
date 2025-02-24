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

        public async Task<TodoTask> CreateTask(TodoTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TodoTask?> UpdateTask(int id, TodoTask task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null) return null;

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.Priority = task.Priority;
            existingTask.CompletionDate = task.CompletionDate;

            await _context.SaveChangesAsync();
            return existingTask;
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