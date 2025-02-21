namespace TaskService.Models
{

    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TodoTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int UserId { get; set; }
    }
}