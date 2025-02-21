import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatToolbar } from '@angular/material/toolbar';
import { MatSort } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TaskService } from '../../../core/services/task.service';
import { TaskFormComponent } from '../../tasks/task-form/task-form.component';
import { Task } from '../../../core/models/task.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  dataSource = new MatTableDataSource<Task>();
  displayedColumns = ['id', 'title', 'description', 'status', 'priority', 'actions'];
  filters = { status: '', priority: '' };

  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private taskService: TaskService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks(this.filters).subscribe(tasks => {
      this.dataSource.data = tasks;
      this.dataSource.sort = this.sort;
    });
  }

  openTaskForm(task?: Task) {
    const dialogRef = this.dialog.open(TaskFormComponent, {
      width: '500px',
      data: task || null
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) this.loadTasks();
    });
  }

  deleteTask(id: number) {
    if (confirm('Tem certeza que deseja excluir esta tarefa?')) {
      this.taskService.deleteTask(id).subscribe({
        next: () => {
          this.snackBar.open('Tarefa excluída com sucesso!', 'Fechar', {
            duration: 3000
          });
          this.loadTasks();
        },
        error: (error) => {
          this.snackBar.open('Erro ao excluir tarefa', 'Fechar', {
            duration: 3000
          });
          console.error('Erro ao excluir tarefa:', error);
        }
      });
    }
  }

  applyFilter() {
    this.loadTasks();
  }

  exportToExcel() {
    this.taskService.exportTasks().subscribe(blob => {
      const a = document.createElement('a');
      const objectUrl = URL.createObjectURL(blob);
      a.href = objectUrl;
      a.download = 'tarefas.xlsx';
      a.click();
      URL.revokeObjectURL(objectUrl);
    });
  }
}