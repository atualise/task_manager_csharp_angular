export type TaskStatus = 'Aberto' | 'EmAndamento' | 'Concluido';
export type TaskPriority = 'Baixa' | 'Media' | 'Alta';

export interface Task {
  id: number;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  createdAt: string;
  completionDate?: string;
  userId: number;
}

export interface CreateTaskDto {
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  completionDate?: string;
} 