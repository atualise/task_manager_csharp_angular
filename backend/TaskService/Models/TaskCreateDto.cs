using System;
using System.ComponentModel.DataAnnotations;

namespace TaskService.Models
{
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [MinLength(5, ErrorMessage = "O título deve ter pelo menos 5 caracteres")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required(ErrorMessage = "O status é obrigatório")]
        [EnumDataType(typeof(TodoTaskStatus), ErrorMessage = "Status inválido")]
        public TodoTaskStatus Status { get; set; }
        [Required(ErrorMessage = "A prioridade é obrigatória")]
        [EnumDataType(typeof(TaskPriority), ErrorMessage = "Prioridade inválida")]
        public TaskPriority Priority { get; set; }
    }
} 