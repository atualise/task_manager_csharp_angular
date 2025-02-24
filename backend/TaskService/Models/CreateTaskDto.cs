namespace TaskService.Models
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TodoTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }
} 