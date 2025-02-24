import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskService } from '../../../core/services/task.service';
import { Task, TaskStatus, TaskPriority } from '../../../core/models/task.model';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent implements OnInit {
  form: FormGroup;
  statusOptions: TaskStatus[] = ['Aberto', 'EmAndamento', 'Concluido'];
  priorityOptions: TaskPriority[] = ['Baixa', 'Media', 'Alta'];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    public dialogRef: MatDialogRef<TaskFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Task | null
  ) {
    this.form = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(5)]],
      description: [''],
      status: ['Aberto', Validators.required],
      priority: ['Baixa', Validators.required],
      completionDate: [null]
    });
  }

  ngOnInit() {
    if (this.data) {
      // Converter a string de data para objeto Date se existir
      const completionDate = this.data.completionDate ? new Date(this.data.completionDate) : null;
  
      this.form.patchValue({
        ...this.data,
        completionDate: completionDate
      });
    }
  }

  save() {
    if (this.form.valid) {
      const taskData = {
        ...this.form.value,
        // Garantir que a data seja enviada no formato correto
        completionDate: this.form.value.completionDate ? 
          this.form.value.completionDate.toISOString() : null
      };
      
      if (this.data?.id) {
        this.taskService.updateTask(this.data.id, taskData).subscribe({
          next: () => this.dialogRef.close(true),
          error: (error) => console.error('Erro ao atualizar tarefa:', error)
        });
      } else {
        this.taskService.createTask(taskData).subscribe({
          next: () => this.dialogRef.close(true),
          error: (error) => console.error('Erro ao criar tarefa:', error)
        });
      }
    }
  }
}