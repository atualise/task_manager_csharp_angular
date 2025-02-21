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
      priority: ['Baixa', Validators.required]
    });
  }

  ngOnInit() {
    if (this.data) {
      this.form.patchValue(this.data);
    }
  }

  save() {
    if (this.form.valid) {
      const formValue = this.form.value;
      const request = this.data?.id
        ? this.taskService.updateTask(this.data.id, formValue)
        : this.taskService.createTask(formValue);

      request.subscribe({
        next: (result) => {
          this.dialogRef.close(result);
        },
        error: (error) => {
          console.error('Erro ao salvar tarefa:', error);
        }
      });
    }
  }
}