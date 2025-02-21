export type TaskStatus = 'Aberto' | 'EmAndamento' | 'Concluido';
export type TaskPriority = 'Baixa' | 'Media' | 'Alta';

export interface Task {
  id?: number;
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
  createdAt?: Date;
  updatedAt?: Date;
}

export interface CreateTaskDto extends Omit<Task, 'id' | 'createdAt' | 'updatedAt'> {
  title: string;
  description: string;
  status: TaskStatus;
  priority: TaskPriority;
} 